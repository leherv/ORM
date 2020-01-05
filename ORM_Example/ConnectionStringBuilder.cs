using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Example
{
    class ConnectionStringBuilder
    {
        public static String  connectionString()
        {
            var builder = new NpgsqlConnectionStringBuilder(true);
            builder.Host = "127.0.0.1";
            builder.Username = "user";
            builder.Password = "1234";
            builder.Port = 5432;
            builder.Database = "orm_db";
            return builder.ConnectionString;
        }
        
    }
}
