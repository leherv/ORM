using Npgsql;
using ORM_Lib.DbSchema;
using ORM_Lib.Deserialization;
using System;
using System.Collections.Generic;

namespace ORM_Lib
{
    internal class Database
    {

        private String _connectionString;

        public Database(String connectionString)
        {
            _connectionString = connectionString;
      
        }

        public int ExecuteDDL(String ddlString)
        {
            var rowsAffected = -1;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            //using var transaction = connection.BeginTransaction();
            //using var command = new NpgsqlCommand(ddlString, connection, transaction);
            using var command = new NpgsqlCommand(ddlString, connection);
            //int paramValue = 5;
            //command.Parameters.AddWithValue("@pricePoint", paramValue);
            try
            {
                rowsAffected = command.ExecuteNonQuery();
                //transaction.Commit();
                Console.WriteLine("Transaction successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                //transaction.Rollback();
            }
            connection.Close();
            return rowsAffected;

        }

        public IEnumerable<T> ExecuteQuery<T>(String queryString, Entity entity, List<(Entity, List<(Column, string)>)> queriedColumns)
        {
         
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand(queryString, connection);
            var objectReader = new ObjectReader<T>(command.ExecuteReader(), entity, queriedColumns);
            var result = objectReader.Serialize();
            connection.Close();
            return result;
        }

    }
}
