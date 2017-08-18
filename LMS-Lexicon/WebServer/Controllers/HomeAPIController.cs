using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebServer.Controllers
{
    public class HomeAPIController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
        
        [HttpGet]
        public void GetXLSFile()
        {
            //Create an instance of ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2016.
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel97to2003;

                //Create a workbook with a worksheet.
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                //Access first worksheet from the workbook instance.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Insert Values into cell “A1,B1,A2,B2” and see what happens.
                worksheet.Range["A1"].Text = "FirstName:";
                worksheet.Range["B1"].Text = "LastName";
                worksheet.Range["C1"].Text = "Attendance";
                
                worksheet.Range["A2"].Text = "API Call3";
                worksheet.Range["B2"].Text = "API Call4";
                worksheet.Range["C2"].Text = "API Call4";
                worksheet.Range["A3"].Text = "API Call3";
                worksheet.Range["B3"].Text = "API Call4";
                worksheet.Range["C3"].Text = "API Call4";
                //Save the workbook to disk in xlx format.
                HttpResponse response = HttpContext.Current.ApplicationInstance.Response;
                response.AppendHeader("Access-Control-Allow-Origin", "*");
                workbook.SaveAs("Attendance.xls",response, ExcelDownloadType.Open);
            }
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
