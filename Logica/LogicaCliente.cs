using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaCliente
    {
        public DatosCliente datosCliente = new DatosCliente();
        public string Add(Cliente cliente)
        {
            return datosCliente.Add(cliente);
        }

        public List<Cliente> leer()
        {
            return datosCliente.Leer();
        }

        public Cliente Buscar(int id)
        {
            return datosCliente.Buscar(id);
        }

        public string Actualizar(Cliente clienteActualizado)
        {
            return datosCliente.Actualizar(clienteActualizado);
        }

        public string Eliminar(int id)
        {
            return datosCliente.Eliminar(id);
        }

    }
}

