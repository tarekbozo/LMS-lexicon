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
        [Display(Name = "Ending date")]
        public string EndingDate { get; set; }
        public virtual ICollection<User> Students { get; set; }

        public DateTime Date { get; set; }
    }
}