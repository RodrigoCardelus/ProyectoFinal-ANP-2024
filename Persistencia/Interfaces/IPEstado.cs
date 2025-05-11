using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC;

namespace Persistencia
{
    public interface IPEstado
    {
        Estado BuscarEstado(int pEstado, Empleado empLogueado);
        List<Estado> Listar(Empleado empLogueado);
    }
}
