using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Orchard.UI.Admin;

namespace CodeSanook.AmazonS3.Controllers
{
    [Admin]
    public class AdminController : Controller
    {
        public ActionResult Settings()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Settings(FormCollection form)
        {
            throw new NotImplementedException();
        }
    }

}
