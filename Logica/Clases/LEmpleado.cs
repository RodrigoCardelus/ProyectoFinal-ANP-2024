using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    internal class LEmpleado : Interfaces.ILEmpleado
    {
        private static LEmpleado instancia = null;
        private LEmpleado() { }
        public static LEmpleado GetInstancia()
        {
            if (instancia == null)
                instancia = new LEmpleado();
            return instancia;
        }
        public Empleado BuscarEmpleado(string pEmp, Empleado empLogueado)
        {
            IPEmpleado emp = FabricaPersistencia.ObtenerPerEmpleado();
            return emp.BuscarEmpleado(pEmp, empLogueado);
        }
        public Empleado Logueo(string usuL, string usuP)
        {
            return FabricaPersistencia.ObtenerPerEmpleado().Logueo(usuL,usuP);
        }
    }
}
