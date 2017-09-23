using CodeSanook.Comment.Models;
using CodeSanook.FacebookConnect.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Mvc;
using Orchard.Security;
using System.Collections.Generic;
using System.Linq;

namespace CodeSanook.Comment.Drivers
{
    public class CommentContainerPartDriver : ContentPartDriver<CommentContainerPart>
    {
        private readonly IContentManager contentManager;
        private readonly IAuthenticationService auth;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CommentContainerPartDriver(IContentManager contentManager, IAuthenticationService auth, IHttpContextAccessor httpContextAccessor)
        {
            this.contentManager = contentManager;
            this.auth = auth;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override string Prefix
        {
            get { return "CommentContainerPart"; }
        }

        protected override DriverResult Display(CommentContainerPart part, string displayType, dynamic shapeHelper)
        {
            var contentItemId = part.ContentItem.Id;
            var user = auth.GetAuthenticatedUser();

            if (displayType == "Detail")
            {
                var commentList = new List<CommentItemViewModel>();
                var comments = contentManager.HqlQuery().ForType("Comment")
                     .Where(alias => alias.ContentPartRecord<CommentPartRecord>(), s => s.Eq("ContentItemId", contentItemId))
                     .OrderBy(alias => alias.ContentPartRecord<CommonPartRecord>(), order => order.Desc("CreatedUtc"))
                     .List()
                     .ToList();

                if (comments.Any())
                {
                    var userIds = comments.Select(c => c.As<CommonPart>().Owner.Id).ToArray();
                    var users = contentManager.HqlQuery().ForType("User")
                       .Where(alias => alias.ContentPartRecord<FacebookUserPartRecord>(),
                       q => q.In("Id", userIds))
                       .List()
                       .ToList();

                    commentList = (from c in comments
                                   join u in users
                                   on (c.As<CommonPart>().Owner.Id) equals u.Id
                                   select new CommentItemViewModel()
                                   {
                                       Comment = c,
                                       User = u
                                   }).ToList();
                }

                var newComment = contentManager.New("Comment");
                var commentPart = newComment.As<CommentPart>();
                commentPart.ContentItemId = contentItemId;

                var httpContext = httpContextAccessor.Current();
                var sessionKey = "tempCommentPart";
                var tempCommentPart = httpContext.Session[sessionKey] as CommentPart;
                if (tempCommentPart != null)
                {
                    commentPart.CommentBody = tempCommentPart.CommentBody;
                    httpContext.Session.Remove(sessionKey);
                }

                var commentShape = contentManager.BuildEditor(newComment);
                var containerShape = ContentShape("Parts_CommentContainer",
                      () => shapeHelper.Parts_CommentContainer(
                          CommentShape: commentShape,
                          CommentList: commentList));

                return Combined(containerShape);
            }
            else
            {
                var summaryShape = ContentShape("Parts_CommentSummary",
                    () => shapeHelper.Parts_Parts_CommentSummary());
                return Combined(summaryShape);
            }
        }
    }
}