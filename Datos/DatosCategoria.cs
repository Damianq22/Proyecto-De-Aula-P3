using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace Datos
{
    public class DatosCategoria : BaseDatos
    {
        public void Add(Categoria categoria)
        {
            try
            {
                string registro = "INSERT INTO categoria (descripcion) VALUES (@descripcion)";

                using (SqlCommand command = new SqlCommand(registro, conexion))
                {
                    command.Parameters.AddWithValue("@descripcion", categoria.descripcion);

                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                CerrarConexion();
            }

        }

        public List<Categoria> Leer()
        {
            List<Categoria> categoriaList = new List<Categoria>();
            string consulta = "SELECT * FROM categoria";
            try
            {
                SqlCommand command = new SqlCommand(consulta, conexion);
                AbrirConexion();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categoriaList.Add(Map(reader));
                    }
                }
                CerrarConexion();
            }
            catch (Exception)
            {
                return null;
            }
            return categoriaList;
        }

        public string Eliminar(int id)
        {
            string ssql = "DELETE FROM [categoria] WHERE [id] = @id";

            try
            {
                using (SqlCommand command = new SqlCommand(ssql, ObtenerConexion()))
                {
                    command.Parameters.AddWithValue("@id", id);

                    AbrirConexion();
                    var i = command.ExecuteNonQuery();
                    CerrarConexion();

                    if (i > 0)
                    {
                        return "Categoria eliminada con exito";
                    }
                    return "Error al eliminar la categoria";
                }
            } 
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
            finally
            {
                CerrarConexion();
            }
        }

        public Categoria Buscar(int id)
        {
            string ssql = "Select * FROM [categoria] WHERE [id] = @id";
            try
            {
                using (SqlCommand command = new SqlCommand(ssql, ObtenerConexion()))
                {
                    command.Parameters.AddWithValue("@id", id);

                    AbrirConexion();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            return Map(reader);
                        }
                    }
                    return null; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                CerrarConexion();
            }
        }

public bool Actualizar(Categoria categoriaProductoNew)
        {
            try
            {
                string Actualizar = "ModificarCategoria";
                SqlCommand command = new SqlCommand(Actualizar, conexion);
                command.Parameters.AddWithValue("@IdCategoria", categoriaProductoNew.id);
                command.Parameters.AddWithValue("@TipoCategoria", categoriaProductoNew.descripcion);
                command.CommandType = CommandType.StoredProcedure;
                AbrirConexion();
                var index = command.ExecuteNonQuery();
                CerrarConexion();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private Categoria Map(SqlDataReader reader)
        {
            Categoria categoria = new Categoria
            {
                id = Convert.ToInt32(reader["id"]),
                descripcion = Convert.ToString(reader["descripcion"]),
            };

            return categoria;
        }
    }
}
  

