module App.Contorllers {

    $(document).on("fbLoaded", (e: JQueryEventObject, ...args: any[]) => {
        console.log("e=%s, args[0]=%s",e, args[0]);
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
        isLogIn = true;

        constructor(private $http: ng.IHttpService) {
            super();
        }

        showMessage(): void {
            this.delay(() => alert("Hello"), 2000);
        }

    }

    angular.module("facebookConnect")
        .controller("facebookLogInController", ["$http", FacebookLogInController]);

}