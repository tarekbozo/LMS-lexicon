using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServer.Models;
using WebServer.Models.LMS;

namespace WebServer.Repository
{
    public class MailRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Message> Messages(string email)
        {
                return db.Messages.Where(m => m.Reciever.Email==email || m.Sender.Email==email);
        }

        public Message Message(int? id)
        {
            return db.Messages.SingleOrDefault(s => s.ID == id);
        }

        /// <summary>
        /// Work to do here
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public bool Add(Subject subject)
        {
            if (db.Subjects.FirstOrDefault(s => string.Compare(s.Name, subject.Name, true) == 0) != null)
            {
                return false;
            }
            db.Subjects.Add(subject);
            SaveChanges();
            return true;
        }

        public bool Edit(Subject subject)
        {
            if (db.Subjects.FirstOrDefault(s => string.Compare(s.Name, subject.Name, true) == 0 && s.ID != subject.ID) != null)
            {
                return false;
            }
            db.Entry(subject).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public bool Delete(int? id)
        {
            Message message = Message(id);

            if (subject != null)
            {
                if (subject.Courses.Where(c => c.Documents.Count() == 0 && c.SubjectID == id).Count() == 0)
                {
                    db.Subjects.Remove(subject);
                    SaveChanges();
                    return true;
                }
            }
            return false;
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