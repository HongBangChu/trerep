using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using trerep;

namespace trerep.Pages
{
    public class DataModel : PageModel
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private string _connStr;
        public DataModel(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("PostgresConnection");
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnGet()
        {

        }

        public ActionResult OnPostImport()
        {
            String result = "";
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    using (var conn = new NpgsqlConnection(_connStr))
                    {
                        conn.Open();
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            // importing row ...
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            ExpandoObject para = new ExpandoObject();
                            para.AddProperty("month", Request.Query["month"]);
                            para.AddProperty("year", Request.Query["year"]);
                            List<string> cells = new List<string>();
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    cells.Add(row.GetCell(j).ToString());
                                }
                            }
                            para.AddProperty("row", cells);
                            using (var cmd = new NpgsqlCommand("data.fxtran_import_row", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, JsonConvert.SerializeObject(para));
                                NpgsqlParameter outParam = new NpgsqlParameter("@o_result", NpgsqlTypes.NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                                cmd.Parameters.Add(outParam);
                                cmd.ExecuteNonQuery();
                                result += outParam.Value;
                            }
                        }
                    }
                }
            }
            return new JsonResult(result);
        }
    }
}