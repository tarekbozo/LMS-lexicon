using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebServer.Models.LMS
{
 public class Attendance
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [ForeignKey("Course")]
        public int CourseID {get;set;}
        public virtual Course Course{get;set;}
        [Required]
        public virtual ICollection<User> Students {get; set;}
    }
}