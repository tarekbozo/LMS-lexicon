﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebServer.Models;
using WebServer.Models.LMS;

namespace WebServer.Repository
{
    public class GradeRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Grade> Grades()
        {
            return db.Grades;
        }

        public Grade Grade(int? id)
        {
            return Grades().FirstOrDefault(g => g.ID == id);
        }

        public void Add(Grade grade)
        {
            grade.Date = DateTime.Now;
            db.Grades.Add(grade);
            SaveChanges();
            new NotificationRepository().Create(grade.ID);
        }

        public void Edit(Grade grade)
        {
            db.Entry(grade).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            Grade grade = Grade(id);

            if (grade != null)
            {
                db.Grades.Remove(grade);
                SaveChanges();
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