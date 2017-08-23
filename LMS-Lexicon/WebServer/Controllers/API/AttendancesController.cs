using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebServer.Models;
using WebServer.Models.LMS;
using WebServer.Repository;

namespace WebServer.Controllers.API
{
    [Authorize(Roles="Admin,Teacher")]
    public class AttendancesController : ApiController
    {
        private AttendancesRepository aRepo = new AttendancesRepository();

        // GET: api/Attendances
        public IQueryable<Attendance> GetAttendances()
        {
            return aRepo.Attendances().AsQueryable();
        }

        // GET: api/Attendances/5
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult GetAttendance(int id)
        {
            Attendance attendance = aRepo.GetAttendance(id);
            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        // PUT: api/Attendances/5
        [ResponseType(typeof(void))]
        public IHttpActionResult EditAttendance(int id, Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else if (id != attendance.ID)
            {
                return BadRequest();
            }

            try
            {
                aRepo.Edit(attendance);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (aRepo.GetAttendance(id)==null)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Attendances
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult PostAttendance([FromBody]Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            aRepo.Add(attendance);
            return CreatedAtRoute("DefaultApi", new { id = attendance.ID }, attendance);
        }
        [HttpGet]
        public IHttpActionResult GetAttendanceAsXLSFile(int id)
        {
            Attendance attendance = aRepo.GetAttendance(id);
            if (attendance != null)
            {
                //Create an instance of ExcelEngine.
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    //Set the default Excel-version  -  New Excel have backcompatible with older Excel but an old Excel can't access new Excel so therefor we set the ExcelVersion to be as low as possible.
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel97to2003;
                    //Create a workbook with a worksheet.
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1); //We only need one Excel page
                    //Access first worksheet from the workbook instance.
                    IWorksheet worksheet = workbook.Worksheets[0]; //We want to work on first page

                    //Titles
                    worksheet.Range["A1"].Text = "Teacher";
                    worksheet.Range["B1"].Text = attendance.Course.Teacher.FirstName+" "+attendance.Course.Teacher.LastName;
                    worksheet.Range["D1"].Text = "Course";
                    worksheet.Range["E1"].Text = attendance.Course.Subject.Name + "(" + attendance.Course.Teacher.FirstName + " " + attendance.Course.Teacher.LastName+")";


                    worksheet.Range["A3"].Text = "Firstname:";
                    worksheet.Range["B3"].Text = "Lastname";
                    worksheet.Range["D3"].Text = "Attendance";

                    int index = 4;
                    foreach (User student in attendance.Students)
                    {
                        //Values
                        worksheet.Range["A"+index].Text = student.FirstName;
                        worksheet.Range["B"+index].Text = student.LastName;
                        worksheet.Range["D"+index].Text = (attendance.Students.Where(s=>s.Id==student.Id).Count()>0).ToString();
                        
                        index++;
                    }
                    //Save the workbook to user-header in xlx format. - The file is downloading
                    HttpResponse response = HttpContext.Current.ApplicationInstance.Response;
                    response.AppendHeader("Access-Control-Allow-Origin", "*");
                    workbook.SaveAs("Attendance.xls", response, ExcelDownloadType.Open);
                    return Ok();
                }
            }
            return NotFound();
        }
        // DELETE: api/Attendances/5
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult DeleteAttendance(int id)
        {
            Attendance attendance = aRepo.GetAttendance(id);
            if (attendance == null)
            {
                return NotFound();
            }

            aRepo.Delete(id);
            return Ok(attendance);
        }
    }
}