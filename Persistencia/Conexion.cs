using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC;
namespace Persistencia
{
    internal class Conexion
    {
        internal static string Cnn(Empleado empleado = null)
        {
            if (empleado == null)
                return "Data Source=FABRICIO\\SQL2014; Initial Catalog = ProyectoFinal2024; Integrated Security = true";
            else
                return "Data Source=FABRICIO\\SQL2014; Initial Catalog = ProyectoFinal2024; User=" + empleado.EmpUsu + "; Password='" + empleado.EmpPass + "'";

            //if (empleado == null)
            //    return "Data Source=DESKTOP-QJP4724; Initial Catalog = ProyectoFinal2024; Integrated Security = true";
            //else
            //    return "Data Source=DESKTOP-QJP4724; Initial Catalog = ProyectoFinal2024; User=" + empleado.EmpUsu + "; Password='" + empleado.EmpPass + "'";

            //if (empleado == null)
            //    return "Data Source=LAPTOP-AS0RIFII\\SQL2017; Initial Catalog = ProyectoFinal2024; Integrated Security = true";
            //else
            //    return "Data Source=LAPTOP-AS0RIFII\\SQL2017; Initial Catalog = ProyectoFinal2024; User=" + empleado.EmpUsu + "; Password='" + empleado.EmpPass + "'";
        }
    }
}
