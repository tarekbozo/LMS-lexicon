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

namespace WebServer.Controllers.API
{
    public class SubjectsController : ApiController
    {
        SubjectsRepository subRepo = new SubjectsRepository();
        [HttpGet]
        public IHttpActionResult Get(int ID)
        {
            Subject s = subRepo.Subject(ID) as Subject;
            if (s == null)
            {
                return BadRequest();
            }
            List<Course> courses = new List<Course>();
            foreach (Course c in s.Courses.OrderBy(c => c.Teacher.FirstName).ThenBy(c => c.Teacher.LastName))
            {
                Course tempC = new Course {
                    Teacher = new User { FirstName=c.Teacher.FirstName, LastName=c.Teacher.LastName}
                };
                courses.Add(tempC);
                tempC = null;
            }
            s = new Subject { ID = s.ID, Name = s.Name, Courses = courses };
            return Ok(s);
        }
        [HttpGet]
        public List<Subject> Get()
        {
            List<Subject> _subjects = new List<Subject>();
            foreach (Subject s in new SubjectsRepository().Subjects())
            {
                Subject tempS = new Subject();
                tempS.ID = s.ID;
                tempS.Name = s.Name;

                if (s.Courses.Count() > 0)
                {
                    List<Course> tList = new List<Course>();

                    foreach (Course c in s.Courses)
                    {
                        Course cTemp = new Course();
                        cTemp.ID = c.ID;
                        tList.Add(cTemp);
                        cTemp = null;
                    }
                    tempS.Courses = tList;
                    tList = null;
                }
                _subjects.Add(tempS);
                tempS = null;
            }
            return _subjects;
        }
        [HttpPost]
        public IHttpActionResult Create(Subject subject)
        {
            bool created = subRepo.Add(subject);
            if (created == false)
            {
                return BadRequest();
            }
            return Ok(subject);
        }
        [HttpDelete]
        public IHttpActionResult Delete(Subject id)
        {
            bool deleted = subRepo.Delete(id.ID);
            if(!deleted){
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult Edit(Subject subject)
        {
            Subject s = subRepo.Subject(subject.ID) as Subject;
            s.Name = subject.Name;
            bool edited = subRepo.Edit(s);
            if(!edited){
                return BadRequest();
            }
            return Ok(subject);
        }
    }
}