using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServer.ViewModels
{
    public class EditCourseVM
    {
        public int ID { get; set; }
        public int SubjectID { get; set; }
        public string TeacherID { get; set; }
    }
}