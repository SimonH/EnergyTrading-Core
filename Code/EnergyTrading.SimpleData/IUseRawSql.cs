using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace EnergyTrading.Data.SimpleData
{
    public interface IUseRawSql
    {
        void SetCurrentConnection(IDbConnection connection);
        IEnumerable<IEnumerable<dynamic>> GetResultSet(string sql, IDictionary<string, object> parameters);
        IEnumerable<dynamic> GetRows(string sql, IDictionary<string, object> parameters);
        dynamic GetRow(string sql, IDictionary<string, object> parameters);
        object GetScalar(string sql, IDictionary<string, object> parameters);
        int ExecuteNonQuery(string sql, IDictionary<string, object> parameters);
    }
}