using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class Articulo
    {
        public int id {  get; set; }
        public string descripcion { get; set; }
        public double precioAlquiler { get; set; }
        public double precioProveedor { get; set; }
        public int cantidad { get; set; }
        public int disponible { get; set; }
        public Proveedor proveedor { get; set; }
        public Categoria categoria { get; set; }
        public int reparacion { get; set; }
    }
}
