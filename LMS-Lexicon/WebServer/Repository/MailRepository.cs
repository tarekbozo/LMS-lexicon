﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebServer.Models;
using WebServer.Models.LMS;

namespace WebServer.Repository
{
    public class MailRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Message> Messages(string user)
        {
            return db.Messages.Where(m => m.Sender.Email == user && m.PermantDelete2 == false || m.Sender.UserName == user && m.PermantDelete2 == false || m.Reciever.Email == user && m.PermantDelete == false || m.Reciever.UserName == user && m.PermantDelete == false);
        }

        public Message Message(int? id)
        {
            return db.Messages.SingleOrDefault(s => s.ID == id);
        }

        public void Add(Message message)
        {
            message.Sent = DateTime.Now;
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
                        if (message.Trash == false)
                        {
                            message.Trash = true;
                        }
                        else if (message.PermantDelete == false)
                        {
                            message.PermantDelete = true;
                        }
                    }
                }
                if (message.Sender != null)
                {
                    if (email == message.Sender.Email)
                    {
                        if (message.Trash2 == false)
                        {
                            message.Trash2 = true;
                        }
                        else if (message.PermantDelete2 == false)
                        {
                            message.PermantDelete2 = true;
                        }
                    }
                }

                if (message.PermantDelete==true && message.PermantDelete2==true)
                {
                    db.Messages.Remove(Message(id));
                    SaveChanges();
                }
                else
                {
                    message.Sender = null;
                    message.Reciever = null;

                    Edit(message);
                    SaveChanges();
                }
            }
        }
        public void Edit(Message message)
        {
            db.Entry(message).State = EntityState.Modified;
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