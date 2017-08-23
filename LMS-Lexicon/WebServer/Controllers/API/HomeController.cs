using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;

namespace WebServer.Controllers
{
    public class HomeController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            String user = User.Identity.Name;

            //String id1 = User.Identity.GetUserId();
            String id2 = RequestContext.Principal.Identity.GetUserId();

            return new string[] { id2, user,User.IsInRole("Teacher").ToString() };
        }


        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
