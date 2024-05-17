using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Utility
{
    internal class UDbconnect
    {
        private static IConfiguration _iconfiguration;  //packages need to be installed for including json file--IConfiguration-building interface
        static UDbconnect()
        {
            Getappsettingfile();
        }
        private static void Getappsettingfile()                 //Adding explicitly a json file
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json");
            _iconfiguration = builder.Build();
        }
        public static string Getconnectstring()
        {
            return _iconfiguration.GetConnectionString("Database_connectionString");
        }
    }
}
