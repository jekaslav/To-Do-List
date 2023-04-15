using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.Entities;
using WebApplication1.Interfaces;

namespace WebApplication1
{
    public class DbContext
    {
        private readonly DbContext _configuration;
        private readonly string _connectionString;
        
        public DbContext(DbContext configuration)
        {
            _configuration = configuration;
            _connectionString = "Server=DESKTOP-20MC601\\MSSQLSERVER01; Database=TodoItems; Trusted_Connection=true; TrustServerCertificate=True;";
        }
        

        public IDbConnection CreateConnection() 
                => new SqlConnection(_connectionString);
    }
}