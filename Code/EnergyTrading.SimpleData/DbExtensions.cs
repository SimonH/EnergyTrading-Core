using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Simple.Data;
using Simple.Data.Ado;

namespace EnergyTrading.Data.SimpleData
{
    internal static class DbExtensions
    {
        public static IEnumerable<IEnumerable<dynamic>> ToResultSets(this IDbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            return connection.RunAsCommand(sql, parameters, c => c.UsingReader(r => r.ToResultSets()));
        }  

        public static IEnumerable<dynamic> ToRows(this IDbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            return connection.RunAsCommand(sql, parameters, c => c.UsingReader(r => r.ToRows()));
        }

        public static dynamic ToRow(this IDbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            return connection.RunAsCommand(sql, parameters, c => c.UsingReader(r => r.ToRow()));
        }
        
        public static object ToScalar(this IDbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            return connection.RunAsCommand(sql, parameters, c => c.ToScalar());
        } 
        
        public static int ExecuteNonQuery(this IDbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            return connection.RunAsCommand(sql, parameters, c => c.ExecuteNonQuery());
        }  


        private static TReturn RunAsCommand<TReturn>(this IDbConnection connection, string sql, IDictionary<string, object> parameters, Func<IDbCommand, TReturn> commandFunc)
        {
            return commandFunc(connection.BuildCommand(sql, parameters));
        }  

        private static IDbCommand BuildCommand(this IDbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            var ret = connection.CreateCommand();
            ret.Connection = connection;
            ret.CommandText = sql;

            parameters.ToList().ForEach(ret.AddParameter);
            return ret;
        }

        private static void AddParameter(this IDbCommand command, KeyValuePair<string, object> data)
        {
            var param = command.CreateParameter();
            param.ParameterName = data.Key;
            param.Value = data.Value ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        private static TReturn UsingReader<TReturn>(this IDbCommand command, Func<IDataReader, TReturn> readerFunc)
        {
            using (var rdr = command.ExecuteReader())
            {
                return readerFunc(rdr);
            }
        }

        private static object ToScalar(this IDbCommand command)
        {
            var result = command.ExecuteScalar();
            return result == DBNull.Value ? null : result;
        }

        private static IEnumerable<IEnumerable<dynamic>> ToResultSets(this IDataReader dataReader)
        {
            return dataReader.ToMultipleDictionaries().ToResultSets();
        }

        private static IEnumerable<dynamic> ToRows(this IDataReader reader)
        {
            return reader.ToDictionaries().ToRows();
        }

        private static dynamic ToRow(this IDataReader reader)
        {
            return reader.Read() ? ((IDataRecord) reader).ToDictionary().ToRow() : null;
        }

        private static IEnumerable<IEnumerable<dynamic>> ToResultSets(this IEnumerable<IEnumerable<IDictionary<string, object>>> data)
        {
            return data.Select(ToRows).ToArray().AsEnumerable();
        }
        
        private static IEnumerable<dynamic> ToRows(this IEnumerable<IDictionary<string, object>> data)
        {
            return new SimpleList(data.Select(ToRow).ToArray().AsEnumerable());
        }
        
        private static SimpleRecord ToRow(this IDictionary<string, object> data)
        {
            return new SimpleRecord(data);
        } 
    }
}