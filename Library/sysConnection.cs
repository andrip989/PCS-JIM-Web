using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Npgsql;

namespace PCS_JIM_Web.Library
{
    public class sysConnection
    {
        NpgsqlConnection dbconnection;
        public string errormessage;
        public string dbstring;
        public sysConnection() : this("DBConnectionString")
        {
        }

        public sysConnection(string _ConfigConectionString)
        {
            dbstring = ConfigurationManager.AppSettings[_ConfigConectionString];
            dbconnection = new NpgsqlConnection(ConfigurationManager.AppSettings[_ConfigConectionString]);
        }

        public void openConnection()
        {
            if (dbconnection.State == ConnectionState.Closed)
                dbconnection.Open();
        }

        public NpgsqlDataReader executeQuery(sysSQLParam sysParam)
        {
            NpgsqlCommand dbCommand;
            ;
            try
            {
                if (dbconnection.State == ConnectionState.Closed)
                    this.openConnection();

                if (dbconnection.State == ConnectionState.Open)
                {
                    dbCommand = dbconnection.CreateCommand();
                    dbCommand.CommandText = sysParam.SQLQuery;

                    if (sysParam.SQLParam != null)
                    {
                        foreach (SqlParameter param in sysParam.SQLParam)
                        {
                            if (param != null)
                            {
                                dbCommand.Parameters.AddWithValue(param.ParameterName, param.Value);
                            }
                        }
                    }
                    dbCommand.Prepare();
                    
                    return dbCommand.ExecuteReader();
                }
            }
            catch (SqlException sqlex)
            {
                errormessage = sqlex.Message;
                this.closeConnection();
            }
            return null;
        }

        public int executeNonQuery(sysSQLParam sysParam)
        {
            NpgsqlCommand dbCommand;
            ;
            try
            {                
                if (dbconnection.State == ConnectionState.Closed)
                    this.openConnection();

                if (dbconnection.State == ConnectionState.Open)
                {
                    dbCommand = dbconnection.CreateCommand();
                    dbCommand.CommandText = sysParam.SQLQuery;

                    if (sysParam.SQLParam != null)
                    {
                        foreach (SqlParameter param in sysParam.SQLParam)
                        {
                            if (param != null)
                            {
                                dbCommand.Parameters.AddWithValue(param.ParameterName, param.Value);
                            }
                        }
                    }
                    dbCommand.Prepare();

                    return dbCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlex)
            {
                errormessage = sqlex.Message;
                this.closeConnection();
            }
            return 0;
        }

        public object executeScalar(sysSQLParam sysParam)
        {
            NpgsqlCommand dbCommand;
            ;
            try
            {
                if (dbconnection.State == ConnectionState.Closed)
                    this.openConnection();

                if (dbconnection.State == ConnectionState.Open)
                {
                    dbCommand = dbconnection.CreateCommand();
                    dbCommand.CommandText = sysParam.SQLQuery;

                    if (sysParam.SQLParam != null)
                    {
                        foreach (SqlParameter param in sysParam.SQLParam)
                        {
                            if (param != null)
                            {
                                dbCommand.Parameters.AddWithValue(param.ParameterName, param.Value);
                            }
                        }
                    }

                    dbCommand.Prepare();

                    return dbCommand.ExecuteScalar();
                }
            }
            catch
            {
                this.closeConnection();
                throw new Exception();
            }
            return 0;
        }

        public object executeNonQuery(string sqlSintak)
        {
            NpgsqlCommand dbCommand;
            ;
            try
            {
                if (dbconnection.State == ConnectionState.Closed)
                    this.openConnection();

                if (dbconnection.State == ConnectionState.Open)
                {
                    dbCommand = dbconnection.CreateCommand();
                    dbCommand.CommandText = sqlSintak;

                    dbCommand.Prepare();

                    return dbCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlex)
            {
                errormessage = sqlex.Message;
                this.closeConnection();
            }
            return 0;
        }

        public DataTable getdataTable(string sqlsyntax)
        {
            DataTable dataTable = new DataTable();

            if (dbconnection.State == ConnectionState.Closed)
                this.openConnection();

            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlsyntax, dbconnection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    // Load data into the DataTable
                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }

        public void closeConnection()
        {
            //if (dbconnection.State == ConnectionState.Open)
                dbconnection.Close();                        
        }

        public NpgsqlConnection Connection
        {
            get
            {
                return dbconnection;
            }
        }


    }
}