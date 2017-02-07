var App;
(function (App) {
    var Services;
    (function (Services) {
        var FacebookService = (function () {
            function FacebookService($q, $http, $document) {
                this.$q = $q;
                this.$http = $http;
                this.$document = $document;
            }
            FacebookService.prototype.logIn = function () {
                var deferred = this.$q.defer();
                FB.login(function (response) {
                    try {
                        if (response.status === 'connected') {
                            deferred.resolve(response.authResponse);
                        }
                        else if (response.status === 'not_authorized') {
                            deferred.reject(response);
                        }
                        else {
                            deferred.reject("user not logged into Facebook");
                        }
                    }
                    catch (ex) {
                        deferred.reject(ex);
                    }
                });
                return deferred.promise;
            };
            FacebookService.prototype.getLogInStatus = function () {
                var deferred = this.$q.defer();
                var forceGetLogInStatus = true;
                FB.getLoginStatus(function (response) {
                    try {
                        if (response.status === 'connected') {
                            var user = {
                                facebookAppScopeUserId: response.authResponse.userID,
                                facebookAccessToken: response.authResponse.accessToken
                            };
                            deferred.resolve({ data: user });
                        }
                        else if (response.status === 'not_authorized') {
                            deferred.reject("The person is logged into Facebook, but not your app");
                        }
                        else {
                            deferred.reject("The person is not logged into Facebook, so we're not sure if");
                        }
                    }
                    catch (ex) {
                        deferred.reject(ex);
                    }
                }, forceGetLogInStatus);
                return deferred.promise;
            };
            FacebookService.prototype.getUserInfo = function (authResponse) {
                console.log(authResponse);
                var deferred = this.$q.defer();
                var graphApiUrl = sprintf('/%s?fields=picture.width(540).height(540),id,first_name,email', authResponse.userID);
                FB.api(graphApiUrl, function (response) {
                    var userInfo = {};
                    userInfo.facebookAppScopeUserId = response.id;
                    userInfo.facebookToken = authResponse.accessToken;
                    userInfo.profileUrl = response.picture.data.url;
                    userInfo.name = response.first_name;
                    userInfo.email = response.email;
                    deferred.resolve(userInfo);
                });
                return deferred.promise;
            };
            FacebookService.prototype.isExistingUser = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("%s/users/existing?facebookId=%s", Setting.apiEndpoint, user.facebookAppScopeUserId);
                var method = "GET";
                var req = {
                    method: method,
                    url: url,
                    headers: {
                        'Content-Type': "application/json",
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json'
                    }
                };
                this.$http(req)
                    .then(function (response) {
                    var data = { existingUser: response.data.existingUser };
                    deferred.resolve({ data: data });
                }).catch(function (response) {
                    deferred.reject(response);
                });
                return deferred.promise;
            };
            FacebookService.prototype.logInWithNewFacebookToken = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("%s/users/login/%s", Setting.apiEndpoint, user.facebookAppScopeUserId);
                var method = "PUT";
                var req = {
                    method: method,
                    url: url,
                    headers: {
                        'Content-Type': "application/json",
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json'
                    },
                    data: {
                        facebookAccessToken: user.facebookAccessToken
                    }
                };
                this.$http(req).then(function (response) {
                    deferred.resolve({ data: { apiToken: response.data.apiToken } });
                }).catch(function (response) {
                    deferred.reject(response);
                });
                return deferred.promise;
            };
            FacebookService.prototype.isExistingUserWithEmail = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("%s/users/existing?email=%s", Setting.apiEndpoint, user.emailFromFacebook);
                var method = "GET";
                var req = {
                    method: method,
                    url: url,
                    headers: {
                        'Content-Type': "application/json",
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json'
                    }
                };
                this.$http(req)
                    .then(function (response) {
                    deferred.resolve({
                        data: {
                            existingUser: response.data.existingUser
                        }
                    });
                })
                    .catch(function (response) {
                    deferred.reject(response);
                });
                return deferred.promise;
            };
            FacebookService.prototype.updateUserWithFacebook = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("%s/users/facebook", Setting.apiEndpoint);
                var method = "PUT";
                var req = {
                    method: method,
                    url: url,
                    headers: {
                        'Content-Type': "application/json",
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json'
                    },
                    data: user
                };
                this.$http(req)
                    .then(function (response) {
                    deferred.resolve({});
                }).catch(function (response) {
                    deferred.reject(response);
                });
                return deferred.promise;
            };
            FacebookService.prototype.registerNewUser = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("%s/users/facebook", Setting.apiEndpoint);
                var method = "POST";
                var req = {
                    method: method,
                    url: url,
                    headers: {
                        'Content-Type': "application/json",
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json'
                    },
                    data: user
                };
                this.$http(req)
                    .then(function (response) {
                    deferred.resolve({ data: { userId: response } });
                }).catch(function (response) {
                    deferred.reject({ data: response });
                });
                return deferred.promise;
            };
            FacebookService.prototype.showIntroLogInModal = function () {
                var jQueryObject = this.$document.find("#introLogIn");
                jQueryObject.modal();
            };
            FacebookService.prototype.requestUserLogIn = function () {
                var deferred = this.$q.defer();
                this.showIntroLogInModal();
                this.onSuccessLoggedIn(function () {
                    deferred.resolve({});
                });
                this.onErrorLoggedIn(function () {
                    deferred.reject({});
                });
                return deferred.promise;
            };
            ;
            FacebookService.prototype.onSuccessLoggedIn = function (callback) {
                callback();
            };
            FacebookService.prototype.onErrorLoggedIn = function (callback) {
                callback();
            };
            return FacebookService;
        }());
        Services.FacebookService = FacebookService;
        angular.module("facebookConnect")
            .service("facebookService", ["$q", "$http", "$document", FacebookService]);
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
