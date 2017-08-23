﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebServer.Models;
using WebServer.Models.LMS;

namespace WebServer.Repository
{
    public class SubjectsRepository : IDisposable
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Subject> Subjects()
        {
            return db.Subjects;
        }

        public Subject Subject(int? id)
        {
            return Subjects().SingleOrDefault(s => s.ID == id);
        }

        public Subject Subject(string name)
        {
            return Subjects().SingleOrDefault(s => s.Name == name);
        }

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
            Subject subject = Subject(id);

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

        public List<Subject> AvaibleSubjects(string user)
        {
            List<Subject> Subject_result = new List<Subject>();
            foreach (Subject s in Subjects().ToList())
            {
                if (s.Courses.Count == 0 || s.Courses.Where(c => c.Teacher.UserName == user || c.TeacherID == user).Count() == 0)
                {
                    Subject_result.Add(s);
                }
            }
            return Subject_result;
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