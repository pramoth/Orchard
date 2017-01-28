using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using FacebookConnect.Models;

namespace FacebookConnect.Handlers
{
    public class FacebookUserPartHandler : ContentHandler
    {
        public FacebookUserPartHandler(IRepository<FacebookUserPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<FacebookUserPart>("User"));
            Filters.Add(StorageFilter.For(repository));
        }

    }
}