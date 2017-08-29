using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebServer.Models;
using WebServer.Models.LMS;

namespace WebServer.Repository
{
    public class AttendancesRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Attendance> Attendances()
        {
            return db.Attendances;
        }

        public Attendance GetAttendance(int? id)
        {
            return Attendances().FirstOrDefault(a => a.ID == id);
        }

        public void Add(Attendance attendance)
        {
            db.Attendances.Add(attendance);
            SaveChanges();
        }

        public void Edit(Attendance attendance)
        {
            db.Entry(attendance).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            Attendance attendance = GetAttendance(id);
            if (attendance != null)
            {
                db.Attendances.Remove(attendance);
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