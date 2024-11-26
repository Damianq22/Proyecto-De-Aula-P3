using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class Alquiler
    {
        public int id { get; set; }
        public DateTime fechaEntrega  { get; set; }
        public DateTime fechaDevolucion { get; set; }
        public double totalPago { get; set; }
        public string estado { get; set; }
        public Cliente cliente { get; set; }
        public Usuario usuario { get; set; }
        public List<DetalleAlquiler> detalles { get; set; }
 
       
        public void CalcularTotal()
        {
            foreach (var item in detalles)
            {
                totalPago += item.subTotal;
            }
        }

        public void TotalPorDias()
        {
           TimeSpan diferencia = fechaDevolucion - fechaEntrega;

            int diasDiferencia = diferencia.Days;

            totalPago = totalPago * diasDiferencia;
        }
    }
}
