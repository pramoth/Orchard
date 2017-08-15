﻿using CodeSanook.Comment.Models;
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
            var contentItem = part.ContentItem.Id;
            //todo find list of all comments for current content item  

            var newComment = contentManager.New("Comment");
            var commentPart = newComment.As<CommentPart>();
            //commentPart.CommentContainerPartRecord  = 

            var editorShape = contentManager.BuildEditor(newComment);

            var formShape = ContentShape("Parts_CommentForm",
                () => shapeHelper.Parts_CommentForm(EditorShape: editorShape));

            var containerShape = ContentShape("Parts_CommentContainer",
                  () => shapeHelper.Parts_CommentContainer(Model: part));

            return Combined(containerShape, formShape);
        }
    }
}