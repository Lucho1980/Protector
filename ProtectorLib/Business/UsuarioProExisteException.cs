using System;
using System.Runtime.Serialization;

namespace UsuarioProBusiness
{
    public class UsuarioProExisteException : BoException
    {
        public UsuarioProExisteException(string legajo) : base("Ya existe un empleado con el legajo " + legajo + ".")
        {
        }
    }
}
