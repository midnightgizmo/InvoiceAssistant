using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlServerCe;

namespace InvoiceAssistant.Database
{
    class Connection
    {
        private SqlCeConnection _Con;

        private static string conString = @"Data Source = C:\MyStuff\Programing Project\InvoiceAssistant\InvoiceAssistant\Database\Invoices.sdf";
        private int commandTimeOut = 60;

        public Connection()
        {
            _Con = new SqlCeConnection(Connection.conString);

        }

        public bool OpenConnection()
        {
            try
            {
                if (_Con.State == System.Data.ConnectionState.Closed)
                    _Con.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                _Con.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _Con.Dispose();
            }
        }

        public SqlCeDataReader ExecuteSelectCommand(string sqlSelectCommand)
        {
            SqlCeCommand cmd = new SqlCeCommand(sqlSelectCommand, _Con);
            cmd.CommandTimeout = commandTimeOut;
            try
            {
                return cmd.ExecuteReader();
            }
            catch
            {
                return null;
            }
        }

        public SqlCeDataReader ExecuteStoredProcedure(string procedureName)
        {
            SqlCeCommand cmd = new SqlCeCommand(procedureName, _Con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = commandTimeOut;

            try
            {
                return cmd.ExecuteReader();
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public object ExecuteScalar(string SQLCommand)
        {

            //cmd.CommandTimeout = commandTimeOut;
            string[] querys = SQLCommand.Split(new char[] { ';' });
            object returnValue = -1;

            foreach (string query in querys)
            {
                if (query.Length > 0)
                {

                    try
                    {
                        SqlCeCommand cmd = new SqlCeCommand(query, _Con);
                        returnValue = cmd.ExecuteScalar();
                        cmd.Dispose();
                    }
                    catch
                    {
                        //return -1;
                    }
                }
            }



            return returnValue;

        }

        public int ExecuteNonReader(string SQLCommand)
        {
            SqlCeCommand cmd = new SqlCeCommand(SQLCommand, _Con);
            //cmd.CommandTimeout = commandTimeOut;
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public DataSet ExecuteDataSet(string SQLCommand)
        {
            DataSet tmpDS = new DataSet();
            SqlCeDataAdapter dapt = new SqlCeDataAdapter(SQLCommand, _Con);

            try
            {
                dapt.Fill(tmpDS, "table");
                return tmpDS;
            }
            catch
            {
                tmpDS.Dispose();

                return null;
            }
            finally
            {
                dapt.Dispose();
            }



        }

        public static string ConnectionString
        {
            get
            {
                return conString;
            }
        }


        #region parameterized query functions
        public DataSet ExecuteParameterizedDataSet(string SQLCommand)
        {
            return ExecuteParameterizedDataSet(SQLCommand, new SqlCeParameter[] { });
        }
        public DataSet ExecuteParameterizedDataSet(string SQLCommand, SqlCeParameter[] parameters)
        {
            DataSet tempDS = new DataSet();
            OpenConnection();
            using (SqlCeCommand command = new SqlCeCommand(SQLCommand, _Con))
            {
                SqlCeDataAdapter dapt;

                foreach(SqlCeParameter param in parameters)
                    command.Parameters.Add(param);

                dapt = new SqlCeDataAdapter(command);
                dapt.Fill(tempDS, "table");

                
            }

            CloseConnection();

            return tempDS;
        }

        public int ExecuteParameterizedNonReader(string SQLCommand, SqlCeParameter[] parameters)
        {
            int returnValue;
            OpenConnection();

            using(SqlCeCommand command = new SqlCeCommand(SQLCommand, _Con))
            {

                foreach (SqlCeParameter param in parameters)
                    command.Parameters.Add(param);

                returnValue = command.ExecuteNonQuery();
                
            }

            CloseConnection();

            return returnValue;
        }

        public object ExecuteParameterizedScalar(string SQLCommand, SqlCeParameter[] parameters)
        {
            object returnValue = null;
            string[] querys = SQLCommand.Split(new char[] { ';' });
            OpenConnection();

            for (int eachQuery = 0; eachQuery < querys.Count(); eachQuery++)
            {
                string query = querys[eachQuery];

                if (query.Length > 0)
                {
                    if (eachQuery == 0)
                    {
                        using (SqlCeCommand command = new SqlCeCommand(query, _Con))
                        {

                            foreach (SqlCeParameter param in parameters)
                                command.Parameters.Add(param);

                            returnValue = command.ExecuteScalar();

                        }
                    }
                    else
                    {
                        using (SqlCeCommand command = new SqlCeCommand(query, _Con))
                        {

                            returnValue = command.ExecuteScalar();

                        }
                    }
                    
                }
            }

            CloseConnection();

            return returnValue;
        }
        #endregion

    }
}
