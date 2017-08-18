using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebServer.Models.LMS
{
    public class Internship
    {
        [Key]
        public int ID { get; set; }
        public string CompanyName { get; set; }

        [Display(Name = "Starting date")]
        public string StartingDate { get; set; }

        [Required]
        [Display(Name = "Ending date")]
        public string EndingDate { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int UserID { get; set; }
        public virtual User Student { get; set; }
    }
}