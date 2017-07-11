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
        private readonly IOrchardServices services;
        private readonly IAuthenticationService auth;

        public FacebookConnectPartDriver(IOrchardServices services, IAuthenticationService auth) {
            this.services = services;
           this. auth = auth;
        }

        protected override DriverResult Display(FacebookConnectPart part, string displayType, dynamic shapeHelper)
        {
            // Acquire Facebook settings
            var settings = services.WorkContext.CurrentSite.As<FacebookConnectSettingsPart>();
            var user = auth.GetAuthenticatedUser();
            var facebookUserContent = user.ContentItem.As<FacebookUserPart>();
            var isConnected = user != null && facebookUserContent !=null;
            
            // Building the FB authentication url to redirect unauthorized users to
            var requestUrl = HttpContext.Current.Request.Url;
            var redirUrl = $"{requestUrl.Scheme}://{requestUrl.Authority}/FacebookConnect";
                

            return ContentShape("Parts_Facebook_Connect",
                () => shapeHelper.Parts_Facebook_Connect(IsConnected: isConnected, RedirectUrl: redirUrl, Settings: settings));
        }

    }
}