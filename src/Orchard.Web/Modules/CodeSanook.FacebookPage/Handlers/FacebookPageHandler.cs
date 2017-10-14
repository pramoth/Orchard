using CodeSanook.FacebookPage.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CodeSanook.Map.Handlers
{
    public class FacebookPageHandler : ContentHandler
    {

        /// <summary>
        ///override in constructor 
        /// </summary>
        /// <param name="repository"></param>
        public FacebookPageHandler(IRepository<FacebookPagePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }

}