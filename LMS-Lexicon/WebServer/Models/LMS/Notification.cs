using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebServer.Models.LMS
{
    public class Notification
    {
        [Key]
        [ForeignKey("News")]
        public int ID { get; set; }

        public virtual News News { get; set; }

        public DateTime SendingDate { get; set; }
        public DateTime? ReadingDate { get; set; }

        public Grade Grade { get; set; }
    }
}