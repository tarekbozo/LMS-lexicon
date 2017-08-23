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

namespace WebServer.Controllers
{
    public class GradesAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/GradesAPI
        public IQueryable<Grade> GetGrades()
        {
            return db.Grades;
        }

        // GET: api/GradesAPI/5
        [ResponseType(typeof(Grade))]
        public IHttpActionResult GetGrade(int id)
        {
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }

        // PUT: api/GradesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGrade(int id, Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grade.ID)
            {
                return BadRequest();
            }

            db.Entry(grade).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/GradesAPI
        [ResponseType(typeof(Grade))]
        public IHttpActionResult PostGrade(Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Grades.Add(grade);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (GradeExists(grade.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = grade.ID }, grade);
        }

        // DELETE: api/GradesAPI/5
        [ResponseType(typeof(Grade))]
        public IHttpActionResult DeleteGrade(int id)
        {
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return NotFound();
            }

            db.Grades.Remove(grade);
            db.SaveChanges();

            return Ok(grade);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GradeExists(int id)
        {
            return db.Grades.Count(e => e.ID == id) > 0;
        }
    }
}