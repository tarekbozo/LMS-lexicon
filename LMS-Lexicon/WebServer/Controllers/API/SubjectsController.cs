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
            if(subject==null || subject.Name=="" || subject.Name==null || subRepo.Subject(subject.Name).Name==subject.Name){
                return BadRequest();
            }
            try
            {
                subRepo.Add(subject);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Create(subject);
        }
    }
}