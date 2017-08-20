using CodeSanook.Comment.Models;
using CodeSanook.FacebookConnect.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Security;
using Orchard.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Comment.Drivers
{
    public class CommentContainerPartDriver : ContentPartDriver<CommentContainerPart>
    {
        private IContentManager contentManager;
        private readonly IAuthenticationService auth;

        protected override string Prefix
        {
            get { return "CommentContainerPart"; }
        }

        public CommentContainerPartDriver(IContentManager contentManager, IAuthenticationService auth)
        {
            this.contentManager = contentManager;
            this.auth = auth;
        }

        protected override DriverResult Display(CommentContainerPart part, string displayType, dynamic shapeHelper)
        {
            var contentItemId = part.ContentItem.Id;

            if (displayType == "Detail")
            {

                var user = auth.GetAuthenticatedUser();
                var commentList = contentManager.HqlQuery().ForType("Comment")
                     .Where(alias => alias.ContentPartRecord<CommentPartRecord>(), s => s.Eq("ContentItemId", contentItemId))
                     .List()
                     .ToList();

                var userIds = commentList.Select(c => c.As<CommonPart>().Owner.Id).ToArray();
                var userList = contentManager.HqlQuery().ForType("User")
                    .Where(alias => alias.ContentPartRecord<FacebookUserPartRecord>(),
                    q => q.In("Id", userIds))
                    .List()
                    .ToList();

                //to do create view model and hide facebook part
                var viewModel = (from c in commentList
                                 join u in userList 
                                 on (c.As<CommonPart>().Owner.Id) equals u.Id
                                 select u).ToList();


                var newComment = contentManager.New("Comment");
                var commentPart = newComment.As<CommentPart>();
                commentPart.ContentItemId = contentItemId;

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