using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json;

namespace trerep.Pages
{
    public class ParaModel : PageModel
    {
        private const string jsonFormat = "{{\"rows\":{0},\"total\":{1}}}";
        private readonly IConfiguration _configuration;
        private string _connStr;
        public ParaModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("PostgresConnection");
        }
        public void OnGet()
        {
            ViewData["connStr"] = _configuration.GetConnectionString("PostgresConnection");
            //using(var conn = new NpgsqlConnection(_connStr))
            //{
            //    conn.Open();
            //    // Insert some data
            //    using (var cmd = new NpgsqlCommand())
            //    {
            //        cmd.Connection = conn;
            //        cmd.CommandText = "INSERT INTO cust (cif) VALUES (@p)";
            //        cmd.Parameters.AddWithValue("p", 1);
            //        //cmd.Parameters.Add("p_jsonstr", NpgsqlTypes.NpgsqlDbType.Json);
            //        //cmd.Parameters.AddWithValue("p_json", NpgsqlTypes.NpgsqlDbType.Json, "");
            //        cmd.ExecuteNonQuery();
            //    }
            //}
        }
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCustBatchUpsert()
        {
            string batchData;
            using (var reader = new StreamReader(Request.Body))
            {
                batchData = reader.ReadToEnd();
            }
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("para.cust_batch_upsert", conn))
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
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        public JsonResult OnGetCust()
        {
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("para.cust_get", conn))
                {
                    dynamic flexible = new ExpandoObject();
                    foreach (var key in Request.Query.Keys) {
                        string value = Request.Query[key].ToString();
                        AddProperty(flexible, key, value);
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

        public JsonResult OnDeleteCust(int cif)
        {
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("para.cust_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, "{\"cif\":"+cif+"}");
                    NpgsqlParameter outParam = new NpgsqlParameter("@o_json", NpgsqlTypes.NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outParam);
                    cmd.ExecuteNonQuery();
                    return new JsonResult(outParam.Value);
                }
            }
        }

        //private string GetDocumentContents(HttpRequest Request)
        //{
        //    string documentContents;
        //    using (Stream receiveStream = Request.InputStream)
        //    {
        //        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
        //        {
        //            documentContents = readStream.ReadToEnd();
        //        }
        //    }
        //    return documentContents;
        //}
    }
}