using System;
using System.Runtime.Serialization;

namespace UsuarioProBusiness
{
    public class SueldoFueraDeRangoException : BoException
    {
        public SueldoFueraDeRangoException(int sueldoMinimo, int sueldoMaximo) : base("El sueldo debe estar entre $" + sueldoMinimo.ToString() + " y $" + sueldoMaximo.ToString() + ".")
        {
        }
    }
}
