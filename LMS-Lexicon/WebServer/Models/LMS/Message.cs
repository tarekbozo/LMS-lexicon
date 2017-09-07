using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebServer.Models.LMS
{
    public class Message
    {
        [Key]
        public int ID { get; set; }
        public string MailContent { get; set; }
        
        public DateTime Sent { get; set; }
        public DateTime? Readed { get; set; }  

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        [ForeignKey("Reciever")]
        public string RecieverId { get; set; }
        public virtual User Reciever { get; set; }
        //User 1 and User 2 - Settings
        public bool Spam { get; set; }
        public bool Spam2 { get; set; }

        public bool Trash { get; set; }
        public bool Trash2 { get; set; }

        public bool Important { get; set; }
        public bool Important2 { get; set; }

        public bool Favorite { get; set; }
        public bool Favorite2 { get; set; }

    }
}