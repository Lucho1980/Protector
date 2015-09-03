using System;

namespace UsuarioProData
{
    /// <summary>
    /// Clase con métodos, propiedades y constantes globales al proyecto 
    /// ControlEmpleadosData.
    /// Esta clase es solo de uso interno en la librería por este
    /// motivo se antepone la palabra reservada "internal".
    /// </summary>
    public class DaComun
    {
        /// <summary>
        /// El constructor se declara privado porque esta clase no debe
        /// ser instanciada.
        /// </summary>
        private DaComun()
        {
        }

        public enum TipoComandoEnum
        {
            Insertar,
            Actualizar,
            Eliminar
        }
    }
}