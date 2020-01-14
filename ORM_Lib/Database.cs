using Npgsql;
using ORM_Lib.DbSchema;
using ORM_Lib.Deserialization;
using ORM_Lib.Query.Insert;
using ORM_Lib.Query.Select;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;

namespace ORM_Lib
{
    internal class Database
    {

        private String _connectionString;
        private DbContext _ctx;

        public Database(String connectionString, DbContext ctx)
        {
            _connectionString = connectionString;
            _ctx = ctx;
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

        public IEnumerable<T> ExecuteQuery<T>(SelectQuery<T> query)
        {
            //TODO: generalize to DbConnection and DbCommand 
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand(query.AsSqlString(), connection);
            PrepareStatement(query, command);
            var objectReader = new ObjectReader<T>(_ctx, command.ExecuteReader(), query._entityExecutedOn, query._combinedQueryColumns);
            var result = objectReader.Serialize();
            connection.Close();
            return result;
        }

        public void ExecuteInsert<T>(InsertStatement<T> statement)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand(statement.AsSqlString(), connection);
            PrepareStatement(statement, command);
            var reader = command.ExecuteReader();
            //var hasRows = reader.HasRows;
            //var f = reader.GetValue(1);

            //// now we set the pks the db returned on our pocos and add them to the cache
            //var entity = statement._entityExecutedOn;
            //var pkCol = entity.PkColumn;
            //var pocos = statement._pocos;
            //var cache = _ctx.Cache;

            //foreach (var poco in pocos)
            //{
            //    var pk = reader[pkCol.Name];
            //    pkCol.PropInfo.SetMethod.Invoke(poco, new[] { pk });
            //    var cacheEntry = cache.GetOrInsert(entity, (long)pk, poco);

            //    // now we fill originalPoco
            //    foreach (var col in entity.Columns)
            //    {
            //        if (col.IsDbColumn)
            //        {
            //            if (col.IsShadowAttribute)
            //            {
            //                //cacheEntry.ShadowAttributes
            //            }
            //            else
            //            {
            //                var value = col.PropInfo.GetMethod.Invoke(poco, new object[] { });
            //                // fill the originalEntries with values for changeTracking later
            //                if (!cacheEntry.OriginalPoco.ContainsKey(col.Name)) cacheEntry.OriginalPoco.Add(col.Name, value);

            //            }
            //        }
            //    }
            //}
            connection.Close();
        }


        private void PrepareStatement(ISqlExpression query, NpgsqlCommand command)
        {
            foreach (var namedParam in query.GetNamedParams())
            {
                var parameter = command.CreateParameter();
                parameter.Value = namedParam.Value ?? DBNull.Value;
                parameter.DbType = namedParam.DbType;
                parameter.ParameterName = namedParam.Alias;
                command.Parameters.Add(parameter);
            }
        }


    }
}
