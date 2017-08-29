namespace WebServer.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebServer.Models;
    using WebServer.Models.LMS;

    internal sealed class Configuration : DbMigrationsConfiguration<WebServer.Models.ApplicationDbContext>
    {

        public Configuration()
        {

            // AutomaticMigrationsEnabled = true;

            //AutomaticMigrationDataLossAllowed = true;

        }



        protected override void Seed(ApplicationDbContext context)
        {

            #region Roles

            if (!context.LMSRoles.Any(r => r.Name == RoleConstants.Admin))
            {

                var store = new RoleStore<Role>(context);

                var roleManager = new RoleManager<Role>(store);



                roleManager.Create(new Role(RoleConstants.Admin));

            }



            if (!context.LMSRoles.Any(r => r.Name == RoleConstants.Teacher))
            {

                var store = new RoleStore<Role>(context);

                var roleManager = new RoleManager<Role>(store);



                roleManager.Create(new Role(RoleConstants.Teacher));

            }



            if (!context.LMSRoles.Any(r => r.Name == RoleConstants.Student))
            {

                var store = new RoleStore<Role>(context);

                var roleManager = new RoleManager<Role>(store);



                roleManager.Create(new Role(RoleConstants.Student));

            }

            #endregion



            #region Subjects

            Subject subject = new Subject { Name = "French" };

            #endregion



            #region Courses

            Course course = new Course { Subject = subject };

            #endregion



            #region Users

            if (!context.LMSUsers.Any(u => u.UserName == "Admin"))
            {

                var store = new UserStore<User>(context);

                var userManager = new UserManager<User>(store);

                var newuser = new User

                {

                    UserName = "Admin",

                    Email = "admin@mail.nu",

                    BirthDate = DateTime.Now.ToString("yyyy/MM/dd"),

                    FirstName = "Admin",

                    LastName = "Histrator"

                };



                userManager.Create(newuser, "Admin-Password1");

                userManager.AddToRole(newuser.Id, RoleConstants.Admin);

            }



            Random rd = new Random();



            List<string> phoneNumbers = new List<string>();



            if (!context.LMSUsers.Any(u => u.UserName == "Liam"))
            {

                string phoneNumber = GeneratePhoneNumber(rd);



                var store = new UserStore<User>(context);

                var userManager = new UserManager<User>(store);

                var newuser = new User

                {

                    UserName = "Liam",

                    Email = "liam@mail.nu",

                    FirstName = "Liam",

                    LastName = "B",

                    BirthDate = DateTime.Now.ToString("yyyy/MM/dd"),

                    PhoneNumber = phoneNumber,

                    PhoneNumberConfirmed = true

                };



                phoneNumbers.Add(phoneNumber);



                userManager.Create(newuser, "Teacher-Password1");

                userManager.AddToRole(newuser.Id, RoleConstants.Teacher);

            }



            if (!context.LMSUsers.Any(u => u.UserName == "Student1"))
            {

                var store = new UserStore<User>(context);

                var userManager = new UserManager<User>(store);



                for (int noStudent = 1; noStudent <= 20; noStudent += 1)
                {

                    int year = rd.Next(1990, 2010);

                    int month = rd.Next(1, 13);

                    DateTime birthDate = new DateTime(year, month, rd.Next(1, DateTime.DaysInMonth(year, month) + 1));



                    string phoneNumber = string.Empty;



                    do
                    {

                        phoneNumber = GeneratePhoneNumber(rd);

                    }

                    while (phoneNumbers.Contains(phoneNumber));



                    var newuser = new User

                    {

                        UserName = "Student" + noStudent.ToString(),

                        Email = "s" + noStudent.ToString() + "@mail.nu",

                        FirstName = "Student",

                        LastName = new string((char)((int)'A' + noStudent - 1), 5),

                        BirthDate = birthDate.ToString("yyyy/MM/dd"),

                        PhoneNumber = phoneNumber,

                        PhoneNumberConfirmed = true

                    };



                    phoneNumbers.Add(phoneNumber);



                    userManager.Create(newuser, "Student-Password1");

                    userManager.AddToRole(newuser.Id, RoleConstants.Student);

                }



                for (int noTeacher = 1; noTeacher <= 10; noTeacher += 1)
                {

                    int year = rd.Next(1960, 2001);

                    int month = rd.Next(1, 13);

                    DateTime birthDate = new DateTime(year, month, rd.Next(1, DateTime.DaysInMonth(year, month) + 1));



                    string phoneNumber = string.Empty;



                    do
                    {

                        phoneNumber = GeneratePhoneNumber(rd);

                    }

                    while (phoneNumbers.Contains(phoneNumber));



                    var newuser = new User

                    {

                        UserName = "Teacher" + noTeacher.ToString(),

                        Email = "t" + noTeacher.ToString() + "@mail.nu",

                        FirstName = "Teacher",

                        LastName = new string((char)((int)'A' + noTeacher - 1), 5),

                        BirthDate = birthDate.ToString("yyyy/MM/dd"),

                        PhoneNumber = phoneNumber,

                        PhoneNumberConfirmed = true

                    };



                    phoneNumbers.Add(phoneNumber);



                    userManager.Create(newuser, "Teacher-Password1");

                    userManager.AddToRole(newuser.Id, RoleConstants.Teacher);

                }

            }



            User user = context.LMSUsers.FirstOrDefault(u => u.Email == "liam@mail.nu");

            course.Teacher = user;

            user.Courses = new List<Course> { course };



            context.Users.AddOrUpdate(u => u.Id, user);

            #endregion



            #region Classrooms

            context.Classrooms.AddOrUpdate(

                c => c.ID,

                new Classroom

                {

                    ID = 1,

                    Name = "A001",

                    Location = "Building A, ground floor, first on the right",

                    Remarks = "Lecture hall",

                    AmountStudentsMax = 354

                },

                new Classroom

                {

                    ID = 2,

                    Name = "A002",

                    Location = "Building A, ground floor, first on the left",

                    Remarks = "Chemistry classroom",

                    AmountStudentsMax = 18

                },

                new Classroom

                {

                    ID = 3,

                    Name = "A003",

                    Location = "Building A, ground floor, second on the right",

                    AmountStudentsMax = 32

                },

                new Classroom

                {

                    ID = 4,

                    Name = "A004",

                    Location = "Building A, ground floor, second on the left",

                    AmountStudentsMax = 28

                },

                new Classroom

                {

                    ID = 5,

                    Name = "A011",

                    Location = "Building A, first floor, first on the right",

                    Remarks = "Computer classroom",

                    AmountStudentsMax = 22

                },

                new Classroom

                {

                    ID = 6,

                    Name = "A012",

                    Location = "Building A, first floor, first on the left",

                    Remarks = "Computer classroom",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 7,

                    Name = "A013",

                    Location = "Building A, first floor, second on the right",

                    Remarks = "Computer classroom",

                    AmountStudentsMax = 18

                },

                new Classroom

                {

                    ID = 8,

                    Name = "A014",

                    Location = "Building A, first floor, second on the left",

                    AmountStudentsMax = 35

                },

                new Classroom

                {

                    ID = 9,

                    Name = "A021",

                    Location = "Building A, second floor, first on the right",

                    AmountStudentsMax = 48

                },

                new Classroom

                {

                    ID = 10,

                    Name = "A022",

                    Location = "Building A, second floor, first on the left",

                    Remarks = "Physics classroom",

                    AmountStudentsMax = 16

                },

                new Classroom

                {

                    ID = 11,

                    Name = "A023",

                    Location = "Building A, second floor, second on the right",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 12,

                    Name = "A024",

                    Location = "Building A, second floor, second on the left",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 13,

                    Name = "B001",

                    Location = "Building B, ground floor, first on the right",

                    Remarks = "Lecture hall",

                    AmountStudentsMax = 358

                },

                new Classroom

                {

                    ID = 14,

                    Name = "B002",

                    Location = "Building B, ground floor, first on the left",

                    Remarks = "Lecture hall",

                    AmountStudentsMax = 215

                },

                new Classroom

                {

                    ID = 15,

                    Name = "B003",

                    Location = "Building B, ground floor, second on the right",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 16,

                    Name = "B004",

                    Location = "Building B, ground floor, second on the left",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 17,

                    Name = "B011",

                    Location = "Building B, first floor, first on the right",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 18,

                    Name = "B012",

                    Location = "Building B, first floor, first on the left",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 19,

                    Name = "B013",

                    Location = "Building B, first floor, second on the right",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 20,

                    Name = "B014",

                    Location = "Building B, first floor, second on the left",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 21,

                    Name = "B021",

                    Location = "Building B, second floor, first on the right",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 22,

                    Name = "B022",

                    Location = "Building B, second floor, first on the left",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 23,

                    Name = "B023",

                    Location = "Building B, second floor, second on the right",

                    AmountStudentsMax = 12

                },

                new Classroom

                {

                    ID = 24,

                    Name = "B024",

                    Location = "Building B, second floor, second on the left",

                    AmountStudentsMax = 12

                }

                );

            #endregion

        }



        private string GeneratePhoneNumber(Random rd)
        {

            return string.Format("{0}-{1}-{2}",

                                 GenerateSequence(rd, 3),

                                 GenerateSequence(rd, 3),

                                 GenerateSequence(rd, 4));

        }



        private string GenerateSequence(Random rd, int length)
        {

            string result = string.Empty;



            while (length > 0)
            {

                result += rd.Next(0, 10).ToString();

                length -= 1;

            }



            return result;

        }
    }
}
