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
    internal class PEstado : IPEstado
    {
        private static PEstado instancia = null;
        private PEstado() { }
        public static PEstado GetInstancia()
        {
            if (instancia == null)
                instancia = new PEstado();
            return instancia;
        }
        public Estado BuscarEstado(int pEstado, Empleado empLogueado)
        {
            Estado est = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("BuscarEstado", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@estado", pEstado);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
               

                if (lector.HasRows)
                {
                    lector.Read();
                    est = new Estado(pEstado, (string)lector["EstNom"]);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return est;
        }
        public List<Estado> Listar(Empleado empLogueado)
        {
            Estado est = null;
            List<Estado> lista = new List<Estado>();

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ListadoEstado", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        est = new Estado((int)lector["EstCod"], (string)lector["EstNom"]);
                        lista.Add(est);
                    }
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }
    }
}
