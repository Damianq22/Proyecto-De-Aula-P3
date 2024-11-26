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

namespace Datos
{
    public class DatosProveedor : BaseDatos
    {
        
        public void Add(Proveedor proveedor)
        {

            string registro = "INSERT INTO PROVEEDOR (nombre, telefono) VALUES (@nombre, @telefono)";

            try
            {
                using (SqlCommand command = new SqlCommand(registro, conexion))
                {
                    command.Parameters.AddWithValue("@nombre", proveedor.nombre);
                    command.Parameters.AddWithValue("@telefono", proveedor.telefono);

                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }


        public List<Proveedor> Leer()
        {
            List<Proveedor> proveedorList = new List<Proveedor>();
            string Consulta = "SELECT * FROM PROVEEDOR";
            try
            {
                SqlCommand command = new SqlCommand(Consulta, conexion);
                AbrirConexion();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    proveedorList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception)
            {
                return null;
            }
            return proveedorList;
        }

        public string Eliminar(int idProveedor)
        {
            string eliminar = "DELETE FROM PROVEEDOR WHERE id = @id";

            try
            {
                using (SqlCommand command = new SqlCommand(eliminar, conexion))
                {
                    command.Parameters.AddWithValue("@id", idProveedor);

                    AbrirConexion();
                    int rowsAffected = command.ExecuteNonQuery();
                    CerrarConexion();

                    if(rowsAffected > 0)
                    {
                        return "Proveedor eliminado con exito";
                    }
                    return "Error al eliminar el proveedor";
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

        public Proveedor Buscar(int IdProveedor)
        {
            try
            {
                var proveedores = Leer();
                if (proveedores == null)
                {
                    return null;
                }
                return proveedores.FirstOrDefault(p => p.id == IdProveedor);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Actualizar(Proveedor proveedorNew)
        {
            string Actualizar = "Update Proveedor set nombre=@nombre, telefono=@telefono where id=@id";
            try
            {
                SqlCommand command = new SqlCommand(Actualizar, conexion);
                command.Parameters.AddWithValue("@id", proveedorNew.id);
                command.Parameters.AddWithValue("@nombre", proveedorNew.nombre);
                command.Parameters.AddWithValue("@telefono", proveedorNew.telefono);
                AbrirConexion();
                var rowsAffected = command.ExecuteNonQuery();
                CerrarConexion();
                if (rowsAffected > 0)
                {
                    return "Proveedor actualizado con exito";
                }
                return "Error al actualizar";
            }
            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
            finally
            {
                CerrarConexion();
            }
        }

        private Proveedor Map(SqlDataReader reader)
        {
            Proveedor proveedor = new Proveedor
            {
                id = Convert.ToInt32(reader["id"]),
                nombre = Convert.ToString(reader["nombre"]),
                telefono = Convert.ToString(reader["telefono"])
            };

            return proveedor;
        }
    }
}
