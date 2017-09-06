using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

        //Upload Document
        [HttpPost]
        [Route("{controller}/upload/{fileName}/{filePath}")]
        public async Task<HttpResponseMessage> UploadDocument(string documentName, string documentPath)
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
            }
            byte[] data = Convert.FromBase64String(documentPath);
            documentPath = Encoding.UTF8.GetString(data);
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var dataStream = await file.ReadAsStreamAsync();
                    // use the data stream to persist the data to the server (file system etc)
                    using (var fileStream = File.Create(documentPath + documentName))
                    {
                        dataStream.Seek(0, SeekOrigin.Begin);
                        dataStream.CopyTo(fileStream);
                    }
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent("Successful upload", Encoding.UTF8, "text/plain");
                    response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
                    return response;
                }
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //Download Document
        [HttpGet]
        [Route("FileServer")]
        public async Task<HttpResponseMessage> GetDocument(int DocumentID)
        {
            using (var docRepo = new DocumentRepository())
            {
                var Document = await docRepo.GetSpecificDocument(DocumentID);
                if (Document == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var stream = File.Open(Document.PathLocator, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Document.FileType);
                return response;
            }
        }

    }
}