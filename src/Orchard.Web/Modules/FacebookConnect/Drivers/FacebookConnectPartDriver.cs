using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using FacebookConnect.Models;
using Orchard.Security;

namespace FacebookConnect.Drivers
{
    public class FacebookConnectPartDriver : ContentPartDriver<FacebookConnectPart>
    {
        private readonly IOrchardServices _services;
        private readonly IAuthenticationService _auth;

        public FacebookConnectPartDriver(IOrchardServices services, IAuthenticationService auth) {
            _services = services;
            _auth = auth;
        }

        protected override DriverResult Display(FacebookConnectPart part, string displayType, dynamic shapeHelper)
        {
            // Acquire Facebook settings
            var settings = _services.WorkContext.CurrentSite.As<FacebookSettingsPart>();
            var u = _auth.GetAuthenticatedUser();
            var isConnected = u != null && string.IsNullOrWhiteSpace(u.As<FacebookUserPart>().UserId);
            

            // Building the FB authentication url to redirect unauthorized users to
            var requestUrl = HttpContext.Current.Request.Url;
            var redirUrl = string.Format("{0}://{1}/FacebookConnect",
                          requestUrl.Scheme,
                          requestUrl.Authority);
                

            return ContentShape("Parts_Facebook_Connect",
                () => shapeHelper.Parts_Facebook_Connect(IsConnected: isConnected, RedirectUrl: redirUrl, Settings: settings));
        }

    }
}