using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
            using var transaction = connection.BeginTransaction();
            using var command = new NpgsqlCommand(ddlString, connection, transaction);
            //int paramValue = 5;
            //command.Parameters.AddWithValue("@pricePoint", paramValue);
            try
            {
                rowsAffected = command.ExecuteNonQuery();
                transaction.Commit();
                Console.WriteLine("Transaction successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                transaction.Rollback();
            }
            connection.Close();
            return rowsAffected;

        }

        public IDataReader ExecuteQuery(String queryString)
        {
            SqlDataReader sqlDataReader = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var command = new NpgsqlCommand(queryString, connection, transaction);
            //int paramValue = 5;
            //command.Parameters.AddWithValue("@pricePoint", paramValue);

            try
            {
                var reader = command.ExecuteReader();
                transaction.Commit();
                Console.WriteLine("Transaction successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                transaction.Rollback();
            }
            connection.Close();
            return sqlDataReader;
        }

    }
}
