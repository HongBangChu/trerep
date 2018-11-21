using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace trerep.Code
{
    public class BaseModel : PageModel
    {
        protected const string jsonFormat = "{{\"rows\":{0},\"total\":{1}}}";
        protected readonly IConfiguration _configuration;
        protected readonly string _connStr;
        protected readonly IHostingEnvironment _hostingEnvironment;
        public BaseModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("PostgresConnection");
        }

        public BaseModel(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("PostgresConnection");
            _hostingEnvironment = hostingEnvironment;
        }

        protected string getPostData()
        {
            string postData;
            using (var reader = new StreamReader(Request.Body))
            {
                postData = reader.ReadToEnd();
            }
            return postData;
        }

        protected async Task<FileStreamResult> doPostExport(string sFileName, IWorkbook workbook)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            FileStreamResult fsr = File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            return fsr;
        }
    }
}
