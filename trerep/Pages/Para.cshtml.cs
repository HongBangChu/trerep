﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.IO;
using System.Data;

namespace trerep.Pages
{
    public class ParaModel : PageModel
    {
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

        public JsonResult OnGetCust()
        {
            using (var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("para.cust_get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_params", NpgsqlTypes.NpgsqlDbType.Text, DBNull.Value);
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