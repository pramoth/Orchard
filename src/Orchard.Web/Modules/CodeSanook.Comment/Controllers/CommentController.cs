using Orchard;
using System;
using System.Web.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Notify;
using Orchard.ContentManagement;
using CodeSanook.Comment.Models;
using Orchard.Localization;
using Orchard.Mvc;

namespace CodeSanook.Comment.Controllers
{
    public class CommentController : Controller, IUpdateModel
    {
        private readonly IOrchardServices orchardService;
        private readonly IAuthenticationService auth;
        private readonly INotifier notifier;
        private readonly IContentManager contentManager;
        private IHttpContextAccessor httpContextAccessor;

        public Localizer T { get; set; }
        public CommentController(
            IOrchardServices orchardService,
            IAuthenticationService auth,
            INotifier notifier,
            IContentManager contentManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.orchardService = orchardService;
            this.auth = auth;
            this.notifier = notifier;
            this.contentManager = contentManager;
            this.httpContextAccessor = httpContextAccessor;
            T = NullLocalizer.Instance;

        }

        public void AddModelError(string key, LocalizedString errorMessage)
        {
            throw new NotImplementedException();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(string returnUrl)
        {
            //new comment item and get comment part
            var comment = contentManager.New("Comment");
            var commentPart = comment.As<CommentPart>();

            //bind data
            var editorShape = contentManager.UpdateEditor(commentPart, this);

            var user = auth.GetAuthenticatedUser();
            if (user == null)
            {
                var httpContext = httpContextAccessor.Current();
                httpContext.Session["tempCommentPart"] = commentPart;

                notifier.Warning(T("Please log in with your Facebook"));
                var logOnUrl = string.Format("~/Users/Account/LogOn?ReturnUrl={0}", returnUrl);
                return this.Redirect(logOnUrl);
            }

            if (!ModelState.IsValidField("Comments.CommentBody"))
            {
                notifier.Error(T("Comment is mandatory"));
            }

            if (ModelState.IsValid)
            {
                contentManager.Create(comment);
            }

            return this.RedirectLocal(returnUrl, "~/");
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }
    }
}