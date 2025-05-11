using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;

namespace Logica.Interfaces
{
    public interface ILEstado
    {
        Estado BuscarEstado(int pEstado, Empleado empLogueado);
        List<Estado> Listar(Empleado empLogueado);
    }
}
