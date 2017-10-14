using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeSanook.FacebookPage.Models
{
    public class FacebookPagePart : ContentPart<FacebookPagePartRecord>
    {
        [Required]
        public string Href
        {
            get { return Retrieve(r => r.Href); }
            set { Store(r => r.Href, value); }
        }

    }
}