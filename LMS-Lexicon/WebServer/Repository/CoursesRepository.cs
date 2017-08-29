﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebServer.Models;
using WebServer.Models.LMS;

namespace WebServer.Repository
{
    public class CoursesRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Course> Courses()
        {
            return db.Courses;
        }

        public Course Course(int? id)
        {
            return Courses().SingleOrDefault(c => c.ID == id);
        }

        public IEnumerable<Course> StudentCourses(string studentId)
        {
            return db.LMSUsers.FirstOrDefault(u => u.Id == studentId)
                              .Schedules
                              .Select(s => s.Course)
                              .OrderBy(c => c.Subject.Name)
                              .ThenBy(c => c.Teacher.ToString());
        }

        public IEnumerable<Course> TeacherCourse(string teacherId)
        {
            return db.LMSUsers.FirstOrDefault(u => u.Id == teacherId)
                              .Courses
                              .OrderBy(c => c.Subject.Name)
                              .ThenBy(c => c.Teacher.ToString());
        }

        public bool Add(Course course)
        {
            if (course.TeacherID != null)
            {
                var _courses = this.Courses().Where(c => c.TeacherID == course.TeacherID && c.SubjectID == course.SubjectID);
                if (_courses.Count() != 0)
                {
                    _courses = null;
                    return false;
                }
                db.Courses.Add(course);
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Course course)
        {
            if (this.db.Courses.Where(_cRel => _cRel.ID != course.ID && _cRel.SubjectID == course.SubjectID && _cRel.TeacherID == course.TeacherID).Count() == 0)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int? id)
        {
            Course course = Course(id);
            if (course != null)
            {
                if (course.Documents.Count() == 0 && course.Schedules.Count() == 0)
                {
                    db.Subjects.Where(s => s.Courses.Remove(course));
                    db.Courses.Remove(course);
                    SaveChanges();
                    return true;
                }
            }
            return false;
        }
        //Save changed
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