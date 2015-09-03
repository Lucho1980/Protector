using System;
using System.Runtime.Serialization;


namespace UsuarioProBusiness
{
    public class DatosObligatoriosException : BoException
    {
        public DatosObligatoriosException() : base("Los campos marcados con (*) son obligatorios.")
        {
        }
    }
}
