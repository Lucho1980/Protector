using System;
using System.Data;
using System.Data.Odbc;
using DataObject;
using System.Configuration;


namespace ODBC
{
    internal class daConexion
    {
        public IDbConnection GetOpenedConnection()
        {
            OdbcConnection connection = new OdbcConnection(ConfigurationManager.AppSettings["Conexion"]);
            connection.Open();

            return connection;
        }
    }
}
