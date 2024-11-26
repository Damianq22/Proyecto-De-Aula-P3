using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatosUsuario : BaseDatos
    {

        public void Add(Usuario usuario)
        {
            string registro = "INSERT INTO USUARIO (usuario, contraseña, nombre) VALUES (@usuario, @contraseña, @nombre)";

            try
            {
                using (SqlCommand command = new SqlCommand(registro, conexion))
                {
                    command.Parameters.AddWithValue("@usuario", usuario.usuario);
                    command.Parameters.AddWithValue("@contraseña", usuario.contraseña);
                    command.Parameters.AddWithValue("@nombre", usuario.nombre);

                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                CerrarConexion();
            }
        }
        public List<Usuario> Leer()
        {
            List<Usuario> usuarios = new List<Usuario>();
            string Consulta = "SELECT * FROM USUARIO";
            try
            {
                SqlCommand command = new SqlCommand(Consulta, conexion);
                AbrirConexion();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception)
            {
                return null;
            }
            return usuarios;
        }
        private Usuario Map(SqlDataReader reader)
        {
            Usuario usuario = new Usuario
            {
                Id = Convert.ToInt32(reader["id"]),
                nombre = Convert.ToString(reader["nombre"]),
                contraseña = Convert.ToString(reader["contraseña"]),
                usuario = Convert.ToString(reader["usuario"])
            };


            return usuario;
        }
        public Usuario Buscar(int id)
        {
            try
            {
                var usuarios = Leer();
                if (usuarios == null)
                {
                    return null;
                }
                return usuarios.FirstOrDefault(p => p.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Actualizar(Usuario usuarioNuevo)
        {
            string query = "update USUARIO set usuario=@usuario, contraseña=@contraseña where id=@id";
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@usuario", usuarioNuevo.usuario);
                        command.Parameters.AddWithValue("@descripcion", usuarioNuevo.contraseña);


                        AbrirConexion();
                        var rowsAffected = command.ExecuteNonQuery();
                        CerrarConexion();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    CerrarConexion();
                }
            }
        }

            public bool Eliminar(int id)
            {
                string query = "DELETE FROM CLIENTE WHERE Id = @Id";
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@Id", id);

                            AbrirConexion();
                            var rowsAffected = command.ExecuteNonQuery();
                            CerrarConexion();

                            return rowsAffected > 0;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    finally
                    {
                        CerrarConexion();
                    }
                }
            }
        }
    }

