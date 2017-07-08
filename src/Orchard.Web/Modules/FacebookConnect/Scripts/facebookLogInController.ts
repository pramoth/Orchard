module App.Contorllers {

    $(document).on("fbLoaded", (e: JQueryEventObject, ...args: any[]) => {
        console.log("e=%s, args[0]=%s", e, args[0]);
    });

    export abstract class ControllerBase {

        delay(delayExpression: Function, delay: number): void {
            window.setTimeout(() => {
                delayExpression();
            }, delay);
        }

        redirect(url: string): void {
            window.location.href = url;
        }

        getCurrentUrl(): string {
            return window.location.href;
        };
    }

    class FacebookLogInController extends ControllerBase {

        userName = "";
        isLogIn = false;

        constructor(
            private facebookService: Services.FacebookService,
            private $q: ng.IQService) {
            super();
        }

        logIn() {
            console.log("log in called");

            this.facebookService.getLogInStatus()
                .then((response: any) => {
                    if (response.status === 'connected') {
                        return this.$q.resolve(response);//wrap response as queue promise
                    } else {
                        return this.facebookService.logIn();
                    }
                })
                .then((response: any) => {
                    //always get new user info
                    return this.facebookService.getUserInfo(response);
                })
                .then((userInfo: any) => {
                    console.log(userInfo);
                    //connect user with server side
                    return this.facebookService.connect(userInfo);
                })
                .then((response: any) => {
                    //action after  logged in successfully
                    this.isLogIn = true;
                    this.redirect("/");
                })
                .catch((response: any) => {
                    console.log(response);
                    alert("Error, please reload page and try again");
                })
                .finally(() => {
                    console.log("finally");
                });
        }

    }

    //register controller to module
    angular.module("facebookConnect")
        .controller("facebookLogInController",
        ["facebookService", "$q", FacebookLogInController]);

}