using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Npgsql;

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
            using(var conn = new NpgsqlConnection(_connStr))
            {
                conn.Open();
                // Insert some data
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO cust (cif) VALUES (@p)";
                    cmd.Parameters.AddWithValue("p", 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}