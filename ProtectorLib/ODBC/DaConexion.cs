using System;
using System.Data;
using System.Data.Odbc;
using UsuarioProData;
using System.Configuration;

namespace UsuarioProODBC
{
    /// <summary>
    /// Esta clase es solo de uso interno en la librería por este
    /// motivo se antepone la palabra reservada "internal".
    /// </summary>
    internal class DaConexion
    {
        /// <summary>
        /// Obtener una conexión abierta a la base de datos utilizando ODBC.
        /// </summary>
        /// <returns>Una conexión abierta a la base de datos.</returns>
        public IDbConnection GetOpenedConnection()
        {
            OdbcConnection connection = new OdbcConnection(ConfigurationManager.AppSettings["Conexion"]);
            connection.Open();

            return connection;
        }
    }
}
