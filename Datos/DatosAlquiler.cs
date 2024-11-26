using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatosAlquiler:BaseDatos
    {
        DatosCliente datosCliente = new DatosCliente();
        DatosUsuario datosUsuario = new DatosUsuario();
        DatosArticulos datosArticulos = new DatosArticulos();
        public void Add(Alquiler alquiler)
        {
            string registroAlquiler = "INSERT INTO Alquiler (fechaEntrega,fechaDevolucion,totalPago,estado, clienteCedula, idUsuario) VALUES " +
                "(@fechaEntrega,@fechaDevolucion,@totalPago,@estado, @clienteCedula, @idUsuario)";
            string registroDetalleAlquiler = "INSERT INTO DetalleAlquiler (alquilerId,articuloId,subtotal,cantidad) VALUES " +
                "(@alquilerId,@articuloId,@subtotal,@cantidad)";

            SqlTransaction accion = null;

            try
            {
                AbrirConexion();
                //Una transacción es una secuencia de operaciones de base de datos que se ejecutan de forma atómica. 
                accion = conexion.BeginTransaction();
                using (SqlCommand command = new SqlCommand(registroAlquiler, conexion, accion))
                {
                    command.Parameters.Add("@fechaEntrega", SqlDbType.DateTime).Value = alquiler.fechaEntrega; // Fecha y hora
                    command.Parameters.Add("@fechaDevolucion", SqlDbType.DateTime).Value = alquiler.fechaDevolucion; // Fecha y hora
                    command.Parameters.Add("@totalPago", SqlDbType.Decimal).Value = alquiler.totalPago; // Decimal para montos
                    command.Parameters.Add("@estado", SqlDbType.VarChar).Value = alquiler.estado; // Cadena de texto (VarChar)
                    command.Parameters.Add("@clienteCedula", SqlDbType.VarChar).Value = alquiler.cliente.Id; // Cadena para la cédula
                    command.Parameters.Add("@idUsuario", SqlDbType.Int).Value = alquiler.usuario.Id; // Entero para el ID de usuario
                    command.ExecuteNonQuery();
                }

                //Recorremos los detalles
                foreach (var detalle in alquiler.detalles)
                {
                    using (SqlCommand command = new SqlCommand(registroDetalleAlquiler, conexion, accion))
                    {
                        command.Parameters.Add("@alquilerId", SqlDbType.Int).Value = alquiler.id; // Entero para alquilerId
                        command.Parameters.Add("@articuloId", SqlDbType.Int).Value = detalle.articulo.id; // Entero para articuloId
                        command.Parameters.Add("@subTotal", SqlDbType.Decimal).Value = detalle.subTotal; // Decimal para subTotal
                        command.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad; // Entero para cantidad
                        command.ExecuteNonQuery();
                    }
                }

                accion.Commit();
            }
            catch (Exception)
            {
                if (accion != null)
                {
                    accion.Rollback();
                }
            }
            finally
            {
                CerrarConexion();
            }

        }
        public List<Alquiler> Leer()
        {
            List<Alquiler> alquilerList = new List<Alquiler>();
            string Consulta = "SELECT * FROM ALQUILER";
            try
            {
                SqlCommand command = new SqlCommand(Consulta, conexion);
                AbrirConexion();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    alquilerList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception)
            {
                return null;
            }
            return alquilerList;
        }
        public Alquiler Buscar(int id)
        {
            try
            {
                var alquileres = Leer();
                if (alquileres == null)
                {
                    return null;
                }
                return alquileres.FirstOrDefault(p => p.id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Actualizar(Alquiler alquilerNuevo)
        {
            string query = "update Alquiler set estado=@estado where id=@id";
            try
            {
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", alquilerNuevo.id);
                    command.Parameters.AddWithValue("@estado", alquilerNuevo.estado);
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
        public List<DetalleAlquiler> ObtenerDetalles(int alquilerId)
        {
            List<DetalleAlquiler> detalles = new List<DetalleAlquiler>();
            int i = 1;
            string consulta = "SELECT * FROM DetalleAlquiler WHERE alquilerId = @alquilerId";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexion.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(consulta, connection))
                    {
                        command.Parameters.AddWithValue("@alquilerId", alquilerId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detalles.Add(MapDetalle(reader,i));
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return detalles;
        }
        public DetalleAlquiler MapDetalle(SqlDataReader reader,int i)
        {
            DetalleAlquiler detalle = new DetalleAlquiler
            {
                id =i ,
                subTotal = Convert.ToDouble(reader["subtotal"]),
                cantidad = Convert.ToInt32(reader["cantidad"]),
            };
            int articuloId = Convert.ToInt32(reader["articuloId"]);
            detalle.articulo = datosArticulos.Buscar(articuloId);
            return detalle;
        }
        public bool Eliminar(int id)
        {
            string query = "DELETE FROM ALQUILER WHERE id = @id";

            try
            {
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", id);

                    AbrirConexion();
                    var rowsAffected = command.ExecuteNonQuery();
                    CerrarConexion();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                CerrarConexion();
            }
        }

        private Alquiler Map(SqlDataReader reader)
        {
            Alquiler alquiler = new Alquiler
            {
                id = Convert.ToInt32(reader["id"]),
                fechaEntrega = Convert.ToDateTime(reader["fechaEntrega"]),
                fechaDevolucion = Convert.ToDateTime(reader["fechaDevolucion"]),
                totalPago = Convert.ToDouble(reader["totalPago"]),
                estado = Convert.ToString(reader["estado"])
            };
            int cedula = Convert.ToInt32(reader["clienteCedula"]);
            int idUsuario = Convert.ToInt32(reader["idUsuario"]);
            alquiler.usuario = ObtenerUsuario(idUsuario);
            alquiler.cliente = ObtenerCliente(cedula);
            alquiler.detalles = ObtenerDetalles(alquiler.id);
            return alquiler;
           
        }
        public Usuario ObtenerUsuario(int id)
        {
            return datosUsuario.Buscar(id);
        }
        public Cliente ObtenerCliente(int cedula)
        {
            return datosCliente.Buscar(cedula);
        }


    }
}

