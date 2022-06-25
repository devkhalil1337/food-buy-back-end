using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace DataAccessLayer
{
    public class SqlDataAccess : IDatabaseAccessProvider
    {
        private SqlConnection _SqlConnection;
        private SqlDataAdapter _SqlAdapter;
        private SqlCommandBuilder _SqlCommandBuilder;

        private string _connectionString;

        public SqlDataAccess(string connectionString)
        {
            this._SqlConnection = new SqlConnection(connectionString);
            _connectionString = connectionString;
        }

        public void OpenConnection()
        {
            this._SqlConnection.Open();
        }

        public void CloseConnection()
        {
            this._SqlConnection.Close();
        }

        public DataTable GetTable(string tableName)
        {
            DataTable dataTable = new DataTable(tableName);
            this._SqlAdapter = new SqlDataAdapter(string.Format("SELECT * FROM {0};", (object)tableName), this._SqlConnection);
            this._SqlCommandBuilder = new SqlCommandBuilder(this._SqlAdapter);
            this._SqlAdapter.Fill(dataTable);
            return dataTable;
        }

        public DataTable Query(string queryString)
        {
            DataTable dataTable = new DataTable();
            this._SqlAdapter = new SqlDataAdapter(queryString, this._SqlConnection);
            this._SqlCommandBuilder = new SqlCommandBuilder(this._SqlAdapter);
            this._SqlAdapter.Fill(dataTable);
            return dataTable;
        }

        public void UpdateTable(DataTable table)
        {
            ((SqlDataAdapter)this._SqlAdapter).Update(table);
        }

        public DataSet ExecuteStoredProcedure(string procName, Dictionary<string, object> parameters)
        {
            DataSet dataSet;

            try
            {
                this._SqlConnection.Open();

                var command = new SqlCommand(procName, this._SqlConnection) { CommandType = CommandType.StoredProcedure };

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> pair in parameters)
                    {
                        var parameter = new SqlParameter(pair.Key, pair.Value);
                        SetSqlType(ref parameter, pair.Value);
                        command.Parameters.Add(parameter);
                    }
                }

                this._SqlAdapter = new SqlDataAdapter { SelectCommand = command };
                dataSet = new DataSet();
                this._SqlAdapter.Fill(dataSet);
            }
            finally
            {
                this._SqlConnection.Close();
            }
            return dataSet;
        }

        public object ExecuteStoredProcedureWithReturnObject(string procName, Dictionary<string, object> parameters)
        {
            object result = null;

            try
            {
                this._SqlConnection.Open();

                var command = new SqlCommand(procName, this._SqlConnection) { CommandType = CommandType.StoredProcedure };

                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    var parameter = new SqlParameter(pair.Key, pair.Value);
                    SetSqlType(ref parameter, pair.Value);
                    command.Parameters.Add(parameter);
                }

                result = command.ExecuteScalar();
            }
            finally
            {
                this._SqlConnection.Close();
            }
            return result;
        }

        public bool ExecuteNonQueryStoredProcedure(string procName, Dictionary<string, object> parameters)
        {
            bool result;

            try
            {
                this._SqlConnection.Open();
                var command = new SqlCommand(procName, this._SqlConnection) { CommandType = CommandType.StoredProcedure };

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> pair in parameters)
                    {
                        var parameter = new SqlParameter(pair.Key, pair.Value);
                        SetSqlType(ref parameter, pair.Value);
                        command.Parameters.Add(parameter);
                    }
                }

                int rowsAffected = command.ExecuteNonQuery();
                result = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                result = false;
            }
            finally
            {
                this._SqlConnection.Close();
            }
            return result;
        }

        private void SetSqlType(ref SqlParameter parameter, object value)
        {
            if (value is long)
            {
                parameter.SqlDbType = SqlDbType.BigInt;
            }
            else if (value is bool)
            {
                parameter.SqlDbType = SqlDbType.SmallInt;
            }
            else if (value is string)
            {
                parameter.SqlDbType = SqlDbType.NVarChar;
            }
            else if (value is byte[])
            {
                parameter.SqlDbType = SqlDbType.Image;
            }
            else if (value is int)
            {
                parameter.SqlDbType = SqlDbType.Int;
            }
            else if (value is DateTime)
            {
                parameter.SqlDbType = SqlDbType.DateTime2;
            }
        }
        public void Dispose()
        {
            if (this._SqlConnection == null)
                return;
            this._SqlConnection.Close();
        }
    }
}
