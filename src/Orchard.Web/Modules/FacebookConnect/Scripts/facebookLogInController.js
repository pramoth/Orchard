var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Contorllers;
    (function (Contorllers) {
        $(document).on("fbLoaded", function (e) {
            var args = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                args[_i - 1] = arguments[_i];
            }
            console.log("e=%s, args[0]=%s", e, args[0]);
        });
        var ControllerBase = (function () {
            function ControllerBase() {
            }
            ControllerBase.prototype.delay = function (delayExpression, delay) {
                window.setTimeout(function () {
                    delayExpression();
                }, delay);
            };
            ControllerBase.prototype.redirect = function (url) {
                window.location.href = url;
            };
            ControllerBase.prototype.getCurrentUrl = function () {
                return window.location.href;
            };
            ;
            return ControllerBase;
        }());
        Contorllers.ControllerBase = ControllerBase;
        var FacebookLogInController = (function (_super) {
            __extends(FacebookLogInController, _super);
            function FacebookLogInController($http) {
                _super.call(this);
                this.$http = $http;
                this.userName = "aaron";
                this.isLogIn = true;
            }
            FacebookLogInController.prototype.showMessage = function () {
                this.delay(function () { return alert("Hello"); }, 2000);
            };
            return FacebookLogInController;
        }(ControllerBase));
        angular.module("facebookConnect")
            .controller("facebookLogInController", ["$http", FacebookLogInController]);
    })(Contorllers = App.Contorllers || (App.Contorllers = {}));
})(App || (App = {}));
