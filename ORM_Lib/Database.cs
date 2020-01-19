using ORM_Lib.Cache;
using ORM_Lib.Deserialization;
using ORM_Lib.Query;
using ORM_Lib.Query.Create;
using ORM_Lib.Query.Insert;
using ORM_Lib.Query.Select;
using ORM_Lib.Query.Update;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ORM_Lib
{
    // TODO: transactions
    internal class Database
    {
        private Func<IDbConnection> _connection;
        private DbContext _ctx;

        public Database(Func<IDbConnection> connection, DbContext ctx)
        {
            _connection = connection;
            _ctx = ctx;
        }

        public IEnumerable<T> ExecuteQuery<T>(SelectQuery<T> query)
        {
            using var connection = _connection.Invoke();
            connection.Open();
            using IDbTransaction transaction = connection.BeginTransaction();
            using var command = connection.CreateCommand();
            command.CommandText = query.AsSqlString();
            command.Transaction = transaction;
            PrepareStatement(query, command);
            try
            {
                var reader = command.ExecuteReader();
                var objectReader = new ObjectReader<T>(_ctx, reader, query._entityExecutedOn, query._combinedQueryColumns);
                var result = objectReader.Serialize();
                reader.Close();
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                transaction.Rollback();
                throw ex;
            }
        }

        public List<T> ExecuteInsert<T>(InsertStatement<T> statement)
        {
            using var connection = _connection.Invoke();
            connection.Open();
            using IDbTransaction transaction = connection.BeginTransaction();
            using var command = connection.CreateCommand();
            command.CommandText = statement.AsSqlString();
            command.Transaction = transaction;
            PrepareStatement(statement, command);
          
            try
            {
                var reader = command.ExecuteReader();

                var entity = statement._entityExecutedOn;
                var pkCol = entity.PkColumn;
                var pocos = statement._pocos;
                var cache = _ctx.Cache;

                var enumerator = pocos.GetEnumerator();
                // now we set the pks the db returned on our pocos and add them to the cache
                while (reader.Read() && enumerator.MoveNext())
                {
                    var poco = enumerator.Current;
                    var pk = reader[pkCol.Name];
                    pkCol.PropInfo.SetMethod.Invoke(poco, new[] { pk });
                    var cacheEntry = cache.GetOrInsert(entity, (long)pk, poco);

                    // now we fill originalPoco
                    foreach (var col in entity.CombinedColumns())
                    {
                        if (col.IsDbColumn)
                        {
                            if (col.IsShadowAttribute)
                            {
                                // TODO: maybe no entry?
                                cacheEntry.ShadowAttributes[col.Name] = null;
                            }
                            else
                            {
                                var value = col.PropInfo.GetMethod.Invoke(poco, new object[0]);
                                // fill the originalEntries with values for changeTracking later
                                if (!cacheEntry.OriginalPoco.ContainsKey(col.Name)) cacheEntry.OriginalPoco.Add(col.Name, value);
                            }
                        }
                    }
                    LazyLoadInjector.InjectLazyLoader<T>(poco, _ctx, statement._entityExecutedOn);
                }
                reader.Close();
                transaction.Commit();
                return pocos;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                transaction.Rollback();
                throw ex;
            }
        }

        public int ExecuteDDL(DdlStatement ddlStatement)
        {
            return ExecuteNonQuery(ddlStatement);
        }

        public void SaveUpdateChanges(UpdateBatch updateBatch)
        {
            ExecuteNonQuery(updateBatch);
        }

        public void SaveInsertChanges(ManyToManyInsertBatch insertBatch)
        {
            ExecuteNonQuery(insertBatch);
        }

        private int ExecuteNonQuery(ISqlExpression sqlExpression)
        {
            var rowsAffected = -1;
            using var connection = _connection.Invoke();
            connection.Open();
            using IDbTransaction transaction = connection.BeginTransaction();
            using var command = connection.CreateCommand();
            command.CommandText = sqlExpression.AsSqlString();
            PrepareStatement(sqlExpression, command);
            command.Transaction = transaction;
            try
            {
                rowsAffected = command.ExecuteNonQuery();
                transaction.Commit();
                Console.WriteLine("Transaction successful");
                return rowsAffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                // most rollback per default but we do not want to rely on that
                transaction.Rollback();
                throw ex;
            }
        }


        private void PrepareStatement(ISqlExpression query, IDbCommand command)
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
