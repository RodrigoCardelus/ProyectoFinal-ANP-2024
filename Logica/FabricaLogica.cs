using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class FabricaLogica
    {
        public static Interfaces.ILArticulo GetLArticulo()
        {
            return (LArticulo.ObtenerInstancia());
        }
        public static Interfaces.ILCategoria GetLCategoria()
        {
            return (LCategoria.ObtenerInstancia());
        }
        public static Interfaces.ILCliente GetLCliente()
        {
            return (LCliente.GetInstancia());
        }
        public static Interfaces.ILEstado GetLEstado()
        {
            return (Clases.LEstado.ObtenerInstancia());
        }
        public static Interfaces.ILEmpleado GetLEmpleado()
        {
            return (LEmpleado.GetInstancia());
        }
        public static Interfaces.ILVenta GetLVenta()
        {
            return (LVenta.GetInstancia());
        }
    }
}
