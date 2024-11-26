using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaProveedor
    {
        public DatosProveedor datosProveedor = new DatosProveedor();
        public void Add(Proveedor proveedor)
        {
            datosProveedor.Add(proveedor);
        }
        public Proveedor Buscar(int id) 
        {
            return datosProveedor.Buscar(id);   
        }
        public List<Proveedor> leer()
        {
            return datosProveedor.Leer();
        }
        public string Actualizar( Proveedor proveedorNuevo) 
        {
            return datosProveedor.Actualizar(proveedorNuevo);
        }

        public string Eliminar(int id)
        {
            return datosProveedor.Eliminar(id);
        }
    }
}
