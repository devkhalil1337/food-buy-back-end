using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLayer
{
    public interface IDatabaseAccessProvider : IDisposable
    {
        void OpenConnection();
        void CloseConnection();
        DataTable GetTable(string tableName);
        DataTable Query(string queryString);
        void UpdateTable(DataTable table);
        DataSet ExecuteStoredProcedure(string procName, Dictionary<string, object> parameters);
        object ExecuteStoredProcedureWithReturnObject(string procName, Dictionary<string, object> parameters);
        bool ExecuteNonQueryStoredProcedure(string procName, Dictionary<string, object> parameters);
    }
}
