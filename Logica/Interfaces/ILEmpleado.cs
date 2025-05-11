using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;

namespace Logica.Interfaces
{
    public interface ILEmpleado
    {
        Empleado BuscarEmpleado(string pEmp, Empleado empLogueado);
        Empleado Logueo(string usuL, string usuP);
    }
}
