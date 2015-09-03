using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UsuarioProData;
using UsuarioProODBC;
using UsuarioProEntity;

namespace UsuarioProBusiness
{
    /// <summary>
    /// Clase de negocio de usuarioPro.
    /// </summary>
    public class BoUsuarioPro
    {
        private DaUsuarioPro usuarioPro;

        public BoUsuarioPro()
        {
            usuarioPro = new DaUsuarioPro();
        }

        /// <summary>
        /// Valida que los datos cargados por el usuario sean válidos según
        /// las reglas de negocio establecidas y lanza la excepción correspondiente.
        /// </summary>
        /// <param name="entity">Objeto con los datos del usuario.</param>
        private void Validar(UsuarioPEntity entidad)
        {
            if (entidad.Legajo == "" ||
                entidad.Apellido == "" ||
                entidad.Nombre == "" ||
                entidad.Telefono == "")
            {
                throw new DatosObligatoriosException();
            }

            if (entidad.TieneFechaNacimiento() && DateTime.Now.CompareTo(entidad.FechaNacimiento) < 0)
                throw new FechaIncorrectaException();

            if (entidad.Sueldo < 20 || entidad.Sueldo > 100)
                throw new SueldoFueraDeRangoException(20, 100);
        }

        /// <summary>
        /// Busca los usuarioPro por diferentes criterios.
        /// </summary>
        /// <param name="legajo">Posible legajo.</param>
        /// <param name="apellido">Posible apellido.</param>
        /// <param name="nombre">Posible nombre.</param>
        /// <returns>Retorna una lista de objetos con datos de los usuarios encontrados.</returns>
        public List<UsuarioPEntity> Buscar(string legajo, string apellido, string nombre)
        {
            try
            {
                return usuarioPro.Buscar(legajo, apellido, nombre);
            }
            catch (DaException ex)
            {
                throw new BoException(ex);
            }
        }

        /// <summary>
        /// Busca un empleado por el legajo.
        /// </summary>
        /// <param name="legajo">Legajo del empleado.</param>
        /// <returns>Objeto con los datos del usuario.</returns>
        public UsuarioPEntity BuscarPorLegajo(string legajo)
        {
            try
            {
                // Busca el usuario utilizando la capa de datos.
                return usuarioPro.BuscarPorClavePrimaria(legajo);
            }
            catch (DaException ex)
            {
                throw new BoException(ex);
            }
        }

        /// <summary>
        /// Crea un empleado.
        /// </summary>
        /// <param name="entity">Objeto con los datos del usuario.</param>
        public void Insertar(UsuarioPEntity entidad)
        {
            try
            {
                // Valida los datos cargados por el usuario.
                Validar(entidad);

                // Si el usuario existe en la base de datos...
                if (usuarioPro.BuscarPorClavePrimaria(entidad.Legajo) != null)
                {
                    // ...se lanza la excepción correspondiente.
                    throw new UsuarioProExisteException(entidad.Legajo);
                }

                // Si no existe el empleado, se crea.
                usuarioPro.Insertar(entidad);
            }
            catch (DaException ex)
            {
                throw new BoException(ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de un usuario.
        /// </summary>
        /// <param name="entity"></param>
        public void Actualizar(UsuarioPEntity entidad)
        {
            try
            {
                // Valida los datos cargados por el usuario.
                Validar(entidad);

                // Si el empleado no existe en la base de datos...
                if (usuarioPro.BuscarPorClavePrimaria(entidad.Legajo) == null)
                {
                    // ...se lanza la excepción correspondiente.
                    throw new UsuarioProNoExisteException(entidad.Legajo);
                }

                // Si existe, se actualizan los datos.
                usuarioPro.Actualizar(entidad);
            }
            catch (DaException ex)
            {
                throw new BoException(ex);
            }
        }

        /// <summary>
        /// Se borra un usuario.
        /// </summary>
        /// <param name="legajo">Legajo del usuario.</param>
        public void Eliminar(string legajo)
        {
            try
            {
                // Se borra el usuario.
                usuarioPro.Eliminar(legajo);
            }
            catch (DaException ex)
            {
                throw new BoException(ex);
            }
        }
    }
}
