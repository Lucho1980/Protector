using System;
using System.Runtime.Serialization;

namespace UsuarioProBusiness
{
    public class UsuarioProNoExisteException : BoException
    {
        public UsuarioProNoExisteException(string legajo) : base("El empleado con legajo " + legajo + " no pudo ser encontrado.")
        {
        }
    }
}
