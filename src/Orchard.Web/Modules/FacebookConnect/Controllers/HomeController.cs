using System;
using System.Linq;
using System.Web.Mvc;
using Facebook.Web.Mvc;
using Orchard;
using Facebook;
using FacebookConnect.Models;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Users.Models;
using Authorizer = Facebook.Web.Authorizer;

namespace FacebookConnect.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IOrchardServices _services;
        private readonly IAuthenticationService _auth;
        private readonly IMembershipService _membershipService;

        public HomeController(IOrchardServices services, IAuthenticationService auth, IMembershipService membershipService)
        {
            _services = services;
            _auth = auth;
            _membershipService = membershipService;
        }

        public ActionResult Connect()
        {
            // Acquire Facebook settings
            var settings = _services.WorkContext.CurrentSite.As<FacebookSettingsPart>();
            
            ActionResult result = Redirect("~/");

            var client = new FacebookApp(new FacebookSettings { AppId = settings.AppId, AppSecret = settings.AppSecret });
            var authorizer = new Authorizer(client);
            if (authorizer.IsAuthorized())
            {
                result = Redirect("~/");
                var session = client.Session;
                var u = _auth.GetAuthenticatedUser();

                dynamic fbUser = client.Get("me");
                string mail = (string)fbUser.email;

                // If already logged in update the account info
                if (u != null) {
                    var current = u.As<FacebookUserPart>();
                    if (string.IsNullOrWhiteSpace(current.UserId))
                    {
                        current.UserId = session.UserId;   
                    }
                }
                // If not logged in check if exists in db and log on or redirect to register screen
                else
                {
                    var user = _services
                        .ContentManager
                        .Query<UserPart, UserPartRecord>()
                        .Where<UserPartRecord>(x => x.Email == mail).List<IUser>().SingleOrDefault();

                    if (user == null)
                    {
                        // Create new user - redirect to form - there is no such binding between FB user Id and any Orchard user
                        //result = RedirectToAction("Register", "Account", new { Area = "Orchard.Users" });

                        user = _membershipService.CreateUser(new CreateUserParams((string)fbUser.name, "SomeP@ssw00rd", (string)fbUser.email, "", "H&I^T^&***Y^", true
                                                          ));
                    }

                    _auth.SignIn(user, true);
                }
            }
            return result;
        }
        public ActionResult Connected() {
            // Acquire Facebook settings
            var settings = _services.WorkContext.CurrentSite.As<FacebookSettingsPart>();

            var client = new FacebookApp(new FacebookSettings { AppId = settings.AppId, AppSecret = settings.AppSecret });
            var authorizer = new Authorizer(client) {Perms = settings.Permissions};
            if (authorizer.IsAuthorized()) {
                return Redirect("~/");
            }
            return Redirect("~/");
        }
    }
}