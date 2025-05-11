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
    internal class PVenta : IPVenta
    {
        private static PVenta instancia = null;
        private PVenta() { }
        public static PVenta GetInstancia()
        {
            if (instancia == null)
                instancia = new PVenta();
            return instancia;
        }
        public void AgregarVenta(Venta pVenta, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("AltaVenta", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@VenMon", pVenta.VenMon);
            comando.Parameters.AddWithValue("@VenDir", pVenta.VenDir);
            comando.Parameters.AddWithValue("@VenCliCI", pVenta.VenCliCi.CliCi);
            comando.Parameters.AddWithValue("@VenEmpUsu", pVenta.VenEmpUsu.EmpUsu);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            SqlParameter parametroNumVen = new SqlParameter("@NumVen", SqlDbType.Int);
            parametroNumVen.Direction = ParameterDirection.Output;
            comando.Parameters.Add(parametroNumVen);


            int oAfectado = -1;
            SqlTransaction trn = null;

            try
            {
                conexion.Open();
                trn = conexion.BeginTransaction();

                comando.Transaction = trn;

                comando.ExecuteNonQuery();

                
                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe el Cliente");
                else if ((int)parametroR.Value == -2)
                    throw new Exception("No existe el Empleado");
                else if ((int)parametroR.Value == -3)
                    throw new Exception("Ocurrio un error inesperado");

                oAfectado = (int)comando.Parameters["@NumVen"].Value;

                foreach (DetalleVenta detalle in pVenta.ListaDetVen)
                {
                    new PDetalleVenta().AgregarDetVenta(oAfectado, detalle, trn);
                }

                PHistoricoEstado.AltaEstadoVenta(oAfectado, trn);

                trn.Commit();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }
        public void CambiarEstadoVen(Venta pVenta, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("CambiarEstado", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@VenNum", pVenta.VenNum);
            comando.Parameters.AddWithValue("@EstCod", pVenta.ListaHisEst.Last().VenEst.EstCod);  // como saco el estado?, trayendo este listado me habilita el ultimo estado con el .Last?

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe el Estado");
                else if ((int)parametroR.Value == -2)
                    throw new Exception("No existe la Venta");
                else if ((int)parametroR.Value == -3)
                    throw new Exception("El Estado no puede ser menor al actual");
                else if ((int)parametroR.Value == -4)
                    throw new Exception("El Estado no puede ser mayor al siguiente del actual");
                else if ((int)parametroR.Value == -5)
                    throw new Exception("Ocurrio un error inesperado");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }
        public List<Venta> Listar(Empleado empLogueado)
        {
            Venta unaVen = null;
            List<Venta> lista = new List<Venta>();


            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ListadoVentas", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        List<DetalleVenta> detUnaVenta = new PDetalleVenta().Listar((int)lector["VenNum"],empLogueado);
                        List<HistoricoEstado> HistUnaVenta = new PHistoricoEstado().Listar((int)lector["VenNum"],empLogueado);
                        Cliente unCli = FabricaPersistencia.ObtenerPerCliente().BuscarCliente((string)lector["VenCliCI"], empLogueado);
                        Empleado unEmp = FabricaPersistencia.ObtenerPerEmpleado().BuscarEmpleado((string)lector["VenEmpUsu"], empLogueado);
                        unaVen = new Venta((int)lector["VenNum"], Convert.ToDateTime(lector["VenFec"]), Convert.ToDouble(lector["VenMon"]),Convert.ToString(lector["VenDir"]), unCli, unEmp, detUnaVenta, HistUnaVenta);
                        lista.Add(unaVen);
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
