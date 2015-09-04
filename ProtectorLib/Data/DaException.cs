using System;
using System.Collections.Generic;
using System.Text;

namespace UsuarioProData
{
    /// <summary>
    /// Clase base de excepción de error de base de datos.
    /// </summary>
    public class DaException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ex">Excepción original</param>
        public DaException(Exception ex) : base("Se produjo un error en la base de datos.", ex)
        {
        }
    }
}
