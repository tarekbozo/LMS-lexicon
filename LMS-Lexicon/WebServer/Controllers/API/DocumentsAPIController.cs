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
    public class DocumentsAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DocumentRepository docRepo = new DocumentRepository();


        // GET: api/Documents
        public Document document { get; set; }

        public IEnumerable<Document> Documents()
        {
            return docRepo.Documents();
        }

        // GET: api/Documents/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult GetDocument(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // PUT: api/Documents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDocument(int id, Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.ID)
            {
                return BadRequest();
            }

            db.Entry(document).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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

        // POST: api/Documents
        [ResponseType(typeof(Document))]
        public IHttpActionResult PostDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Documents.Add(document);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = document.ID }, document);
        }

        [ResponseType(typeof(Document))]
        public IHttpActionResult DeleteDocument(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            db.Documents.Remove(document);
            db.SaveChanges();

            return Ok(document);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentExists(int id)
        {
            return db.Documents.Count(e => e.ID == id) > 0;
        }
    }
}