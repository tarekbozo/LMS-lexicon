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

        // UploadDocument Methods

         //Get : Specific Course
        //public IHttpActionResult UploadDocumentForSpecificCourse()
        //{
        //    ViewBag.Courses = GetCourses(false);
        //    return View();
        //}

        //// POST : Specific Course
        //[HttpPost]
        //public IHttpActionResult UploadDocumentForSpecificCourse(UploadDocumentVM viewModel)
        //{
        //    if (ModelState.IsValid && viewModel.File != null)
        //    {
        //        string result = CreateDocument(viewModel, RoleConstants.Teacher);

        //        if (result.Length == 0)
        //            return RedirectToAction("MyDocuments");
        //        else
        //        {
        //            ViewBag.ErrorMessage = result;
        //            ViewBag.Courses = GetCourses(false);

        //            return View(viewModel);
        //        }
        //    }

        //    ViewBag.Courses = GetCourses(false);
        //    return View(viewModel);
        //}

        // POST: api/Documents
        //[ResponseType(typeof(Document))]
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
        // POST: Documents/Delete
        [ResponseType(typeof(Document))]
        public IHttpActionResult DeleteDocument(int id)
        {
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            docRepo.Delete(id);

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