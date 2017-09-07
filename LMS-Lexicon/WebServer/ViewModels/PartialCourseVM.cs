using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServer.Models.LMS;

namespace WebServer.ViewModels
{
    public class PartialCourseVM
    {
        public int? ID { get; set; }
        public Subject Subject { get; set; }
        public PartialUserVM Teacher { get; set; }
        public bool IsDeletable { get; set; }
    }
}