using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebServer.Models;
using WebServer.Models.LMS;
using Microsoft.AspNet.Identity;
using WebServer.ViewModels;

namespace WebServer.Controllers.API
{
    public class MessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Messages
        public IQueryable<Message> GetMessages()
        {
            return db.Messages.Where(m=>m.Sender.UserName==User.Identity.Name || m.Reciever.UserName==User.Identity.Name);
        }
    }
}