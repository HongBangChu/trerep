using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using trerep.Code;

namespace trerep.Pages
{
    public class ReportModel : BaseModel
    {
        public ReportModel(IConfiguration configuration, IHostingEnvironment hostingEnvironment) : base(configuration, hostingEnvironment) { }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"CustomerReport.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Thống kê CN");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Tháng");
                row.CreateCell(3).SetCellValue("Mã CN");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("Năm");

                row = excelSheet.CreateRow(2);
                row.CreateCell(3).SetCellValue("Số lượng KH KDVTT");
                
                row = excelSheet.CreateRow(3);
                row.CreateCell(3).SetCellValue("SL KHDN MBNT");

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}