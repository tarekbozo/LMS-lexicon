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
    public class ClassroomsAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ClassroomsAPI
        public IQueryable<Classroom> GetClassrooms()
        {
            return db.Classrooms;
        }

        // GET: api/ClassroomsAPI/5
        [ResponseType(typeof(Classroom))]
        public IHttpActionResult GetClassroom(int id)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return NotFound();
            }

            return Ok(classroom);
        }

        // PUT: api/ClassroomsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClassroom(int id, Classroom classroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != classroom.ID)
            {
                return BadRequest();
            }

            db.Entry(classroom).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(id))
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

        // POST: api/ClassroomsAPI
        [ResponseType(typeof(Classroom))]
        public IHttpActionResult PostClassroom(Classroom classroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Classrooms.Add(classroom);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = classroom.ID }, classroom);
        }

        // DELETE: api/ClassroomsAPI/5
        [ResponseType(typeof(Classroom))]
        public IHttpActionResult DeleteClassroom(int id)
        {
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return NotFound();
            }

            db.Classrooms.Remove(classroom);
            db.SaveChanges();

            return Ok(classroom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassroomExists(int id)
        {
            return db.Classrooms.Count(e => e.ID == id) > 0;
        }
    }
}