using CodeSanook.Comment.Models;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Comment.Drivers
{
    public class CommentContainerPartDriver : ContentPartDriver<CommentContainerPart>
    {
        protected override DriverResult Display(CommentContainerPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_CommentContainer",
                () => shapeHelper.Parts_CommentContainer(
                    Model: part));
        }
    }
}