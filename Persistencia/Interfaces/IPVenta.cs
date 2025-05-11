using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC;

namespace Persistencia
{
    public interface IPVenta
    {
        void AgregarVenta(Venta pVenta, Empleado empLogueado);
        void CambiarEstadoVen(Venta pVenta, Empleado empLogueado);
        List<Venta> Listar(Empleado empLogueado);
    }
}
