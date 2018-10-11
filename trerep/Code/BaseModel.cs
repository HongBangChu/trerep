using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
    }
}
