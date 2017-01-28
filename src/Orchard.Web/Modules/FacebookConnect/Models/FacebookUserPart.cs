using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace FacebookConnect.Models
{
    public class FacebookUserPart : ContentPart<FacebookUserPartRecord>
    {
        public string UserId
        {
            get { return Record.UserId; }
            set { Record.UserId = value; }
        }
    }

    public class FacebookUserPartRecord : ContentPartRecord {
        public virtual string UserId { get; set; }
    }
}