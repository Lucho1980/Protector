using System;
using System.Runtime.Serialization;

namespace UsuarioProBusiness
{
    /// <summary>
    /// Clase base de excepción de error de capa de negocio.
    /// También puede ser utilizada para lanzar una excepción genérica.
    /// </summary>
    public class BoException : Exception
    {
        public BoException(string mensaje) : base(mensaje)
        {
        }

        public BoException(Exception ex) : base("Se produjo un error en la aplicación.", ex)
        {
        }
    }
}
