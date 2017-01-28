using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;
using FacebookConnect.Models;

namespace FacebookConnect
{
    public class MissingSettingsBanner : INotificationProvider
    {
        private readonly IOrchardServices _orchardServices;

        public MissingSettingsBanner(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IEnumerable<NotifyEntry> GetNotifications()
        {

            var facebookSettings = _orchardServices.WorkContext.CurrentSite.As<FacebookSettingsPart>();

            if (facebookSettings == null || string.IsNullOrWhiteSpace(facebookSettings.AppId) || string.IsNullOrWhiteSpace(facebookSettings.AppSecret) || string.IsNullOrWhiteSpace(facebookSettings.Permissions))
            {
                yield return new NotifyEntry { 
                    Message =
                    T("<b>Facebook Application Settings needs to be configured.</b><br/> { Configuration || Settings || Facebook Application Settings }"),
                    Type = NotifyType.Warning
                };
            }
        }
    }
}
