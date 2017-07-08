using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace FacebookConnect.Models
{
    public class FacebookSettingsPartRecord : ContentPartRecord
    {
        public virtual string AppId { get; set; }
        public virtual string AppSecret { get; set; }
    }
}