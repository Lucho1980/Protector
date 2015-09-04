using System;

namespace ProtectorEntity
{
    /// <summary>
    /// Cada instancia de esta clase representa un usuario.
    /// </summary>
    public class UsuarioPEntity
    {
        private string legajo;
        private string apellido;
        private string nombre;
        private string telefono;
        private string email;
        private Nullable<DateTime> fechaNacimiento;
        private uint sueldo;

        public UsuarioPEntity()
        {
            Legajo = "";
            Apellido = "";
            Nombre = "";
            Telefono = "";
            Sueldo = 0;
            email = null;
            fechaNacimiento = null;
        }

        public string Legajo
        {
            get
            {
                return legajo;
            }
            set
            {
                legajo = value.Trim();
            }
        }

        public string Apellido
        {
            get
            {
                return apellido;
            }
            set
            {
                apellido = value.Trim();
            }
        }

        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value.Trim();
            }
        }

        public string Telefono
        {
            get
            {
                return telefono;
            }
            set
            {
                telefono = value.Trim();
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = (value.Trim() != "" ? value.Trim() : null);
            }
        }

        public DateTime FechaNacimiento
        {
            get
            {
                return fechaNacimiento.Value;
            }
            set
            {
                fechaNacimiento = value;
            }
        }

        public uint Sueldo
        {
            get
            {
                return sueldo;
            }
            set
            {
                sueldo = value;
            }
        }

        public bool TieneEmail()
        {
            return (email != null);
        }

        public void BlanquearEmail()
        {
            email = null;
        }

        public bool TieneFechaNacimiento()
        {
            return (fechaNacimiento != null);
        }

        public void BlanquearFechaNacimiento()
        {
            fechaNacimiento = null;
        }
    }
}
