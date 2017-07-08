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
using Orchard.Mvc.Extensions;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Themes;

namespace FacebookConnect.Controllers
{
    [HandleError, Themed]
    public class FacebookController : Controller
    {
        private readonly IOrchardServices services;
        private readonly IAuthenticationService auth;
        private readonly IMembershipService membershipService;
        private readonly IUserEventHandler userEventHandler;

        //property injection
        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public FacebookController(
            IOrchardServices services,
            IAuthenticationService auth,
            IMembershipService membershipService,
            IUserEventHandler userEventHandler)
        {
            this.services = services;
            this.auth = auth;
            this.membershipService = membershipService;
            this.userEventHandler = userEventHandler;

            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }



        [AlwaysAccessible]
        public ActionResult Connect(string returnUrl)
        {
            if (auth.GetAuthenticatedUser() != null)
                return this.RedirectLocal(returnUrl);

            var shape = services.New.FacebookLogIn().Title(T("Log On").Text);
            return new ShapeResult(this, shape);
        }

        [AlwaysAccessible]
        [HttpPost]
        public ActionResult Connect(FacebookLogInRequest request, FormCollection form)
        {
            // Acquire Facebook settings
            var settings = services.WorkContext.CurrentSite.As<FacebookSettingsPart>();

            var client = new FacebookClient(request.FacebookAccessToken);

            //https://developers.facebook.com/tools/explorer/?method=GET&path=me%3Ffields%3Dpicture.width(200).height(200)%2Cemail&version=v2.9
            var query = "me?fields=picture.height(200).width(200),email,first_name,last_name";
            dynamic fbUser = client.Get(query);
            var email = (string)fbUser.email;
            var imageUrl = fbUser.picture.data.url;

            // If already logged in update the account info
            var user = auth.GetAuthenticatedUser();
            if (user != null)
            {
                var facebookUser = user.ContentItem.As<FacebookUserPart>();
                if (facebookUser != null)
                {
                    //update user Facebook profile 
                    facebookUser.FirstName = (string)fbUser.first_name;
                    facebookUser.LastName = (string)fbUser.last_name;
                    facebookUser.ProfilePictureUrl = imageUrl;
                }
            }
            // If not logged in check if exists in db and log on or redirect to register screen
            else
            {
                user = services.ContentManager.Query<UserPart, UserPartRecord>()
                   .Where<UserPartRecord>(x => x.Email == email)
                   .List<IUser>()
                   .SingleOrDefault();

                if (user == null)
                {
                    var userParam = new CreateUserParams(
                        (string)fbUser.first_name,
                        GeneratePassword(8),
                        email,
                        null, null, true);

                    user = membershipService.CreateUser(userParam);
                }

                //relationship match with field UserId
                var facebookUser = user.ContentItem.As<FacebookUserPart>();
                facebookUser.FirstName = (string)fbUser.first_name;
                facebookUser.LastName = (string)fbUser.last_name;
                facebookUser.ProfilePictureUrl = imageUrl;

                //sign in
                auth.SignIn(user, true);

                //update last log in, to make cookie valid
                userEventHandler.LoggedIn(user);
            }

            return new JsonResult();
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