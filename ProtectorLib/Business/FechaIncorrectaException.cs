using System;
using System.Runtime.Serialization;

namespace UsuarioProBusiness
{
    public class FechaIncorrectaException : BoException
    {
        public FechaIncorrectaException() : base("La Fecha de Nacimiento debe ser menor a la fecha actual.")
        {
        }
    }
}