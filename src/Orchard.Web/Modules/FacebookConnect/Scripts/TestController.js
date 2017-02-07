var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Controllers;
    (function (Controllers) {
        var ControllerBase = App.Contorllers.ControllerBase;
        var TestController = (function (_super) {
            __extends(TestController, _super);
            function TestController() {
                _super.apply(this, arguments);
            }
            return TestController;
        }(ControllerBase));
    })(Controllers = App.Controllers || (App.Controllers = {}));
})(App || (App = {}));
