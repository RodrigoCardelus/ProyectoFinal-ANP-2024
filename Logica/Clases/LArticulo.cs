using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    internal class LArticulo : Interfaces.ILArticulo
    {
        private static LArticulo instancia = null;
        private LArticulo() { }
        public static LArticulo ObtenerInstancia()
        {
            if (instancia == null)
                instancia = new LArticulo();
            return instancia;
        }
        public void AgregarArticulo(Articulo pArticulo, Empleado empLogueado)
        {
            if (pArticulo.ArtFechVen < DateTime.Now)
                throw new Exception("La fecha de vencimiento debe ser mayor a la actual");
            IPArticulo art = FabricaPersistencia.ObtenerPerArticulo();
            art.AgregarArticulo(pArticulo,empLogueado);
        }
        public void ModificarArticulo(Articulo pArticulo, Empleado empLogueado)
        {
            if (pArticulo.ArtFechVen < DateTime.Now)
                throw new Exception("La fecha de vencimiento debe ser mayor a la actual");
            IPArticulo art = FabricaPersistencia.ObtenerPerArticulo();
            art.ModificarArticulo(pArticulo, empLogueado);
        }
        public void EliminarArticulo(Articulo pArticulo, Empleado empLogueado)
        {
            IPArticulo art = FabricaPersistencia.ObtenerPerArticulo();
            art.EliminarArticulo(pArticulo, empLogueado);
        }
        public Articulo BuscarArticulo(string pArticulo, Empleado empLogueado)
        {
            IPArticulo art = FabricaPersistencia.ObtenerPerArticulo();
            return art.BuscarArticulo(pArticulo,  empLogueado);
        }
        public List<Articulo> Listar(Empleado empLogueado)
        {
            IPArticulo art = FabricaPersistencia.ObtenerPerArticulo();
            return art.Listar(empLogueado);
        }
    }
}
