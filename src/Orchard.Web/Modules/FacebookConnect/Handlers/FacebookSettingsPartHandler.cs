using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using FacebookConnect.Models;

namespace FacebookConnect.Handlers
{
    public class FacebookSettingsPartHandler : ContentHandler
    {
        public FacebookSettingsPartHandler(IRepository<FacebookSettingsPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<FacebookSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }

    }
}