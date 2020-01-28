using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;

namespace Inventory.NotificationScheduler
{
    public class DataSourceCommunicator
    {
        private string ConnectionString = "";
        private SqlConnectionStringBuilder ConnStringBuilder;
        private string DBServerName = "";
        private string DBName = "";
        private Dictionary<string, object> ParamCollection;


        public enum ParamType
        {
            ConnectionString,
            ServerName,
            ServerCredentials
        };

        public DataSourceCommunicator()
        {
            Initialize();
        }

        /// <summary>
        /// Used to Initialize a database connection with or without integrated security
        /// </summary>
        /// <param name="PType">Use ParamType defined in same class</param>
        /// <param name="Param">This should be the name of the server that you need to connect to</param>
        /// <param name="DBUserName">SQL User name. If you wish to use Integrated security, use a different constructor.</param>
        /// <param name="DBPassword">SQL Password</param>
        public DataSourceCommunicator(ParamType PType, string Param, string DBUserName = "", string DBPassword = "")
        {
            try
            {
                //writeLogToFile("Testing", String.Format("Constructor: Values {0} {1} {2} {3}",PType.ToString(),Param,DBUserName,DBPassword));
                if (PType.Equals(ParamType.ConnectionString))
                {
                    ParamCollection = new Dictionary<string, object>();
                    this.ConnectionString = Param;
                }
                else if (PType.Equals(ParamType.ServerName))
                {
                    Initialize(Param);
                    ConnectionString = ConnStringBuilder.ConnectionString;
                }
                else if (PType.Equals(ParamType.ServerCredentials) && !(String.IsNullOrEmpty(DBUserName) && String.IsNullOrEmpty(DBPassword)))
                {
                    //writeLogToFile("Testing", String.Format("Calling Initialize: Values {0} {1} {2} {3}", PType.ToString(), Param, DBUserName, DBPassword));
                    Initialize(Param, DBUserName, DBPassword);
                    ConnectionString = ConnStringBuilder.ConnectionString;
                }
            }
            catch (Exception ex)
            {
                writeLogToFile("Constructor", ex.Message);
                throw;
            }
        }

        private void Initialize(string ParamServerName = "", string ParamDBUserName = "", string ParamDBPassword = "")
        {
            try
            {
                ConnStringBuilder = new SqlConnectionStringBuilder();
                ParamCollection = new Dictionary<string, object>();

                if (!String.IsNullOrEmpty(ParamServerName))
                {
                    //writeLogToFile("Testing", "Using Server Name");
                    DBServerName = ParamServerName;
                }

                if (String.IsNullOrEmpty(ParamDBUserName) || String.IsNullOrEmpty(ParamDBPassword))
                {
                    //writeLogToFile("Testing", "Username Password is empty");
                    ConnStringBuilder.IntegratedSecurity = true;
                }
                else if (!(String.IsNullOrEmpty(ParamDBUserName) || String.IsNullOrEmpty(ParamDBPassword)))
                {
                    //writeLogToFile("Testing","Setting security to false");
                    ConnStringBuilder.IntegratedSecurity = false;
                    ConnStringBuilder.UserID = ParamDBUserName;
                    ConnStringBuilder.Password = ParamDBPassword;
                }

                ConnStringBuilder.DataSource = DBServerName;
                ConnStringBuilder.InitialCatalog = DBName;

                this.ConnectionString = ConnStringBuilder.ConnectionString;
                //writeLogToFile("Final Connection String", ConnStringBuilder.ConnectionString);
            }
            catch (Exception ex)
            {
                writeLogToFile("Initialize", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Execute a stored procedure using the DataSourceCommunicator class
        /// </summary>
        /// <param name="SPName">Name of the stored proc</param>
        /// <param name="ParamDBName">[Optional] The database in which the Stored Proc Exists</param>
        /// <returns></returns>
        public string ExecuteStoredProcedure(string SPName, string ParamDBName = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(ParamDBName))
                {
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                else
                {
                    ParamDBName = "";//System.Configuration.ConfigurationManager.AppSettings["dbDatabaseName"].ToString();
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }

                SqlConnection DBConnection = new SqlConnection(this.ConnectionString);
                DBConnection.Open();
                SqlCommand Command = InitializeQuery(SPName, DBConnection);

                DataSet ResultDS = new DataSet();
                SqlDataAdapter DataAdapter = new SqlDataAdapter(Command);
                DataAdapter.Fill(ResultDS);

                string ReturnString = ResultDS.GetXml();

                DBConnection.Close();
                DBConnection.Dispose();
                ResultDS.Dispose();
                DataAdapter.Dispose();
                Command.Dispose();
                ParamCollection.Clear();

                return ReturnString;
            }
            catch (Exception ex)
            {
                writeLogToFile("ExecuteStoredProcedure", ex.Message);
                ClearParams();
                throw;
            }
        }

        /// <summary>
        /// Execute a stored procedure using the DataSourceCommunicator class
        /// </summary>
        /// <param name="SPName">Name of the stored proc</param>
        /// <param name="ParamDBName">[Optional] The database in which the Stored Proc Exists</param>
        /// <returns></returns>
        public DataSet ExecuteStoredProcedureAsDataSet(string SPName, string ParamDBName = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(ParamDBName))
                {
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                else
                {
                    ParamDBName = "";// System.Configuration.ConfigurationManager.AppSettings["dbDatabaseName"].ToString();
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                SqlConnection DBConnection = new SqlConnection(this.ConnectionString);
                DBConnection.Open();
                SqlCommand Command = InitializeQuery(SPName, DBConnection);

                DataSet ResultDS = new DataSet();
                SqlDataAdapter DataAdapter = new SqlDataAdapter(Command);
                DataAdapter.Fill(ResultDS);

               
                DBConnection.Close();
                DBConnection.Dispose();
                ResultDS.Dispose();
                DataAdapter.Dispose();
                Command.Dispose();
                ParamCollection.Clear();

                return ResultDS;
            }
            catch (Exception ex)
            {
                writeLogToFile("ExecuteStoredProcedure", ex.Message);
                ClearParams();
                throw;
            }
        }


        public object ExecuteScalar(string SPName, string ParamDBName = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(ParamDBName))
                {
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                else
                {
                    ParamDBName = "";// System.Configuration.ConfigurationManager.AppSettings["dbDatabaseName"].ToString();
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                SqlConnection DBConnection = new SqlConnection(this.ConnectionString);
                DBConnection.Open();
                SqlCommand Command = InitializeQuery(SPName, DBConnection);
                object Result = Command.ExecuteScalar();

                DBConnection.Close();
                DBConnection.Dispose();
                Command.Dispose();
                ParamCollection.Clear();

                return Result;
            }
            catch (Exception ex)
            {
                writeLogToFile("ExecuteScalar", ex.Message);
                ClearParams();
                throw;
            }
        }


        public int ExecuteNonQuery(string SPName, string ParamDBName = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(ParamDBName))
                {
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                else
                {
                    ParamDBName = "";// System.Configuration.ConfigurationManager.AppSettings["dbDatabaseName"].ToString();
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                SqlConnection DBConnection = new SqlConnection(this.ConnectionString);
                DBConnection.Open();
                SqlCommand Command = InitializeQuery(SPName, DBConnection);
                int Result = Command.ExecuteNonQuery();

                DBConnection.Close();
                DBConnection.Dispose();
                Command.Dispose();
                ParamCollection.Clear();

                return Result;
            }
            catch (Exception ex)
            {
                writeLogToFile("ExecuteNonQuery", ex.Message);
                throw;
            }
        }

        public DataSet FillDataSet(string SPName, string ParamDBName = "")
        {
            try
            {
                DataSet dSet=new DataSet();
                if (!String.IsNullOrEmpty(ParamDBName))
                {
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                else
                {
                    ParamDBName = "";//System.Configuration.ConfigurationManager.AppSettings["dbDatabaseName"].ToString();
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                SqlConnection DBConnection = new SqlConnection(this.ConnectionString);
                DBConnection.Open();
                SqlCommand Command = InitializeQuery(SPName, DBConnection);
                SqlDataAdapter dAdr = new SqlDataAdapter(Command);
                //int Result = Command.ExecuteNonQuery();
                dAdr.Fill(dSet);
                DBConnection.Close();
                DBConnection.Dispose();
                Command.Dispose();
                ParamCollection.Clear();

                return dSet;
            }
            catch (Exception ex)
            {
                writeLogToFile("ExecuteNonQuery", ex.Message);
                throw;
            }
        }

        private SqlDataReader ExecuteReader(string SPName, string ParamDBName = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(ParamDBName))
                {
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                else
                {
                    ParamDBName = "";// System.Configuration.ConfigurationManager.AppSettings["dbDatabaseName"].ToString();
                    ConnStringBuilder.InitialCatalog = ParamDBName;
                    this.ConnectionString = ConnStringBuilder.ConnectionString;
                }
                SqlConnection DBConnection = new SqlConnection(this.ConnectionString);
                DBConnection.Open();
                SqlCommand Command = InitializeQuery(SPName, DBConnection);
                SqlDataReader Reader = Command.ExecuteReader();

                DBConnection.Close();
                DBConnection.Dispose();
                Command.Dispose();
                ParamCollection.Clear();

                return Reader;
            }
            catch (Exception ex)
            {
                writeLogToFile("ExecuteReader", ex.Message);
                return null;
            }
        }

        private SqlCommand InitializeQuery(string SPName, ArrayList ParamNames, ArrayList ParamValues, SqlConnection DBConnection)
        {
            try
            {
                SqlCommand com = new SqlCommand(SPName, DBConnection);

                com.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < ParamNames.Count; i++)
                {
                    com.Parameters.AddWithValue(ParamNames[i].ToString(), ParamValues[i].ToString());
                }
                return com;
            }
            catch (Exception ex)
            {
                writeLogToFile("InitializeQuery", ex.Message);
                throw;
            }
            finally
            {
                ParamNames.Clear();
                ParamValues.Clear();
            }
        }

        private SqlCommand InitializeQuery(string SPName, SqlConnection DBConnection)
        {
            try
            {
                SqlCommand com = new SqlCommand(SPName, DBConnection);

                com.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < ParamCollection.Count; i++)
                {
                    com.Parameters.AddWithValue(ParamCollection.Keys.ToArray()[i], ParamCollection.Values.ToArray()[i]);
                }
                return com;
            }
            catch (Exception ex)
            {
                writeLogToFile("InitializeQuery", ex.Message);
                throw;
            }
            finally
            {
                ParamCollection.Clear();
            }
        }
        
        public void AddParameter(string ParamName, object ParamValue)
        {
            try
            {
                ParamCollection.Add(ParamName, ParamValue);
            }
            catch (Exception ex)
            {
                writeLogToFile("AddParameter", String.Format("{0} - Attempted Value {1} : {2}", ex.Message, ParamName, ParamValue));
                throw;
            }
        }

        public string GetQueryString()
        {
            return this.ConnectionString;
        }

        public void ClearParams()
        {
            ParamCollection.Clear();
        }

        public void writeLogToFile(string Modulename, string msg)
        {
            string dPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string dMsg = "[" + DateTime.Now.ToString() + "] - " + Modulename + " | " + msg;
            string filename = dPath + DateTime.Now.Date.ToString("yyyyMMdd") + ".txt";
            StreamWriter oStream = new StreamWriter(filename, true);
            oStream.WriteLine(dMsg);
            oStream.Flush();
            oStream.Close();
        }

    }
}
