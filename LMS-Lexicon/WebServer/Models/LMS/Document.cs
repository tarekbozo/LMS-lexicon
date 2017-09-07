using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebServer.Models.LMS
{
    public class Document
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Required]
        public byte[] DocumentContent { get; set; }

        public string ContentType { get; set; }

        [Required]
        [Display(Name = "Uploaded")]
        public DateTime UploadingDate { get; set; }

        [Required]
        [ForeignKey("Uploader")]
        public string UploaderID { get; set; }
        [Display(Name = "By")]
        public virtual User Uploader { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        [ForeignKey("VisibleFor")]
        public string RoleID { get; set; }
        public virtual Role VisibleFor { get; set; }
    }
    public class Assignment 
    {
        [Key]
        public int ID { get; set; }
        public virtual ICollection<Document> files { get; set; }

        [ForeignKey("AssignmentFile")]
        public int AssignmentFileID { get; set; }
        public virtual Document AssignmentFile { get; set; }
        public string AssignmentTheme { get; set; }
        public DateTime Deadline { get; set; }
        public Status Status { get; set; }
        public string StudentComment { get; set; }
        public string Link { get; set; }
        public string TeacherComment { get; set; }

    }
    public enum Status
    {
        Completed,
        Completion,
        Examined,
        NotFinnished
    }
}