using System;
using System.Collections.Generic;
using System.Data;

namespace EnergyTrading.Data.SimpleData
{
    public class UseRawSql : IUseRawSql
    {
        private IDbConnection _currentConnection;
        public void SetCurrentConnection(IDbConnection connection)
        {
            _currentConnection = connection;
        }

        private void CheckCurrentConnection()
        {
            if (_currentConnection == null || _currentConnection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("You must have a valid IDbConnection to use raw sql. Are you running inside PerformDatabaseAction?");
            }
        }

        private void CheckSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentOutOfRangeException(nameof(sql), "sql statement cannot be null or white space!");
            }
        }

        private IDictionary<string, object> CheckInput(string sql, IDictionary<string, object> parameters)
        {
            CheckCurrentConnection();
            CheckSql(sql);
            return parameters ?? new Dictionary<string, object>();
        }

        public IEnumerable<IEnumerable<dynamic>> GetResultSet(string sql, IDictionary<string, object> parameters)
        {
            var validParameters = CheckInput(sql, parameters);
            return _currentConnection.ToResultSets(sql, validParameters);
        }

        public IEnumerable<dynamic> GetRows(string sql, IDictionary<string, object> parameters)
        {
            var validParameters = CheckInput(sql, parameters);
            return _currentConnection.ToRows(sql, validParameters);
        }

        public dynamic GetRow(string sql, IDictionary<string, object> parameters)
        {
            var validParameters = CheckInput(sql, parameters);
            return _currentConnection.ToRow(sql, validParameters);
        }

        public object GetScalar(string sql, IDictionary<string, object> parameters)
        {
            var validParameters = CheckInput(sql, parameters);
            return _currentConnection.ToScalar(sql, validParameters);
        }

        public int ExecuteNonQuery(string sql, IDictionary<string, object> parameters)
        {
            var validParameters = CheckInput(sql, parameters);
            return _currentConnection.ExecuteNonQuery(sql, validParameters);
        }
    }
}