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
    internal class PCategoria : IPCategorias
    {
        private static PCategoria instancia = null;
        private PCategoria() { }
        public static PCategoria GetInstancia()
        {
            if (instancia == null)
                instancia = new PCategoria();
            return instancia;
        }
        public void AgregarCategoria(Categoria pCategoria, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("AltaCategoria", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@catCod", pCategoria.CatCod);
            comando.Parameters.AddWithValue("@catNom", pCategoria.CatNom);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("Ya existe la categoria");
                else if ((int)parametroR.Value == -2)
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
        public void ModificarCategoria(Categoria pCategoria, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ModificarCategoria", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@catCod", pCategoria.CatCod);
            comando.Parameters.AddWithValue("@catNom", pCategoria.CatNom);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe la categoria");
                else if ((int)parametroR.Value == -2)
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
        public void EliminarCategoria(Categoria pCategoria, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("BajaCategoria", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@catCod", pCategoria.CatCod);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe la categoria");
                else if ((int)parametroR.Value == -2)
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
        public Categoria BuscarCategoria(string pCategoria, Empleado empLogueado)
        {
            Categoria unaCat = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ConsultaCategoria", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@catCod", pCategoria);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                

                if (lector.HasRows)
                {
                    lector.Read();
                    unaCat = new Categoria(pCategoria, (string)lector["CatNom"]);
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
            return unaCat;
        }
        public List<Categoria> Listar(Empleado empLogueado)
        {
            Categoria unaCat = null;
            List<Categoria> lista = new List<Categoria>();

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ListadoCategorias", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        unaCat = new Categoria((string)lector["CatCod"], (string)lector["CatNom"]);
                        lista.Add(unaCat);
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
        internal Categoria BuscarTodasLasCategorias(string pCategoria, Empleado empLogueado)
        {
            Categoria unaCat = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ConsultaTodasCategorias", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@catCod", pCategoria);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
               

                if (lector.HasRows)
                {
                    lector.Read();
                    unaCat = new Categoria(pCategoria, (string)lector["CatNom"]);
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
            return unaCat;
        }
    }
}
