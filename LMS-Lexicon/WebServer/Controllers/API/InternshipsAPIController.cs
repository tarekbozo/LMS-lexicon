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

namespace WebServer.Controllers
{
    public class InternshipsAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        IntershipRepository iRepo = new IntershipRepository();

        // GET: api/InternshipsAPI
        public IQueryable<Internship> GetInternships()
        {
            return db.Internships;
        }

        // GET: api/InternshipsAPI/5
        [ResponseType(typeof(Internship))]
        public IHttpActionResult GetInternship(int id)
        {
            Internship internship = db.Internships.Find(id);
            if (internship == null)
            {
                return NotFound();
            }

            return Ok(internship);
        }

        // PUT: api/InternshipsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInternship(int id, Internship internship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != internship.ID)
            {
                return BadRequest();
            }

            db.Entry(internship).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipExists(id))
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

        // POST: api/InternshipsAPI
        [ResponseType(typeof(Internship))]
        public IHttpActionResult PostInternship(Internship internship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Internships.Add(internship);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = internship.ID }, internship);
        }

        // DELETE: api/InternshipsAPI/5
        [ResponseType(typeof(Internship))]
        public IHttpActionResult DeleteInternship(int id)
        {
            Internship internship = db.Internships.Find(id);
            if (internship == null)
            {
                return NotFound();
            }

            db.Internships.Remove(internship);
            db.SaveChanges();

            return Ok(internship);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InternshipExists(int id)
        {
            return db.Internships.Count(e => e.ID == id) > 0;
        }
    }
}