using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServer.ViewModels
{
    public class CreateMessageVM
    {
        public string MessageContent { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
    }
}