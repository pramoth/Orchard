using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Orchard;
using Facebook;
using FacebookConnect.Models;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Users.Events;
using Orchard.Users.Models;

namespace FacebookConnect.Controllers
{
    [HandleError]
    public class FacebookController : Controller
    {
        private readonly IOrchardServices _services;
        private readonly IAuthenticationService _auth;
        private readonly IMembershipService _membershipService;
        private readonly IUserEventHandler _userEventHandler;

        public FacebookController(
            IOrchardServices services,
            IAuthenticationService auth,
            IMembershipService membershipService,
            IUserEventHandler userEventHandler)
        {
            _services = services;
            _auth = auth;
            _membershipService = membershipService;
            _userEventHandler = userEventHandler;
        }

        [HttpPost]
        public ActionResult Connect(string facebookAccessToken)
        {
            // Acquire Facebook settings
            var settings = _services.WorkContext.CurrentSite.As<FacebookSettingsPart>();

            var client = new FacebookClient(facebookAccessToken);


            dynamic fbUser = client.Get("me");
            var email = (string)fbUser.email;

            // If already logged in update the account info
            var user = _auth.GetAuthenticatedUser();
            if (user != null)
            {
                var facebookUser = user.As<FacebookUserPart>();
                if (string.IsNullOrWhiteSpace(facebookUser.UserId))
                {
                    //update user facebook id
                    facebookUser.UserId = user.Id.ToString();
                }
            }
            // If not logged in check if exists in db and log on or redirect to register screen
            else
            {
                user = _services.ContentManager.Query<UserPart, UserPartRecord>()
                   .Where<UserPartRecord>(x => x.Email == email)
                   .List<IUser>()
                   .SingleOrDefault();

                if (user == null)
                {
                    // Create new user - redirect to form - there is no such binding between FB user Id and any Orchard user
                    //result = RedirectToAction("Register", "Account", new { Area = "Orchard.Users" });

                    var userParam = new CreateUserParams(
                        (string)fbUser.name,
                        GeneratePassword(8),
                        (string)fbUser.email,
                        null, null, true);

                    user = _membershipService.CreateUser(userParam);
                }

                var facebookUser = user.As<FacebookUserPart>();
                facebookUser.UserId = user.Id.ToString();

                //sign in
                _auth.SignIn(user, true);

                //update last log in, to make cookie valid
                _userEventHandler.LoggedIn(user);
            }


            return new JsonResult();
        }

        public ActionResult Connected()
        {
            //            // Acquire Facebook settings
            //            var settings = _services.WorkContext.CurrentSite.As<FacebookSettingsPart>();
            //
            //            var client = new FacebookApp(new FacebookSettings { AppId = settings.AppId, AppSecret = settings.AppSecret });
            //            var authorizer = new Authorizer(client) { Perms = settings.Permissions };
            //            if (authorizer.IsAuthorized())
            //            {
            //                return Redirect("~/");
            //            }
            return Redirect("~/");
        }

        public static string GeneratePassword(int resetPasswordLength)
        {
            // Create an array of characters to user for password reset.
            // Exclude confusing or ambiguous characters such as 1 0 l o i
            var characters = new[] { "2", "3", "4", "5", "6", "7", "8",
                "9", "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n",
                "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};

            var newPassword = new StringBuilder();
            var rnd = new Random();

            for (var index = 0; index < resetPasswordLength; index++)
            {
                newPassword.Append(characters[rnd.Next(characters.Length)]);
            }
            return newPassword.ToString();
        }


    }
}