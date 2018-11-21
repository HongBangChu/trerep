using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            string sFileName = @"CustomerReport.xlsx";
            IWorkbook workbook = createWorkbook();
            FileStreamResult fsr = await this.doPostExport(sFileName, workbook);
            return fsr;
        }

        private void createSheetHead(ISheet excelSheet, int baseRow)
        {
            IRow row = excelSheet.CreateRow(baseRow);
            row.CreateCell(0).SetCellValue("Tháng");
            row.CreateCell(3).SetCellValue("Mã CN");

            row = excelSheet.CreateRow(baseRow + 1);
            row.CreateCell(0).SetCellValue("Năm");

            row = excelSheet.CreateRow(baseRow + 2);
            row.CreateCell(3).SetCellValue("Số lượng KH KDVTT");

            row = excelSheet.CreateRow(baseRow + 3);
            row.CreateCell(3).SetCellValue("SL KHDN MBNT");
            row.CreateCell(6).SetCellValue("SL KHDN IRS");

            row = excelSheet.CreateRow(baseRow + 4);
            row.CreateCell(3).SetCellValue("SL KHCN MBNT");
            row.CreateCell(6).SetCellValue("SL KHDN TLHH");

            row = excelSheet.CreateRow(baseRow + 5);
            row.CreateCell(3).SetCellValue("SL KHDN TDPS");
            row.CreateCell(6).SetCellValue("SL KHDN CoS");

            row = excelSheet.CreateRow(baseRow + 6);
            row.CreateCell(3).SetCellValue("SL KHDN CCS");
            row.CreateCell(6).SetCellValue("SL KHDN DCM");
        }

        private void createTableHead(ISheet excelSheet, int baseRow, int baseColumn)
        {
            IRow row = excelSheet.CreateRow(baseRow);
            row.CreateCell(baseColumn).SetCellValue("CIF");
            excelSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(baseRow, baseRow + 1, baseColumn, baseColumn));
            row.CreateCell(baseColumn + 1).SetCellValue("Tên KH");
            excelSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(baseRow, baseRow + 1, baseColumn + 1, baseColumn + 1));
            row.CreateCell(baseColumn + 2).SetCellValue("BDS");
            excelSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(baseRow, baseRow + 1, baseColumn + 2, baseColumn + 2));

            row.CreateCell(baseColumn + 3).SetCellValue("fx");
            var cra = new NPOI.SS.Util.CellRangeAddress(baseRow, baseRow, baseColumn + 3, baseColumn + 19);
            excelSheet.AddMergedRegion(cra);
        }

        private IWorkbook createWorkbook()
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("Thống kê CN");
            int sheetHeadBaseRow = 0;
            createSheetHead(excelSheet, sheetHeadBaseRow);

            // table head
            int tableHeadBaseRow = 8, tableHeadBaseColumn = 0;
            createTableHead(excelSheet, tableHeadBaseRow, tableHeadBaseColumn);

            // table body
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("report.customer_get", conn))
                {
                    NpgsqlTransaction tran = conn.BeginTransaction();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, this.getPostData().toJsonString());

                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    int i = 9;
                    while (dr.Read())
                    {
                        IRow row = excelSheet.CreateRow(i);
                        row.CreateCell(0).SetCellValue(dr["cif"].ToString());
                        row.CreateCell(1).SetCellValue(dr["name"].ToString());
                        i++;
                    }
                    dr.Close();

                    tran.Commit();
                }
            }

            return workbook;
        }
    }
}