using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class Usuario
    {
        public int Id { get; set; }
        public string usuario { get; set; }
        public string contraseña { get; set; }
        public string nombre { get; set; }
    }
}
