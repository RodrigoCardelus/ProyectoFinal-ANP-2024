using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC;

namespace Persistencia
{
    public interface IPCliente
    {
        void AgregarCliente(Cliente unCli, Empleado empLogueado);
        void ModificarCliente(Cliente unCli, Empleado empLogueado);
        Cliente BuscarCliente(string pCliente, Empleado empLogueado);
        List<Cliente> Listar(Empleado empLogueado);
    }
}
