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
    internal class PHistoricoEstado
    {
        internal static void AltaEstadoVenta(int pVenta, SqlTransaction pTransaccion)
        {
            SqlCommand comando = new SqlCommand("AltaEstadoVenta", pTransaccion.Connection);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@VenNum", pVenta);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                comando.Transaction = pTransaccion;
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe la Venta");
                else if ((int)parametroR.Value == -2)
                    throw new Exception("Ocurrio un error inesperado");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal List<HistoricoEstado> Listar(int pVen, Empleado empLogueado)
        {
            HistoricoEstado unaV;
            List<HistoricoEstado> lista = new List<HistoricoEstado>();
            SqlDataReader reader;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("DetalleEstadoxVenta", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@VenNum", pVen);

            try
            {
                conexion.Open();
                reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        unaV = new HistoricoEstado((DateTime)reader["fecha"], FabricaPersistencia.ObtenerPerEstado().BuscarEstado((int)reader["VenEst"],empLogueado));
                        lista.Add(unaV);
                    }
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
            return lista;
        }
    }
}
