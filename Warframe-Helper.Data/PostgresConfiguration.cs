using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Helper.Data
{
    public class PostgresConfiguration
    {

        public required string Host { get; set; }
        public required string Port { get; set; }
        public required string Database { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        
        public string GetConnectionString() {

            return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
        }
    }
}
