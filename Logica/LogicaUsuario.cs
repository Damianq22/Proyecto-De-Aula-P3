using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaUsuario
    {
        DatosUsuario datosUsuario = new DatosUsuario();

        public void Add(Usuario usuario)
        {
            datosUsuario.Add(usuario);
        }

        public List<Usuario> Leer() 
        {
            return datosUsuario.Leer();
        }

        public Usuario Buscar(int id)
        {
            return datosUsuario.Buscar(id);
        }

        public void Eliminar( int id)
        {
            datosUsuario.Eliminar(id);
        }

        public void Actualizar( Usuario usuarioNuevo)
        { 
            datosUsuario.Actualizar(usuarioNuevo);
        }


    }
}
