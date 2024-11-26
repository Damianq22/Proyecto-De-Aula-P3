using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class Proveedor
    {
        public int id { get; set; }
        public List<Articulo> articulos { get; set; }
        public string telefono { get; set; }
        public string nombre { get; set; }
    }
}
