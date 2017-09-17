using System.Web.Mvc;
using Orchard.Localization;
using Orchard;
using System.Web.Http;
using System.Collections.Generic;

namespace CodeSanook.Api.Controllers
{
    public class UserApiController : ApiController
    {

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public UserApiController(IOrchardServices services)
        {
            Services = services;
            T = NullLocalizer.Instance;
        }


        public IHttpActionResult Get()
        {
            var itemsList = new List<string>{
                "Item 1",
                "Item 2",
                "Item 3"
            };

            return Ok(itemsList);
        }


    }
}
