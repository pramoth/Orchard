using CodeSanook.Comment.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Comment.Drivers
{
    //To have part in content type item, we need to have content part driver 
    public class CommentPartDriver : ContentPartDriver<CommentPart>
    {
        protected override string Prefix
        {
            get
            {
                return "CommentPart";
            }
        }

        protected override DriverResult Editor(CommentPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Comment_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Comment",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(CommentPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}