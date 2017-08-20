using Orchard;
using System;
using System.Web.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Notify;
using Orchard.ContentManagement;
using CodeSanook.Comment.Models;
using Orchard.Localization;
using CodeSanook.FacebookConnect.Models;
using AutoMapper;

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
            var comment = contentManager.New("Comment");
            var commentPart = comment.As<CommentPart>();
            var facebookUserPartForComment = comment.As<FacebookUserPart>();

            //bind data
            var editorShape = contentManager.UpdateEditor(commentPart, this);
            if (!ModelState.IsValidField("Comments.CommentBody"))
            {
                notifier.Error(T("Comment is mandatory"));
            }

            if (ModelState.IsValid)
            {
                var currentFacebookUserPart = user.ContentItem.As<FacebookUserPart>();
                Mapper.Initialize(cfg => cfg.CreateMap<FacebookUserPart, FacebookUserPart>());
                Mapper.Map<FacebookUserPart, FacebookUserPart>(currentFacebookUserPart, facebookUserPartForComment);

                facebookUserPartForComment = comment.As<FacebookUserPart>();
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