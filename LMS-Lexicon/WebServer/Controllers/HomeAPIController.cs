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

                //Insert sample text into cell “A1”.
                worksheet.Range["A1"].Text = "Hello World";
                worksheet.Range["C1"].Text = "API Call1";
                worksheet.Range["C2"].Text = "API Call2";
                worksheet.Range["C3"].Text = "API Call3";
                worksheet.Range["C4"].Text = "API Call4";
                //Save the workbook to disk in xlx format.
                HttpResponse debug = HttpContext.Current.ApplicationInstance.Response;
                debug.AppendHeader("Access-Control-Allow-Origin", "*");

                workbook.SaveAs("Attendance.xls", debug, ExcelDownloadType.Open);
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
