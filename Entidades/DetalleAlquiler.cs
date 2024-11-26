using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{

    [Serializable]
    public class DetalleAlquiler
    {
        public int id { get; set; }
        public Articulo articulo { get; set; }
        public double subTotal { get; set; }
        public int cantidad { get; set; }
        public void CalcularSubtotal()
        {
            subTotal = articulo.precioAlquiler * cantidad;
        }
    }
}
