using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;

namespace Logica.Interfaces
{
    public interface ILCliente
    {
        void AgregarCliente(Cliente pCliente, Empleado empLogueado);
        void ModificarCliente(Cliente pCliente, Empleado empLogueado);
        Cliente BuscarCliente(string pCliente, Empleado empLogueado);
        List<Cliente> Listar(Empleado empLogueado);
    }
}
