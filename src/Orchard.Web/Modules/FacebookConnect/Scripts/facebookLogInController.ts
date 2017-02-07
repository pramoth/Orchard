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

        userName = "aaron";
        isLogIn = false;

        constructor(private facebookService: Services.FacebookService) {
            super();
        }

        logIn() {
            this.facebookService.logIn()
                .then((authResponse: any) => {
                    return this.facebookService.getUserInfo(authResponse);
                })
                .then((userInfo: any) => {
                    console.log(userInfo);
                    this.isLogIn = true;
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

    angular.module("facebookConnect")
        .controller("facebookLogInController", ["facebookService", FacebookLogInController]);

}