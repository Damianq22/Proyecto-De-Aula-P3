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
    public class DatosArticulos : BaseDatos
    {
        DatosCategoria datosCategoria = new DatosCategoria();
        DatosProveedor datosProveedor = new DatosProveedor();


        public void Add(Articulo articulo)
        {
            string registro = "INSERT INTO ARTICULO (descripcion, precioAlquiler, precioProveedor, cantidad , disponible, reparacion ,proveedorId, categoriaId) VALUES ( @descripcion, @precioAlquiler, @precioProveedor, @cantidad , @disponible, @reparacion ,@proveedorId, @categoriaId)";

            try
            {
                using (SqlCommand command = new SqlCommand(registro, conexion))
                {
                    command.Parameters.AddWithValue("@descripcion", articulo.descripcion);
                    command.Parameters.AddWithValue("@precioAlquiler", articulo.precioAlquiler);
                    command.Parameters.AddWithValue("@precioProveedor", articulo.precioProveedor);
                    command.Parameters.AddWithValue("@Cantidad", articulo.cantidad);
                    command.Parameters.AddWithValue("@disponible", articulo.disponible);
                    command.Parameters.AddWithValue("@reparacion", articulo.reparacion);
                    command.Parameters.AddWithValue("@proveedorId", articulo.proveedor.id);
                    command.Parameters.AddWithValue("@categoriaId", articulo.categoria.id);

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

        public List<Articulo> Leer()
        {
            List<Articulo> productoList = new List<Articulo>();
            string Consulta = "SELECT * FROM Articulo";
            try
            {
                SqlCommand command = new SqlCommand(Consulta, conexion);
                AbrirConexion();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    productoList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception)
            {
                return null;
            }
            return productoList;
        }

        public string Eliminar(int idArticulo)
        {
            string query = "DELETE FROM Articulo WHERE id = @id";

            try
            {
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", idArticulo);

                    AbrirConexion();
                    var rowsAffected = command.ExecuteNonQuery();
                    CerrarConexion();

                    if(rowsAffected > 0)
                    {
                        return "Eliminado con exito";
                    }
                    return "Error al eliminar";
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

        public Articulo Buscar(int idProducto)
        {
            try
            {
                var productos = Leer();
                if (productos == null)
                {
                    return null;
                }
                return productos.FirstOrDefault(p => p.id == idProducto);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Actualizar(Articulo articulo)
        {
            string query = "update Articulo set precioAlquiler=@precioAlquiler, precioProveedor=@precioProveedor, cantidad = @cantidad, disponible=@disponible, reparacion=@reparacion  where id=@id";
            try
            {
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", Convert.ToInt32(articulo.id)); // Asegurarse de que sea un entero
                    command.Parameters.AddWithValue("@precioAlquiler", Convert.ToDecimal(articulo.precioAlquiler)); // Convertir a decimal si es necesario
                    command.Parameters.AddWithValue("@disponible", Convert.ToInt32(articulo.disponible)); // Asegurarse de que sea un entero
                    command.Parameters.AddWithValue("@precioProveedor", Convert.ToDecimal(articulo.precioProveedor)); // Convertir a decimal si es necesario
                    command.Parameters.AddWithValue("@cantidad", Convert.ToInt32(articulo.cantidad)); // Asegurarse de que sea un entero
                    command.Parameters.AddWithValue("@reparacion", Convert.ToInt32(articulo.reparacion)); // Asegurarse de que sea un entero
                    AbrirConexion();
                    var rowsAffected = command.ExecuteNonQuery();
                    CerrarConexion();
                    if(rowsAffected > 0)
                    {
                        return "Se actualizo correctamente";
                    }
                    return "Error al actualizar";
                }
            }
            catch (Exception e)
            {
                return $"Error: {e.Message} ";
                
            }
            finally
            {
                CerrarConexion();
            }
        }


        private Articulo Map(SqlDataReader reader)
        {
            Articulo articulo = new Articulo
            {
                id = Convert.ToInt32(reader["id"]),
                descripcion = Convert.ToString(reader["descripcion"]),
                cantidad = Convert.ToInt32(reader["cantidad"]),
                precioAlquiler = Convert.ToDouble(reader["precioAlquiler"]),
                precioProveedor = Convert.ToDouble(reader["precioProveedor"]),
                disponible = Convert.ToInt32(reader["disponible"]),
                reparacion = Convert.ToInt32(reader["reparacion"])
            };
            int IdCategoria = Convert.ToInt32(reader["categoriaId"]);
            articulo.categoria = ObtenerCategoria(IdCategoria);
            int idProveedor = Convert.ToInt32(reader["proveedorId"]);
            articulo.proveedor = ObtenerProveedor(idProveedor);

            return articulo;
        }

        private Categoria ObtenerCategoria(int IdCategoria)
        {
            return datosCategoria.Buscar(IdCategoria);
            
        }
        private Proveedor ObtenerProveedor(int idProveedor)
        {
            return datosProveedor.Buscar(idProveedor);
        }
    }
}

