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
    internal class PDetalleVenta
    {
        internal void AgregarDetVenta(int pVen, DetalleVenta pDetalleVenta, SqlTransaction pTransaccion)
        {
            SqlCommand comando = new SqlCommand("AltaDetalleVenta", pTransaccion.Connection);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@DetVenNum", pVen);
            comando.Parameters.AddWithValue("@DetVenArtCod", pDetalleVenta.DetVenArtCod.ArtCod);
            comando.Parameters.AddWithValue("@DetVenCant", pDetalleVenta.DetVenCant);

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
                    throw new Exception("Ya existe el Detalle de Venta");
                else if ((int)parametroR.Value == -3)
                    throw new Exception("No existe el Articulo");
                else if ((int)parametroR.Value == -4)
                    throw new Exception("Ocurrio un error inesperado");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal List<DetalleVenta> Listar(int pVenNum, Empleado empLogueado)
        {
            DetalleVenta unDVen ;
            List<DetalleVenta> lista = new List<DetalleVenta>();

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("DetalleDeVenta", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@VenNum", pVenNum);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        unDVen = new DetalleVenta((int)lector["DetVenCant"], PArticulo.GetInstancia().BuscarTodosLosArticulos((string)lector["DetVenArtCod"], empLogueado));
                        lista.Add(unDVen);
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
