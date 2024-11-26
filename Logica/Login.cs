using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Login
    {
        LogicaUsuario logicausuario = new LogicaUsuario();
        public static Usuario user;
        public Usuario Loguear(string usuario, string contraseña)
        {
           user = logicausuario.Leer().Where(u=>u.usuario == usuario && u.contraseña== contraseña).FirstOrDefault();
            return user;
        }
    }
}
