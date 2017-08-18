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

        [Display(Name = "Week day")]
        public WeekDays WeekDay { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int UserID { get; set; }
        public virtual User Student { get; set; }
    }
}