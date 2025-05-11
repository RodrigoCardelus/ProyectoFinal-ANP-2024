using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC;
using Persistencia;

namespace Logica
{
    internal class LCategoria : Interfaces.ILCategoria
    {
        private static LCategoria instancia = null;
        private LCategoria() { }
        public static LCategoria ObtenerInstancia()
        {
            if (instancia == null)
                instancia = new LCategoria();
            return instancia;
        }
        public void AgregarCategoria(Categoria pCategoria, Empleado empLogueado)
        {
            IPCategorias cat = FabricaPersistencia.ObtenerPerCategoria();
            cat.AgregarCategoria(pCategoria, empLogueado);
        }
        public void ModificarCategoria(Categoria pCategoria, Empleado empLogueado)
        {
            IPCategorias cat = FabricaPersistencia.ObtenerPerCategoria();
            cat.ModificarCategoria(pCategoria, empLogueado);
        }
        public void EliminarCategoria(Categoria pCategoria, Empleado empLogueado)
        {
            IPCategorias cat = FabricaPersistencia.ObtenerPerCategoria();
            cat.EliminarCategoria(pCategoria,  empLogueado);
        }
        public Categoria BuscarCategoria(string pCategoria, Empleado empLogueado)
        {
            IPCategorias cat = FabricaPersistencia.ObtenerPerCategoria();
            return cat.BuscarCategoria(pCategoria,  empLogueado);
        }
        public List<Categoria> Listar(Empleado empLogueado)
        {
            IPCategorias cat = FabricaPersistencia.ObtenerPerCategoria();
            return cat.Listar( empLogueado);
        }
    }
}
