using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC;

namespace Persistencia
{
    public interface IPArticulo
    {
        void AgregarArticulo(Articulo pArticulo, Empleado empLogueado);
        void ModificarArticulo(Articulo pArticulo, Empleado empLogueado);
        void EliminarArticulo(Articulo pArticulo, Empleado empLogueado);
        Articulo BuscarArticulo(string pArticulo, Empleado empLogueado);
        List<Articulo> Listar(Empleado empLogueado);
    }
}
