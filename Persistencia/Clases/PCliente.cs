using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using EC;

namespace Persistencia
{
    internal class PCliente : IPCliente
    {
        private static PCliente instancia = null;
        private PCliente() { }
        public static PCliente GetInstancia()
        {
            if (instancia == null)
                instancia = new PCliente();
            return instancia;
        }
        public void AgregarCliente(Cliente pCliente, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("AltaCliente", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@cliCi", pCliente.CliCi);
            comando.Parameters.AddWithValue("@nombre", pCliente.CliNom);
            comando.Parameters.AddWithValue("@clinumTar", pCliente.CliNumTar);
            comando.Parameters.AddWithValue("@telefono", pCliente.CliTel);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("Ya existe el cliente");
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
        public void ModificarCliente(Cliente pCliente, Empleado empLogueado)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ModificarCliente", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@cliCi", pCliente.CliCi);
            comando.Parameters.AddWithValue("@nombre", pCliente.CliNom);
            comando.Parameters.AddWithValue("@clinumTar", pCliente.CliNumTar);
            comando.Parameters.AddWithValue("@telefono", pCliente.CliTel);

            SqlParameter parametroR = new SqlParameter("@retorno", SqlDbType.Int);
            parametroR.Direction = ParameterDirection.ReturnValue;
            comando.Parameters.Add(parametroR);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

                if ((int)parametroR.Value == -1)
                    throw new Exception("No existe el cliente");
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
        public Cliente BuscarCliente(string pCliente, Empleado empLogueado)
        {
            Cliente unCli = null;

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ConsultaCliente", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@cliCi", pCliente);

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                

                if (lector.HasRows)
                {
                    lector.Read();
                    unCli = new Cliente(pCliente, (string)lector["CliNom"], (string)lector["CliNumTar"], (string)lector["CliTel"]);
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
            return unCli;
        }
        public List<Cliente> Listar(Empleado empLogueado)
        {
            Cliente unCli = null;
            List<Cliente> lista = new List<Cliente>();

            SqlConnection conexion = new SqlConnection(Conexion.Cnn(empLogueado));
            SqlCommand comando = new SqlCommand("ListadoClientes", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            try
            {
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();
                if (lector.HasRows)
                {
                    while(lector.Read())
                    {
                        unCli = new Cliente((string)lector["CliCi"], (string)lector["CliNom"], (string)lector["CliNumTar"], (string)lector["CliTel"]);
                        lista.Add(unCli);
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
