﻿using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using System.IO;

namespace WebServer.Controllers
{
    //This controller will be the news controller later - it's for the home page
    public class HomeController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1","value2" };
        }


        // GET api/values/5
        [HttpPost]
        public string SendFile(byte[] file)
        {
            return "a file";
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
