using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica.Clases
{
    internal class LEstado : Interfaces.ILEstado
    {
        private static LEstado instancia = null;
        private LEstado() { }
        public static LEstado ObtenerInstancia()
        {
            if (instancia == null)
                instancia = new LEstado();
            return instancia;
        }
        public Estado BuscarEstado(int pEstado, Empleado empLogueado)
        {
            IPEstado est = FabricaPersistencia.ObtenerPerEstado();
            return est.BuscarEstado(pEstado, empLogueado);
        }
        public List<Estado> Listar(Empleado empLogueado)
        {
            IPEstado est = FabricaPersistencia.ObtenerPerEstado();
            return est.Listar(empLogueado);
        }
    }
}
