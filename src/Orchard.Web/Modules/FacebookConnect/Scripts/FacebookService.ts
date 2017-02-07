//http://www.dwmkerr.com/promises-in-angularjs-the-definitive-guide/
interface JQuery {
    modal(): JQuery;
}

module App.Services {

    declare function sprintf(format: string, ...val: any[]): string;

    export interface ISetting {
        apiEndpoint: string;
    }

    export interface IFb {
        login(callback: (response: any) => any): void;
        getLoginStatus(callback: (response: any) => any, forceGetLogInStatus: boolean): void;
        api(url: string, callback: (response: any) => any): void;
    }

    declare var FB: IFb;
    declare var Setting: ISetting;

    export class FacebookService {

        constructor(
            private $q: ng.IQService,
            private $http: ng.IHttpService,
            private $document: ng.IDocumentService) {
        }

        logIn(): ng.IPromise<any> {

            var deferred = this.$q.defer();
            FB.login((response: any) => {

                try {
                    if (response.status === 'connected') {

                        deferred.resolve(response.authResponse);

                    } else if (response.status === 'not_authorized') {
                        // The person is logged into Facebook, but not your app.
                        deferred.reject(response);
                    } else {
                        // The person is not logged into Facebook, so we're not sure if
                        // they are logged into this app or not.
                        deferred.reject("user not logged into Facebook");
                    }
                } catch (ex) {
                    deferred.reject(ex);
                }


            });
            return deferred.promise;
        }


        getLogInStatus(): ng.IPromise<any> {

            var deferred = this.$q.defer();
            const forceGetLogInStatus = true;
            FB.getLoginStatus((response: any) => {

                try {
                    // The response object is returned with a status field that lets the
                    // app know the current login status of the person.
                    // Full docs on the response object can be found in the documentation
                    // for FB.getLoginStatus().
                    if (response.status === 'connected') {
                        var user = {
                            facebookAppScopeUserId: response.authResponse.userID,
                            facebookAccessToken: response.authResponse.accessToken
                        };
                        deferred.resolve({ data: user });
                    } else if (response.status === 'not_authorized') {
                        // The person is logged into Facebook, but not your app.
                        deferred.reject("The person is logged into Facebook, but not your app");
                    } else {
                        // The person is not logged into Facebook, so we're not sure if
                        // they are logged into this app or not.
                        deferred.reject("The person is not logged into Facebook, so we're not sure if");
                    }
                } catch (ex) {
                    deferred.reject(ex);
                }

            }, forceGetLogInStatus);

            return deferred.promise;
        }


        getUserInfo(authResponse:any):ng.IPromise<any> {
            console.log(authResponse);
            var deferred = this.$q.defer();
            var graphApiUrl = sprintf('/%s?fields=picture.width(540).height(540),id,first_name,email', authResponse.userID);
            FB.api(graphApiUrl, (response: any) => {
                var userInfo:any = {};
                userInfo.facebookAppScopeUserId = response.id;
                userInfo.facebookToken = authResponse.accessToken;
                userInfo.profileUrl = response.picture.data.url;
                userInfo.name = response.first_name;
                userInfo.email = response.email;
                deferred.resolve(userInfo);
            });

            return deferred.promise;
        }

        isExistingUser(user: any) {
            var deferred = this.$q.defer();
            var url = sprintf("%s/users/existing?facebookId=%s",
                Setting.apiEndpoint, user.facebookAppScopeUserId);
            const method = "GET";
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
                .then((response: any) => {

                    var data = { existingUser: response.data.existingUser };
                    deferred.resolve({ data: data });

                }).catch((response: any) => {
                    deferred.reject(response);
                });

            return deferred.promise;
        }


        logInWithNewFacebookToken(user: any) {
            var deferred = this.$q.defer();
            var url = sprintf("%s/users/login/%s",
                Setting.apiEndpoint,
                user.facebookAppScopeUserId);

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

            this.$http(req).then((response: any) => {
                deferred.resolve({ data: { apiToken: response.data.apiToken } });
            }).catch((response: any) => {
                deferred.reject(response);
            });

            return deferred.promise;
        }

        isExistingUserWithEmail(user: any) {

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
                .then((response: any) => {
                    deferred.resolve({
                        data: {
                            existingUser: response.data.existingUser
                        }
                    });
                })
                .catch((response: any) => {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        updateUserWithFacebook(user: any) {

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
                .then((response: any) => {
                    deferred.resolve({});

                }).catch((response: any) => {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        registerNewUser(user: any) {
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
                .then((response: any) => {
                    deferred.resolve({ data: { userId: response } });
                }).catch((response: any) => {
                    deferred.reject({ data: response });
                });

            return deferred.promise;
        }

        showIntroLogInModal() {
            var jQueryObject = this.$document.find("#introLogIn");
            jQueryObject.modal();
        }

        requestUserLogIn() {
            var deferred = this.$q.defer();
            this.showIntroLogInModal();
            this.onSuccessLoggedIn(() => {
                deferred.resolve({});
            });

            this.onErrorLoggedIn(() => {
                deferred.reject({});
            });

            return deferred.promise;
        };

        onSuccessLoggedIn(callback: () => void): void {
            callback();
        }

        onErrorLoggedIn(callback: () => void): void {
            callback();
        }

    }

    angular.module("facebookConnect")
        .service("facebookService", ["$q", "$http", "$document", FacebookService]);
}