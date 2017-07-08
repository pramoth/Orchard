using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace FacebookConnect.Models
{
    public class FacebookSettingsPart : ContentPart<FacebookSettingsPartRecord>
    {
        public string AppId
        {
            get { return Record.AppId; }
            set { Record.AppId = value; }
        }

        public string AppSecret
        {
            get { return Record.AppSecret; }
            set { Record.AppSecret = value; }
        }
    }
}