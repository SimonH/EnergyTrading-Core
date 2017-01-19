using System.Collections.Generic;
using System.Data;

namespace EnergyTrading.Data.SimpleData
{
    public class UseRawSqlMock : IUseRawSql
    {
        readonly IDictionary<string, object> _setResults = new Dictionary<string, object>();
        public void SetCurrentConnection(IDbConnection connection)
        {
        }

        private string NullString(string source)
        {
            return source ?? "null";
        }

        private string GenerateKey(string sql, IDictionary<string, object> parameters)
        {
            return NullString(sql) + "#" + string.Join("#", parameters.Keys);
        } 

        public void Setup(string sql, IDictionary<string, object> parameters, object result)
        {
            _setResults.Add(GenerateKey(sql, parameters), result);
        }

        private object GetSetResults(string sql, IDictionary<string, object> parameters)
        {
            var key = GenerateKey(sql, parameters);
            return _setResults.ContainsKey(key) ? _setResults[key] : null;
        } 

        public IEnumerable<IEnumerable<dynamic>> GetResultSet(string sql, IDictionary<string, object> parameters)
        {
            return GetSetResults(sql, parameters) as IEnumerable<IEnumerable<dynamic>>; 
        }

        public IEnumerable<dynamic> GetRows(string sql, IDictionary<string, object> parameters)
        {
            return GetSetResults(sql, parameters) as IEnumerable<dynamic>;
        }

        public dynamic GetRow(string sql, IDictionary<string, object> parameters)
        {
            return GetSetResults(sql, parameters);
        }

        public object GetScalar(string sql, IDictionary<string, object> parameters)
        {
            return GetSetResults(sql, parameters);
        }

        public int ExecuteNonQuery(string sql, IDictionary<string, object> parameters)
        {
            var result = GetSetResults(sql, parameters);
            if (result is int)
            {
                return (int)result;
            }
            return int.MinValue;
        }
    }
}