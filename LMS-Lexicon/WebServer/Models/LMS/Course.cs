﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebServer.Models.LMS
{
    public class Course
    {
         [Key]
        public int ID { get; set; }

        [ForeignKey("Subject")]
        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherID { get; set; }
        public virtual User Teacher { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
    
}