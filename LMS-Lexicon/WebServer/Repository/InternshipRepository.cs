using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServer.Models;
using WebServer.Models.LMS;

namespace WebServer.Repository
{
    public class InternshipRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Internship> Internships()
        {
            return db.Internships;
        }

        public Internship Internship(int? id)
        {
            return Internships().FirstOrDefault(i => i.ID == id);
        }

        public void Add(Internship internship)
        {
            internship.Date = DateTime.Now;
            db.Internships.Add(internship);
            SaveChanges();
        }

        private void SaveChanges()
        {
            db.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                db.Dispose();
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}