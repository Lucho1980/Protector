using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject
{

    public class daException : Exception
    {

        public daException(Exception ex)
            : base("Se produjo un error en la base de datos.", ex)
        {
        }
    }
}
