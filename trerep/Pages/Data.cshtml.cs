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
using trerep.Code;

namespace trerep.Pages
{
    public class DataModel : BaseModel
    {
        public DataModel(IConfiguration configuration) : base(configuration) { }

        public void OnGet()
        {

        }

        public JsonResult OnPostFxTranBatchUpsert()
        {
            string batchData;
            using (var reader = new StreamReader(Request.Body))
            {
                batchData = reader.ReadToEnd();
            }
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("data.fxtran_batch_upsert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, batchData);
                    NpgsqlParameter outParam = new NpgsqlParameter("@o_result", NpgsqlTypes.NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outParam);
                    cmd.ExecuteNonQuery();
                    return new JsonResult(outParam.Value);
                }
            }
        }
        
        public JsonResult OnGetFxTran()
        {
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("data.fxtran_get", conn))
                {
                    ExpandoObject flexible = new ExpandoObject();
                    foreach (var key in Request.Query.Keys)
                    {
                        string value = Request.Query[key].ToString();
                        flexible.AddProperty(key, value);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, JsonConvert.SerializeObject(flexible));

                    NpgsqlParameter outRows = new NpgsqlParameter("@o_rows", NpgsqlTypes.NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outRows);
                    NpgsqlParameter outTotal = new NpgsqlParameter("@o_total", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outTotal);
                    cmd.ExecuteNonQuery();

                    return new JsonResult(string.Format(jsonFormat, outRows.Value, outTotal.Value));
                }
            }
        }

        public JsonResult OnDeleteFxTran(int tranid)
        {
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("data.fxtran_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, "{\"tranid\":" + tranid + "}");
                    NpgsqlParameter outParam = new NpgsqlParameter("@o_json", NpgsqlTypes.NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outParam);
                    cmd.ExecuteNonQuery();
                    return new JsonResult(outParam.Value);
                }
            }
        }
    }
}