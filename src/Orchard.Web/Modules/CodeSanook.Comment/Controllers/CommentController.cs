using Orchard;
using Orchard.Core.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Notify;
using Orchard.ContentManagement;
using CodeSanook.Comment.Models;
using Orchard.Localization;

namespace CodeSanook.Comment.Controllers
{
    public class CommentController : Controller, IUpdateModel
    {
        private readonly IOrchardServices orchardService;
        private readonly IAuthenticationService auth;
        private readonly INotifier notifier;
        private readonly IContentManager contentManager;

        public Localizer T { get; set; }
        public CommentController(
            IOrchardServices orchardService,
            IAuthenticationService auth,
            INotifier notifier,
            IContentManager contentManager)
        {
            this.orchardService = orchardService;
            this.auth = auth;
            this.notifier = notifier;
            this.contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public void AddModelError(string key, LocalizedString errorMessage)
        {
            throw new NotImplementedException();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(string returnUrl)
        {
            var user = auth.GetAuthenticatedUser();
            if (user == null)
            {
                return this.RedirectLocal(returnUrl, "~/");
            }

            //new comment item and get comment part
            var comment = contentManager.New<CommentPart>("Comment");

            //bind data
            var editorShape = contentManager.UpdateEditor(comment, this);

            if (!ModelState.IsValidField("Comments.CommentBody"))
            {
                notifier.Error(T("Comment is mandatory"));
            }

            if (ModelState.IsValid)
            {
                contentManager.Create(comment);
                //var commentPart = commentItem.As<CommentPart>();
            }

            return this.RedirectLocal(returnUrl, "~/");
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }
    }
}