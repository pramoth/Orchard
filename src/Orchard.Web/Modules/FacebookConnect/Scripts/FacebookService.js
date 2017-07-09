var App;
(function (App) {
    var Services;
    (function (Services) {
        var FbScope = (function () {
            function FbScope(requiredPermissions) {
                this.requiredPermissions = requiredPermissions;
                this.return_scopes = true;
                this.scope = requiredPermissions.join(",");
            }
            FbScope.prototype.validateHasAllRequiredPermissions = function (grantedPermissions) {
                for (var index = 0; index < this.requiredPermissions.length - 1; index++) {
                    var permissionToCheck = this.requiredPermissions[index];
                    var foundIndex = grantedPermissions.indexOf(permissionToCheck);
                    console.log("found permission at index " + foundIndex);
                    if (foundIndex < 0) {
                        throw new Error("Please log in again and allow " + permissionToCheck + " permission.");
                    }
                }
            };
            return FbScope;
        }());
        Services.FbScope = FbScope;
        var FacebookService = (function () {
            function FacebookService($q, $http, $document) {
                this.$q = $q;
                this.$http = $http;
                this.$document = $document;
                this.fbScope = new FbScope(["email", "public_profile"]);
            }
            FacebookService.prototype.getAntiForgeryToken = function () {
                return $("[name='__RequestVerificationToken']").val();
            };
            FacebookService.prototype.logIn = function () {
                var _this = this;
                var deferred = this.$q.defer();
                FB.login(function (response) {
                    try {
                        if (response.status === 'connected') {
                            var grantedScopes = response.authResponse.grantedScopes;
                            _this.fbScope.validateHasAllRequiredPermissions(grantedScopes);
                            deferred.resolve(response);
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
                }, this.fbScope);
                return deferred.promise;
            };
            FacebookService.prototype.getLogInStatus = function () {
                var deferred = this.$q.defer();
                var forceGetLogInStatus = true;
                FB.getLoginStatus(function (response) {
                    try {
                        console.log("get log in status \n%o\n", response);
                        deferred.resolve(response);
                    }
                    catch (ex) {
                        deferred.reject(ex);
                    }
                }, forceGetLogInStatus);
                return deferred.promise;
            };
            FacebookService.prototype.getUserInfo = function (response) {
                var authResponse = response.authResponse;
                var grantedScopes = response.authResponse.grantedScopes;
                console.log("grantedScopes \n%o\n", grantedScopes);
                var deferred = this.$q.defer();
                var graphApiUrl = sprintf('/%s?fields=picture.width(540).height(540),id,first_name,last_name,email', authResponse.userID);
                FB.api(graphApiUrl, function (queryResponse) {
                    var userInfo = {};
                    userInfo.facebookAccessToken = authResponse.accessToken;
                    userInfo.facebookAppScopeUserId = queryResponse.id;
                    userInfo.profilePictureUrl = queryResponse.picture.data.url;
                    userInfo.firstName = queryResponse.first_name;
                    userInfo.lastName = queryResponse.last_name;
                    userInfo.email = queryResponse.email;
                    deferred.resolve(userInfo);
                });
                return deferred.promise;
            };
            FacebookService.prototype.connect = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("/facebook/connect/");
                var method = "POST";
                var req = {
                    method: method,
                    url: url,
                    headers: {
                        'Content-Type': "application/json",
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json',
                        '__RequestVerificationToken': this.getAntiForgeryToken()
                    },
                    data: user
                };
                this.$http(req).then(function (response) {
                    deferred.resolve({});
                }).catch(function (response) {
                    deferred.reject(response);
                });
                return deferred.promise;
            };
            FacebookService.prototype.isExistingUser = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("%s/users/existing?facebookId=%s", Setting.apiEndpoint, user.facebookAppScopeUserId);
                var req = {
                    method: "GET",
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
            FacebookService.prototype.isExistingUserWithEmail = function (user) {
                var deferred = this.$q.defer();
                var url = sprintf("%s/users/existing?email=%s", Setting.apiEndpoint, user.emailFromFacebook);
                var req = {
                    method: "GET",
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
                var req = {
                    method: "PUT",
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
                var req = {
                    method: "POST",
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
