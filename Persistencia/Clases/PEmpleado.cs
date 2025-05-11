using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using EC;

namespace Persistencia
{
    internal class PEmpleado : IPEmpleado
    {
        private static PEmpleado instancia = null;
        private PEmpleado() { }
        public static PEmpleado GetInstancia()
        {
            if (instancia == null)
                instancia = new PEmpleado();
            return instancia;
        }
        public Empleado BuscarEmpleado(string pEmp, Empleado empLogueado)
        {
            Empleado emp = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("BuscarEmpleado", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@empleado", pEmp);

            try
            {
                conexion.Open();
                SqlDataReader reader = comando.ExecuteReader();
                
                if (reader.HasRows)
                {
                    reader.Read();
                    emp = new Empleado(pEmp, (string)reader["EmpNom"], (string)reader["EmpPass"]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return emp;
        }
        public Empleado Logueo(string usuL,string usuP)
        {
            Empleado emp = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn());
            SqlCommand comando = new SqlCommand("LogueoEmpleado", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@usuario", usuL);
            comando.Parameters.AddWithValue("@contraseña", usuP);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();

                if (lector.HasRows)
                {
                    lector.Read();
                    emp = new Empleado((string)lector["EmpUsu"], (string)lector["EmpNom"], (string)lector["EmpPass"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return emp;
        }
    }
}
