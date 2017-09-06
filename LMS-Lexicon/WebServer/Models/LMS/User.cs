using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebServer.Models.LMS
{
    public class User : ApplicationUser
    {
        [Display(Name = "First name")]
        [CustomValidation(typeof(User), "ValidateFirstName")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [CustomValidation(typeof(User), "ValidateLastName")]
        public string LastName { get; set; }

        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }

        [ForeignKey("Internship")]
        public int? InternshipID { get; set; }
        public virtual Internship Internship { get; set; } 

        public override string ToString()
        {
            string result = string.Empty;

            if (FirstName != null)
                result = FirstName;

            if (LastName != null)
            {
                if (result.Length > 0)
                    result += " ";

                result += LastName;
            }

            return result;
        }

        public static ValidationResult ValidateFirstName(string name, ValidationContext context)
        {
            ValidationResult result = null;
            Regex rgx = new Regex(@"^[a-zA-Z][a-zA-Z-]*$");

            if (!string.IsNullOrEmpty(name) && !rgx.IsMatch(name))
            {
                result = new ValidationResult(string.Format("{0} must only contain letters and hyphens.", context.DisplayName));
            }

            return result;
        }

        public static ValidationResult ValidateLastName(string name, ValidationContext context)
        {
            ValidationResult result = null;
            Regex rgx = new Regex(@"^[a-zA-Z][a-zA-Z -]*$");

            if (!string.IsNullOrEmpty(name) && !rgx.IsMatch(name))
            {
                result = new ValidationResult(string.Format("{0} must only contain letters, spaces and hyphens.", context.DisplayName));
            }

            return result;
        }
    }
}