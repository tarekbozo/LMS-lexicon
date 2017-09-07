using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebServer.Models;
using WebServer.Models.LMS;
using WebServer.Repository;
using WebServer.ViewModels;

namespace WebServer.Controllers.API
{
    [Authorize(Roles="Admin")]
    public class CoursesController : ApiController
    {
        /// <summary>
        /// Return all courses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<PartialCourseVM> Get()
        {
            //On visual studio 2017, access violation might occur than we are sending the whole user through the api.
            //return new CoursesRepository().Courses() works fine on vs 13 with the api
            //Guess that microsoft want us to avoid sending security information.

            return new CoursesRepository().Courses()
                                          .Select(c => new PartialCourseVM
                                          {
                                              ID = c.ID,
                                              IsDeletable = c.Documents.Count() + c.Schedules.Count() == 0,
                                              //Binding data
                                              Subject = new Subject
                                              {
                                                  ID = c.SubjectID,
                                                  Name = c.Subject.Name
                                              },
                                              Teacher = new PartialUserVM
                                              {
                                                  //Set data that are needed
                                                  Id = c.TeacherID,
                                                  FirstName = c.Teacher.FirstName,
                                                  LastName = c.Teacher.LastName,
                                                  Email = c.Teacher.Email,
                                                  HasSchedules = c.Schedules.Count > 0
                                              }
                                          })
                                          .ToList();
        }
        [HttpGet]
        public IHttpActionResult Get(int ID)
        {
            if (ID == null)
            {
                return BadRequest();
            }
            Course course = new CoursesRepository().Course(ID) as Course;
            if (course == null)
            {
                return BadRequest("Couldn't find Course with id of "+ID);
            }
            return Ok(new Course { ID = course.ID, Subject = new Subject { ID=course.Subject.ID, Name = course.Subject.Name }, Teacher = new User { Id=course.Teacher.Id, FirstName = course.Teacher.FirstName, LastName = course.Teacher.LastName, Email = course.Teacher.Email } });
        }
        [HttpDelete]
        public IHttpActionResult Delete(Course id)
        {
            bool deleted = new CoursesRepository().Delete(id.ID);
            if (!deleted)
            {
                return BadRequest("Course couldn't be Deleted");
            }
            return Ok();
        }
        [HttpPatch]
        public IHttpActionResult Edit(EditCourseVM course)
        {
            Course c = new CoursesRepository().Course(course.ID) as Course;
            if (c == null)
            {
                return BadRequest("Course id didn't match");
            }
            c = new Course { ID = course.ID, SubjectID = course.SubjectID, TeacherID = course.TeacherID };
            bool edited = new CoursesRepository().Edit(c);
            if(!edited){
                return BadRequest("Course couldn't be edited");
            }
            return Ok(c);
        }
        [HttpPost]
        [OverrideAuthorization]
        [AllowAnonymous]
        public IHttpActionResult Create(CreateCourseVM course)
        {
            if (course.TeacherID == null || course.SubjectID == null || course.TeacherID == "")
            {
                return BadRequest();
            }

            Course c = new Course { SubjectID = course.SubjectID, TeacherID = course.TeacherID };
            bool created = new CoursesRepository().Add(c);
            if (!created)
            {
                return BadRequest("Dublicated data detected!");
            }

            return Ok(c);
        }
    }
}