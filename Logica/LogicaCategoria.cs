using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaCategoria
    {
        public DatosCategoria datosCategorias = new DatosCategoria();
        public void Add(Categoria categoria)
        {
            datosCategorias.Add(categoria);
        }
        public Categoria Buscar(int id) 
        {
            return datosCategorias.Buscar(id);
        }
        public List<Categoria> leer()
        {
            return datosCategorias.Leer();
        }

        public void Actualizar(Categoria categoriaNuevo )
        {
            datosCategorias.Actualizar(categoriaNuevo);
        }

        public string Eliminar(int id) 
        {
            return datosCategorias.Eliminar(id);
        }


    }
}
