using CodeSanook.Comment.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Comment.Drivers
{
    public class CommentContainerPartDriver : ContentPartDriver<CommentContainerPart>
    {
        private IContentManager contentManager;

        protected override string Prefix
        {
            get { return "CommentContainerPart"; }
        }

        public CommentContainerPartDriver(IContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override DriverResult Display(CommentContainerPart part, string displayType, dynamic shapeHelper)
        {
            if (displayType == "Detail")
            {
                var contentItemId = part.ContentItem.Id;
                var commentList = contentManager.Query<CommentPart, CommentPartRecord>()
                     .Where(c => c.ContentItemId == contentItemId)
                     .List()
                     .Select(c => c.ContentItem)
                     .ToList();

                var newComment = contentManager.New("Comment");
                var commentPart = newComment.As<CommentPart>();
                commentPart.ContentItemId = contentItemId;

                var commentShape = contentManager.BuildEditor(newComment);
                var containerShape = ContentShape("Parts_CommentContainer",
                      () => shapeHelper.Parts_CommentContainer(CommentShape: commentShape, CommentList: commentList));

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