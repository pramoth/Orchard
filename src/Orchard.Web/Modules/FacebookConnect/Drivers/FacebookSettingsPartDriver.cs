using Orchard.ContentManagement.Drivers;
using FacebookConnect.Models;
using Orchard.UI.Notify;
using Orchard.Security;
using Orchard;
using Orchard.Localization;
using Orchard.ContentManagement;

namespace FacebookConnect.Drivers
{
    public class FacebookSettingsPartDriver : ContentPartDriver<FacebookSettingsPart>
    {
        public FacebookSettingsPartDriver(
            INotifier notifier,
            IAuthorizationService authorizationService,
            IAuthenticationService authenticationService,
            IOrchardServices services)
        {
            _notifier = notifier;
            _authorizationService = authorizationService;
            _authenticationService = authenticationService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private const string TemplateName = "Parts/Facebook.Settings";
        private readonly INotifier _notifier;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthenticationService _authenticationService;

        protected override DriverResult Editor(FacebookSettingsPart part, dynamic shapeHelper)
        {
            if (!_authorizationService.TryCheckAccess(Permissions.EditSettings, _authenticationService.GetAuthenticatedUser(), part))
                return null;

            return ContentShape("Parts_Facebook_Settings",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(FacebookSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (!_authorizationService.TryCheckAccess(Permissions.EditSettings, _authenticationService.GetAuthenticatedUser(), part))
                return null;

            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                _notifier.Information(T("Facebook Settings Saved"));
            }
            else
            {
                _notifier.Error(T("Error during facebook settings update!"));
            }
            return Editor(part, shapeHelper);
        }
    }
}