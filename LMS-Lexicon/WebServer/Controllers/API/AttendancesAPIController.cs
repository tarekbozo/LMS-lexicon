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
    public class AttendancesAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AttendancesAPI
        public IQueryable<Attendance> GetAttendances()
        {
            return db.Attendances;
        }

        // GET: api/AttendancesAPI/5
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult GetAttendance(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        // PUT: api/AttendancesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAttendance(int id, Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attendance.ID)
            {
                return BadRequest();
            }

            db.Entry(attendance).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(id))
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

        // POST: api/AttendancesAPI
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult PostAttendance(Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attendances.Add(attendance);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = attendance.ID }, attendance);
        }

        // DELETE: api/AttendancesAPI/5
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult DeleteAttendance(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return NotFound();
            }

            db.Attendances.Remove(attendance);
            db.SaveChanges();

            return Ok(attendance);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttendanceExists(int id)
        {
            return db.Attendances.Count(e => e.ID == id) > 0;
        }
    }
}