using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;

namespace Persistencia
{
    public interface IPEmpleado
    {
        Empleado Logueo(string usuL, string usuP);
        Empleado BuscarEmpleado(string pEmp, Empleado empLogueado);
    }
}
