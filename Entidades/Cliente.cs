using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class Cliente
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set;}
        public string direccion { get; set; }

    }
}
