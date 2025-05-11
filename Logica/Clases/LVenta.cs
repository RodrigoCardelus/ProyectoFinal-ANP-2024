using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    internal class LVenta : Interfaces.ILVenta
    {
        private static LVenta instancia = null;
        private LVenta() { }
        public static LVenta GetInstancia()
        {
            if (instancia == null)
                instancia = new LVenta();
            return instancia;
        }
        public void AgregarVenta(Venta pVenta, Empleado empLogueado)
        {
            IPVenta ven = FabricaPersistencia.ObtenerPerVenta();
            ven.AgregarVenta(pVenta, empLogueado);
        }
        public void CambiarEstadoVen(Venta pVenta, Empleado empLogueado)
        {
            IPVenta ven = FabricaPersistencia.ObtenerPerVenta();
            ven.CambiarEstadoVen(pVenta,empLogueado);
        }
        public List<Venta> Listar(Empleado empLogueado)
        {
            IPVenta ven = FabricaPersistencia.ObtenerPerVenta();
            return ven.Listar(empLogueado);
        }

    }
}
