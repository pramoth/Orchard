using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.FacebookPage.Models
{
    public class FacebookPagePartRecord : ContentPartRecord
    {
        public virtual string Href { get; set; }
    }
}