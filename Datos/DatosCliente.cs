using Entidades;
using Microsoft.Win32;
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
    public class DatosCliente:BaseDatos
    {
        
        public string Add(Cliente cliente)
        {
            string registro = "INSERT INTO Cliente (cedula, nombre, telefono, direccion) VALUES  (@cedula, @nombre, @telefono, @direccion)";

            try
            {
                using (SqlCommand command = new SqlCommand(registro, conexion))
                {
                    command.Parameters.AddWithValue("@cedula", cliente.Id);
                    command.Parameters.AddWithValue("@nombre", cliente.nombre);
                    command.Parameters.AddWithValue("@telefono", cliente.telefono);
                    command.Parameters.AddWithValue("@direccion", cliente.direccion);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                    return "Registro exitoso";
                }
            }
            catch (Exception ex)
            {
                return $"Error {ex.Message}";
            }
            finally
            {
                CerrarConexion();
            }
        }
        
        public List<Cliente> Leer()
        {
            List<Cliente> clienteList = new List<Cliente>();
            string Consulta = "SELECT * FROM Cliente";
            try
            {
                SqlCommand command = new SqlCommand(Consulta, conexion);
                AbrirConexion();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clienteList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception)
            {
                return null;
            }
            return clienteList;
        }
        public Cliente Buscar(int cedula)
        {
            try
            {
                var clientes = Leer();
                if (clientes == null)
                {
                    return null;
                }
                return clientes.FirstOrDefault(p => p.Id == cedula);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Actualizar(Cliente clienteNuevo)
        {
            string query = "update CLIENTE set telefono=@telefono, direccion=@direccion where cedula=@cedula";
            try
            {
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@cedula", clienteNuevo.Id);
                    command.Parameters.AddWithValue("@telefono", clienteNuevo.telefono);
                    command.Parameters.AddWithValue("@direccion", clienteNuevo.direccion);
                    AbrirConexion();
                    var rowsAffected = command.ExecuteNonQuery();
                    CerrarConexion();

                    if(rowsAffected > 0)
                    {
                        return "Cliente actualizado con exito";
                    }
                    return "Error al actualizar";
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

        public string Eliminar(int cedula)
        {
            string query = "DELETE FROM Cliente WHERE cedula = @cedula";

            try
            {
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@cedula", cedula);

                    AbrirConexion();
                    var rowsAffected = command.ExecuteNonQuery();
                    CerrarConexion();

                    if(rowsAffected > 0)
                    {
                        return "Cliente eliminado con exito";
                    }
                    return "Error al momento de eliminar cliente";
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

        private Cliente Map(SqlDataReader reader)
        {
            Cliente cliente = new Cliente
            {
                Id = Convert.ToInt32(reader["cedula"]),
                nombre = Convert.ToString(reader["nombre"]),
                telefono = Convert.ToString(reader["telefono"]), 
                direccion = Convert.ToString(reader["direccion"]) 
            };

            return cliente;
        }
    }
}

