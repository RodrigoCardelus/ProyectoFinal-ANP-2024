using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class FabricaPersistencia
    {
        public static IPArticulo ObtenerPerArticulo()
        {
            return (PArticulo.GetInstancia());
        }
        public static IPCategorias ObtenerPerCategoria()
        {
            return (PCategoria.GetInstancia());
        }
        public static IPCliente ObtenerPerCliente()
        {
            return (PCliente.GetInstancia());
        }
        public static IPEstado ObtenerPerEstado()
        {
            return (PEstado.GetInstancia());
        }
        public static IPEmpleado ObtenerPerEmpleado()
        {
            return (PEmpleado.GetInstancia());
        }
        public static IPVenta ObtenerPerVenta()
        {
            return (PVenta.GetInstancia());
        }
    }
}
