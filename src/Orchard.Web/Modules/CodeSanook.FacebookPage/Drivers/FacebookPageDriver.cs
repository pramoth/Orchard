using CodeSanook.FacebookPage.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.FacebookPage.Drivers
{
    public class FacebookPageDriver : ContentPartDriver<FacebookPagePart>
    {
        protected override DriverResult Display(FacebookPagePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_FacebookPage", 
                () => shapeHelper.Parts_FacebookPage(Href: part.Href));
            //This will render View/Parts/FacebookPage.cshtml with model which has 
            //Href property
        }

        //GET
        protected override DriverResult Editor( FacebookPagePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_FacebookPage_Edit",//just name
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/FacebookPage",
                    Model: part,
                    Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(FacebookPagePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}