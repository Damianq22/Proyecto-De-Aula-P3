using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaAlquiler
    {
        public DatosAlquiler datosArticulos = new DatosAlquiler();
        public LogicaArticulo logicaArticulo = new LogicaArticulo();
        public void Add(Alquiler alquiler)
        {
            datosArticulos.Add(alquiler);
            if (alquiler.estado == "Alquilado")
            {
                ActualizarDisponibilidad(alquiler.detalles);
            }
        }
        public string NotificarDevolucion()
        {
            var notificar = leer().Where(a => a.estado == "Alquilado" && a.fechaDevolucion == DateTime.Now.Date).ToList();
            if(notificar.Count()>0)
            {
                return $"Existen {notificar.Count} facturas que estan agendadas para devolver hoy ";
            }
            return null;
        }
        public string EntregarPendientes()
        {
            string entregados = null;
            var entregar = leer().Where(a=>a.fechaEntrega == DateTime.Now.Date && a.estado=="Pendiente").ToList();
            if (entregar.Count!=0)
            {
                foreach (var alquiler in entregar)
                {
                    ActualizarDisponibilidad(alquiler.detalles);
                    alquiler.estado = "Alquilado";
                    entregados = alquiler.id +" ";
                    Actualizar(alquiler);
                }
                return $"Se ha modificado el estado de las facturas {entregados} las cuales deben ser entregadas hoy ";
            }
            return null;
        }
        private void ActualizarDisponibilidad(List<DetalleAlquiler> detalles)
        {
            foreach (var item in detalles)
            {
                item.articulo.disponible -= item.cantidad;
                logicaArticulo.Actualizar(item.articulo);
            }
        }
        public List<Alquiler> leer()
        {
            return datosArticulos.Leer();
        }

        public Alquiler Buscar(int id)
        {
            return datosArticulos.Buscar(id);
        }

        public void Actualizar(Alquiler articuloActualizado)
        {
            datosArticulos.Actualizar(articuloActualizado);
        }

        public void Eliminar(int id)
        {
            datosArticulos.Eliminar(id);
        }
        public string ValidarPendientes(Alquiler alquiler)
        {
            if (leer()!=null || leer().Count!=0)
            {
                List<Alquiler> alquileres = leer().Where(a => a.fechaEntrega >= alquiler.fechaEntrega && a.fechaDevolucion <= alquiler.fechaDevolucion && a.estado == "Pendiente").ToList();
                if (alquileres != null || alquileres.Count!=0)
                {
                    foreach (var recorrido in alquileres)
                    {
                        foreach (var detalle in recorrido.detalles)
                        {
                            foreach (var item in alquiler.detalles)
                            {
                                if (detalle.articulo.id == item.articulo.id)
                                {
                                    if (detalle.cantidad + item.cantidad > item.articulo.disponible)
                                    {
                                        return $"El articulo: {item.articulo.categoria.descripcion} {item.articulo.descripcion} la cantidad es insufuciente para la fecha solicitada";
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            return null;

        }
    }
}
