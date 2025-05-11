using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC;

namespace Persistencia
{
    public interface IPCategorias
    {
        void AgregarCategoria(Categoria pCategoria, Empleado empLogueado);
        void ModificarCategoria(Categoria pCategoria, Empleado empLogueado);
        void EliminarCategoria(Categoria pCategoria, Empleado empLogueado);
        Categoria BuscarCategoria(string pCategoria, Empleado empLogueado);
        List<Categoria> Listar(Empleado empLogueado);
    }
}
