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
    internal class PArticulo : IPArticulo
    {
        private static PArticulo instancia = null;
        private PArticulo() { }
        public static PArticulo GetInstancia()
        {
            if (instancia == null)
                instancia = new PArticulo();
            return instancia;
        }
        public void AgregarArticulo(Articulo pArticulo, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("AltaArticulo", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@artCod", pArticulo.ArtCod);
            comando.Parameters.AddWithValue("@artNom", pArticulo.ArtNom);
            comando.Parameters.AddWithValue("@artTipo", pArticulo.ArtTipo);
            comando.Parameters.AddWithValue("@artTam", pArticulo.ArtTam);
            comando.Parameters.AddWithValue("@artFechaVen", pArticulo.ArtFechVen);
            comando.Parameters.AddWithValue("@precio", pArticulo.ArtPre);
            comando.Parameters.AddWithValue("@catCod", pArticulo.ArtCatCod.CatCod);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("Ya existe el Articulo");
                else if ((int)parametroR.Value == -2)
                    throw new Exception("No existe la Categoria");
                else if ((int)parametroR.Value == -3)
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
        public void ModificarArticulo(Articulo pArticulo, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ModificarArticulo", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@artCod", pArticulo.ArtCod);
            comando.Parameters.AddWithValue("@artNom", pArticulo.ArtNom);
            comando.Parameters.AddWithValue("@artTipo", pArticulo.ArtTipo);
            comando.Parameters.AddWithValue("@artTam", pArticulo.ArtTam);
            comando.Parameters.AddWithValue("@artFechaVen", pArticulo.ArtFechVen);
            comando.Parameters.AddWithValue("@precio", pArticulo.ArtPre);
            comando.Parameters.AddWithValue("@catCod", pArticulo.ArtCatCod.CatCod);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe el Articulo");
                else if ((int)parametroR.Value == -2)
                    throw new Exception("No existe la Categoria");
                else if ((int)parametroR.Value == -3)
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
        public void EliminarArticulo(Articulo pArticulo, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("EliminarArticulo", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@artCod", pArticulo.ArtCod);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe el Articulo");
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
        public Articulo BuscarArticulo(string pArticulo, Empleado empLogueado)
        {
            Articulo unArt = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ConsultaArticulo", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@artCod", pArticulo);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                

                if (lector.HasRows)
                {
                    lector.Read();
                    Categoria catArt = PCategoria.GetInstancia().BuscarTodasLasCategorias((string)lector["ArtCatCod"],empLogueado);
                    unArt = new Articulo(pArticulo, (string)lector["ArtNom"], (string)lector["ArtTipo"], (int)lector["ArtTam"], Convert.ToDouble(lector["ArtPre"]), (DateTime)lector["ArtFechVen"], catArt);
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
            return unArt;
        }
        public List<Articulo> Listar(Empleado empLogueado)
        {
            Articulo unArt = null;
            List<Articulo> lista = new List<Articulo>();

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ListadoArticulos", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                if (lector.HasRows)
                {
                    while (lector.Read())
                    {
                        Categoria catArt = PCategoria.GetInstancia().BuscarTodasLasCategorias((string)lector["ArtCatCod"], empLogueado);
                        

                        unArt = new Articulo((string)lector["ArtCod"], (string)lector["ArtNom"], 
                            (string)lector["ArtTipo"], (int)lector["ArtTam"],Convert.ToDouble(lector["ArtPre"]), (DateTime)lector["ArtFechVen"], catArt);
                        lista.Add(unArt);
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
        internal Articulo BuscarTodosLosArticulos(string pArticulo, Empleado empLogueado)
        {
            Articulo unArt = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ConsultaTodosArticulos", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@artCod", pArticulo);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
               

                if (lector.HasRows)
                {
                    lector.Read();
                    Categoria catArt = PCategoria.GetInstancia().BuscarTodasLasCategorias((string)lector["ArtCatCod"], empLogueado);
                    unArt = new Articulo(pArticulo, (string)lector["ArtNom"], (string)lector["ArtTipo"], (int)lector["ArtTam"], Convert.ToDouble(lector["ArtPre"]),(DateTime)lector["ArtFechVen"],  catArt);
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
            return unArt;
        }
    }
}
