﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using trerep.Code;

namespace trerep.Pages.Reports
{
    public class CustByBDSModel : BaseModel
    {
        public CustByBDSModel(IConfiguration configuration, IHostingEnvironment hostingEnvironment) : base(configuration, hostingEnvironment) { }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostExport()
        {
            string para;
            using (var reader = new StreamReader(Request.Body))
            {
                para = reader.ReadToEnd();
            }
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
                row.CreateCell(6).SetCellValue("SL KHDN IRS");

                row = excelSheet.CreateRow(4);
                row.CreateCell(3).SetCellValue("SL KHCN MBNT");
                row.CreateCell(6).SetCellValue("SL KHDN TLHH");

                row = excelSheet.CreateRow(5);
                row.CreateCell(3).SetCellValue("SL KHDN TDPS");
                row.CreateCell(6).SetCellValue("SL KHDN CoS");

                row = excelSheet.CreateRow(6);
                row.CreateCell(3).SetCellValue("SL KHDN CCS");
                row.CreateCell(6).SetCellValue("SL KHDN DCM");

                // table head
                row = excelSheet.CreateRow(8);
                row.CreateCell(0).SetCellValue("CIF");
                excelSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(8, 9, 0, 0));
                row.CreateCell(1).SetCellValue("Tên KH");
                excelSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(8, 9, 1, 1));
                row.CreateCell(2).SetCellValue("BDS");
                excelSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(8, 9, 2, 2));

                row.CreateCell(3).SetCellValue("fx");
                var cra = new NPOI.SS.Util.CellRangeAddress(8, 8, 3, 19);
                excelSheet.AddMergedRegion(cra);

                // table body
                using (var conn = new NpgsqlConnection(_connStr))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("para.cust_batch_upsert", conn))
                    {
                        NpgsqlTransaction tran = conn.BeginTransaction();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, para);
                        NpgsqlParameter outParam = new NpgsqlParameter("@o_rows", NpgsqlTypes.NpgsqlDbType.Refcursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(outParam);
                        cmd.ExecuteNonQuery();

                        NpgsqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            // do what you want with data, convert this to json or...
                            excelSheet.CreateRow(9).CreateCell(0).SetCellValue(dr["cif"].ToString());
                        }
                        dr.Close();

                        tran.Commit();
                    }
                }

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