using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using DataObject;
using Entities;

namespace ODBC
{
    public class daProtectors
   {
		/// <summary>
		/// Constantes con sentencias SQL. 
		/// </summary>
		private const string SQLSearchByPrimaryKey = "SELECT * FROM Empleado WHERE EmpleadoLegajo = ?";
		private const string SQLSearch = "SELECT * FROM Empleado WHERE EmpleadoLegajo LIKE ? AND EmpleadoApellido LIKE ? AND EmpleadoNombre LIKE ?";
		private const string SQLInsert = "INSERT INTO Empleado (EmpleadoLegajo, EmpleadoApellido, EmpleadoNombre, EmpleadoTelefono, EmpleadoEmail, EmpleadoFechaNacimiento, EmpleadoSueldo) VALUES (?, ?, ?, ?, ?, ?, ?)";
		private const string SQLUpdate = "UPDATE Empleado SET EmpleadoApellido = ?, EmpleadoNombre = ?, EmpleadoTelefono = ?, EmpleadoEmail = ?, EmpleadoFechaNacimiento = ?, EmpleadoSueldo = ? WHERE EmpleadoLegajo = ?";
		private const string SQLDelete = "DELETE FROM Empleado WHERE EmpleadoLegajo = ?";

        private daConexion connectionDA = new daConexion();

        public daProtectors()
		{
		}

		/// <summary>
		/// Crea una instancia de la clase EmpleadoEntity y la carga con 
		/// los datos contenidos en el objeto DataReader.
		/// </summary>
		/// <param name="dr">Objeto DataReader con los datos del empleado.</param>
		/// <returns>Objeto empleado con los datos cargados.</returns>
		private ProtectorEntity CrearEntidad(OdbcDataReader dr)
		{
            ProtectorEntity entidad = new ProtectorEntity();

            entidad.Legajo = dr["EmpleadoLegajo"].ToString();
			entidad.Apellido = dr["EmpleadoApellido"].ToString();
			entidad.Nombre = dr["EmpleadoNombre"].ToString();
			entidad.Telefono = dr["EmpleadoTelefono"].ToString();
					
			// Si el email no es Null
			if (!dr.IsDBNull(dr.GetOrdinal("EmpleadoEmail")))
			{
				entidad.Email = dr["EmpleadoEmail"].ToString();
			}

			// Si la fecha de nacimiento no es Null
			if (!dr.IsDBNull(dr.GetOrdinal("EmpleadoFechaNacimiento")))
			{
				entidad.FechaNacimiento = Convert.ToDateTime(dr["EmpleadoFechaNacimiento"]);
			}

			entidad.Sueldo = Convert.ToUInt32(dr["EmpleadoSueldo"]);

			return entidad;
		}

		/// <summary>
		/// Crea los parámetros comunes para el Insert y el Update de un empleado.
		/// </summary>
		/// <param name="command">Objeto Command que será ejecutado.</param>
		/// <param name="entidad">Objeto con los datos del empleado.</param>
        private void CrearParametros(OdbcCommand command, ProtectorEntity entidad)
		{
            OdbcParameter parameter = null;
			
			parameter = command.Parameters.Add("?", OdbcType.VarChar);
			parameter.Value = entidad.Apellido;

			parameter = command.Parameters.Add("?", OdbcType.VarChar);
			parameter.Value = entidad.Nombre;

			parameter = command.Parameters.Add("?", OdbcType.VarChar);
			parameter.Value = entidad.Telefono;

			parameter = command.Parameters.Add("?", OdbcType.VarChar);
			
			if (entidad.TieneEmail())
				parameter.Value = entidad.Email;
			else
				parameter.Value = System.DBNull.Value;

			parameter = command.Parameters.Add("?", OdbcType.DateTime);

			if (entidad.TieneFechaNacimiento())
				parameter.Value = entidad.FechaNacimiento;
			else
				parameter.Value = System.DBNull.Value;

			parameter = command.Parameters.Add("?", OdbcType.Int);
			parameter.Value = entidad.Sueldo;
		}

		/// <summary>
		/// Ejecuta una sentencia SQL utilizando un OdbcCommand. 
		/// </summary>
		/// <param name="sqlCommandType">Tipo de acción a realizar. Ver daCommon.</param>
		/// <param name="entidad">Objeto con los datos del empleado.</param>
        private void EjecutarComando(daComun.TipoComandoEnum sqlCommandType, ProtectorEntity entidad)
		{
			// Conexión a la base de datos.
			OdbcConnection connection = null;
			// Comando a ejecutar en la base de datos.
			OdbcCommand command = null;
			
			try
			{
				// Se obtiene una conexión abierta.
                connection = (OdbcConnection)connectionDA.GetOpenedConnection();

				// Se crea el parámetro Legajo y se le asigna el valor.
                IDataParameter paramLegajo = new OdbcParameter("?", OdbcType.VarChar);
				paramLegajo.Value = entidad.Legajo;

				// Dependiendo de la acción que se quiera realizar:
				switch (sqlCommandType)
				{
					case daComun.TipoComandoEnum.Insertar:
						// Se crea el comando con la sentendia Insert,
						// se le agrega el parámetro legajo y luego el resto de
						// los parámetros.
						command = new OdbcCommand(SQLInsert, connection);
						command.Parameters.Add(paramLegajo);
						CrearParametros(command, entidad);
						break;

					case daComun.TipoComandoEnum.Actualizar:
						// Se crea el comando con la sentendia Update,
						// se crean los parámetros comunes y luego se 
						// agrega el parámetro Legajo.
						// Esto se hace en este orden porque si miramos la 
						// sentencia Update, veremos que el parámetro Legajo
						// es el último.
						command = new OdbcCommand(SQLUpdate, connection);
						CrearParametros(command, entidad);
						command.Parameters.Add(paramLegajo);
						break;

					case daComun.TipoComandoEnum.Eliminar:
						// Se crea el comando con la sentendia Delete y
						// se agrega el parámetro Legajo.
						command = new OdbcCommand(SQLDelete, connection);
						command.Parameters.Add(paramLegajo);
						break;
				}
				
				// Se ejecuta el comando en la base de datos.
				command.ExecuteNonQuery();
				// Se cierra la conexión.
				connection.Close();
			}
			catch (Exception ex)
			{
				// En caso de que se produzca un error, se lo lanza hacia la
				// capa superior.
				throw new daException(ex);
			}
			finally
			{
				// Esta parte del código se ejecuta siempre.

				if (command != null)
				{
					// Se libera el recurso.
					command.Dispose();
				}
				
				if (connection != null)
				{
					// Se libera el recurso.
					connection.Dispose();
				}
			}
		}

		/// <summary>
		/// Se busca un empleado por el Legajo.
		/// </summary>
		/// <param name="legajo">Legajo del empleado</param>
		/// <returns>Objeto con los datos del empleado.</returns>
        public ProtectorEntity BuscarPorClavePrimaria(string legajo)
		{
			// Conexión a la base de datos
            OdbcConnection connection = null;
            // Comando a ejecutar en la base de datos.
            OdbcCommand command = null;
            // DataReader con registros de datos.
            OdbcDataReader dr = null;
            // Objeto con datos del empleado.
            ProtectorEntity entidad = null;
			
			try
			{
				// Se obtiene una conexión abierta.
                connection = (OdbcConnection)connectionDA.GetOpenedConnection();
				// Se crea el comando con la sentencia Select.
				command = new OdbcCommand(SQLSearchByPrimaryKey, connection);
				// Se agrega el parámetro Legajo y se le asigna el valor.
				command.Parameters.Add("?", OdbcType.VarChar);
				command.Parameters[0].Value = legajo;
				// Se ejecuta el comando y devuelve un objeto del tipo DataReader
				// con los datos del empleado.
				dr = command.ExecuteReader();

				// Si se puede leer el objeto DataReader.
				if (dr.Read())
				{
					// Se crea el objeto con los datos del empleado.
					entidad = CrearEntidad(dr);
				}

				// Se cierra el DataReader.
				dr.Close();
				// Se cierra la conexión.
				connection.Close();
			}
			catch (Exception ex)
			{
                // En caso de que se produzca un error, se lo lanza hacia la
                // capa superior.
                throw new daException(ex);
            }
			finally
			{
				// Se marca al objeto para que sea liberado de la memoria.
				dr = null;
				
				if (command != null)
				{
					// Se libera el recurso.
					command.Dispose();
				}
				
				if (connection != null)
				{
					// Se libera el recurso.
					connection.Dispose();
				}
			}

			return entidad;
		}

		/// <summary>
		/// Se buscan empleados por distintos criterios.
		/// </summary>
		/// <param name="legajo">Posible legajo.</param>
		/// <param name="apellido">Posible apellido.</param>
		/// <param name="nombre">Posible nombre</param>
		/// <returns>Lista de objetos con datos de los empleados encontrados.</returns>
		public List<EmpleadoEntity> Buscar(string legajo, string apellido, string nombre)
		{
            // Conexión a la base de datos
            OdbcConnection connection = null;
            // Comando a ejecutar en la base de datos.
            OdbcCommand command = null;
            // DataReader con registros de datos.
            OdbcDataReader dr = null;
            // Lista de objetos con datos de empleados.
            List<EmpleadoEntity> empleados = null;
			
			try
			{
                // Se obtiene una conexión abierta.
                connection = (OdbcConnection)connectionDA.GetOpenedConnection();
                // Se crea el comando con la sentencia Select.
				command = new OdbcCommand(SQLSearch, connection);
                // Se agrega el parámetro Legajo.
				command.Parameters.Add("?", OdbcType.VarChar);
				command.Parameters[0].Value = "%" + legajo + "%";
                // Se agrega el parámetro Apellido.
				command.Parameters.Add("?", OdbcType.VarChar);
				command.Parameters[1].Value = "%" + apellido + "%";
                // Se agrega el parámetro Nombre.
				command.Parameters.Add("?", OdbcType.VarChar);
				command.Parameters[2].Value = "%" + nombre + "%";
                // Se ejecuta el comando SQL en la base de datos y se devuelve 
                // una instancia de DataReader con los registros encontrados.
				dr = command.ExecuteReader();

				// Se crea una instancia de la lista de empleados.
                empleados = new List<EmpleadoEntity>();

				// Mientras que se pueda leer el DataReader.
				while (dr.Read())
				{
					// Se agrega un objeto con los datos del empleado a la lista.
                    empleados.Add(CrearEntidad(dr));
				}

				// Se cierra el DataReader.
                dr.Close();
                // Se cierra la conexión.
				connection.Close();
			}
			catch (Exception ex)
			{
                // En caso de que se produzca un error, se lo lanza hacia la
                // capa superior.
                throw new daException(ex);
            }
			finally
			{
				dr = null;
				
				if (command != null)
				{
                    // Se libera el recurso.
                    command.Dispose();
				}
				
				if (connection != null)
				{
                    // Se libera el recurso.s
                    connection.Dispose();
				}
			}

			return empleados;
		}

		/// <summary>
		/// Insertar un empleado.
		/// </summary>
		/// <param name="entidad">Objeto con los datos del empleado.</param>
		public void Insertar(EmpleadoEntity entidad)
		{
			EjecutarComando(daComun.TipoComandoEnum.Insertar, entidad);
		}

		/// <summary>
		/// Modificar un empleado.
		/// </summary>
		/// <param name="entidad">Objeto con los datos del empleado.</param>
		public void Actualizar(EmpleadoEntity entidad)
		{
            EjecutarComando(daComun.TipoComandoEnum.Actualizar, entidad);
		}

		/// <summary>
		/// Borrar un empleado.
		/// </summary>
		/// <param name="legajo">Legajo del empleado.</param>
		public void Eliminar(string legajo)
		{
            EmpleadoEntity entidad = new EmpleadoEntity();
            entidad.Legajo = legajo;

            EjecutarComando(daComun.TipoComandoEnum.Eliminar, entidad);
		}
	}
}
