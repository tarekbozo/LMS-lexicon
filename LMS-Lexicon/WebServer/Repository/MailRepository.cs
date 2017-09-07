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

        public void Add(Message message)
        {
            db.Messages.Add(message);
            SaveChanges();
        }

        public void Delete(int? id,string email)
        {
            Message message = Message(id);

            if (message != null)
            {
                if (message.Reciever != null)
                {
                    if(email==message.Reciever.Email){
                        message.Reciever = null;
                        message.RecieverId = null;
                    }
                }
                if (message.Sender != null)
                {
                    if (email == message.Sender.Email)
                    {
                        message.Sender = null;
                        message.SenderId = null;
                    }
                }

                if (message.Sender == null && message.Reciever == null)
                {
                    db.Messages.Remove(Message(id));
                    SaveChanges();
                }
            }
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