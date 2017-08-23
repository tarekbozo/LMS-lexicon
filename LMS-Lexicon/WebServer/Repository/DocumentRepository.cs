//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using WebServer.Models;
//using WebServer.Models.LMS;

//namespace WebServer.Repository
//{
//    public class DocumentRepository
//    {

//        public DocumentRepository docRepo = new DocumentRepository();

//        public Document document { get; set; }

//        public IEnumerable<Document> Documents()
//        {
//            return docRepo.Document;
//        }

//        //Add
//        public void Add(Document document)
//        {
//            docRepo.Documents.Add(document);
//            docRepo.SaveChanges();
//        }

//        public Document Document(int? id)
//        {
//            return Documents().FirstOrDefault(d => d.ID == id);
//        }

//        //Delete
//        public void Delete(int id)
//        {
//            Document document = Document(id);
//            if (document != null)
//            {
//                docRepo.Documents.Remove(document);
//                docRepo.SaveChanges();
//            }
//        }
//        private void SaveChanges()
//        {
//            docRepo.SaveChanges();
//        }
//        #region IDisposable
//        private bool disposedValue = false;

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                docRepo.Dispose();
//                disposedValue = true;
//            }
//        }
//        public void Dispose()
//        {
//            Dispose(true);
//        }
//        #endregion
//    }
//}
