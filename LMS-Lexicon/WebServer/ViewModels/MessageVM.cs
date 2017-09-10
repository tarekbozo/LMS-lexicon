using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServer.Models.LMS;

namespace WebServer.ViewModels
{
    public class MessageVM
    {
        public int ID { get; set; }
        //mail
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime Sent { get; set; }
        public DateTime? Readed { get; set; }
        //User details
        public User Sender { get; set; }
        public User Reciever { get; set; }
        public bool Owned { get; set; }
        //Settings
        public bool Favorite { get; set; }
        public bool Trash { get; set; }
        public bool Important { get; set; }
        public bool PermantDelete { get; set; }
    }
}