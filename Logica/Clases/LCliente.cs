using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    internal class LCliente : Interfaces.ILCliente
    {
        private static LCliente instancia = null;
        private LCliente() { }
        public static LCliente GetInstancia()
        {
            if (instancia == null)
                instancia = new LCliente();
            return instancia;
        }
        public void AgregarCliente(Cliente pCliente, Empleado empLogueado)
        {
            IPCliente cli = FabricaPersistencia.ObtenerPerCliente();
            cli.AgregarCliente(pCliente ,empLogueado);
        }
        public void ModificarCliente(Cliente pCliente, Empleado empLogueado)
        {
            IPCliente cli = FabricaPersistencia.ObtenerPerCliente();
            cli.ModificarCliente(pCliente ,empLogueado);
        }
        public Cliente BuscarCliente(string pCliente, Empleado empLogueado)
        {
            IPCliente cli = FabricaPersistencia.ObtenerPerCliente();
            return cli.BuscarCliente(pCliente, empLogueado);
        }
        public List<Cliente> Listar(Empleado empLogueado)
        {
            IPCliente cli = FabricaPersistencia.ObtenerPerCliente();
            return cli.Listar(empLogueado);
        }
    }
}
