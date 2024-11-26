using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class BaseDatos
    {
        private string cadenaConexion = "Server=.\\SQLEXPRESS;Database=Rent4Event;Trusted_Connection=True;";
        protected SqlConnection conexion;
        public BaseDatos()
        {
            conexion = new SqlConnection(cadenaConexion);
        }
        public bool AbrirConexion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
                return true;
            }
            return false;
        }

        public void CerrarConexion()
        {
            conexion.Close();
        }

        public SqlConnection ObtenerConexion()
        {
            return conexion;
        }
    }
}

