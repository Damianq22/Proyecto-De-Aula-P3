using Entidades;
using Logica;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentacion
{
    /// <summary>
    /// Lógica de interacción para VistaDetalles.xaml
    /// </summary>
    public partial class VistaDetalles : Window
    {
        LogicaAlquiler logicaAlquiler = new LogicaAlquiler();
        LogicaArticulo logicaArticulo = new LogicaArticulo();
        Alquiler alquiler = new Alquiler();
        ObservableCollection<DetalleAlquiler> detallesAlquiler;
        public VistaDetalles(Alquiler alquiler)
        {
            InitializeComponent();
            detallesAlquiler = new ObservableCollection<DetalleAlquiler>(alquiler.detalles);
            tablaArticulos.ItemsSource = detallesAlquiler;
            this.alquiler = alquiler;
            if (alquiler.estado != "Alquilado")
            {
                btnDevolucion.IsEnabled = false;
            }
        }

        private void btnDevolucion_Click(object sender, RoutedEventArgs e)
        {
            if (tablaArticulos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un artículo de la tabla.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DetalleAlquiler detalleAlquiler = (DetalleAlquiler)tablaArticulos.SelectedItem;

            // Validar entradas.
            if (!ValidarCampoNumerico(txtMalEstado, out int malEstado, "artículos en mal estado"))
            {
                return;
            }

            // Validar suma total.
            if (!ValidarSumaTotal(malEstado, detalleAlquiler.cantidad))
            {
                return;
            }

            // Actualizar los valores del artículo.
            Articulo articulo = logicaArticulo.Buscar(detalleAlquiler.articulo.id);
            int diferencia = detalleAlquiler.cantidad - malEstado;
            articulo.disponible += diferencia ;
            articulo.reparacion += malEstado;
            logicaArticulo.Actualizar(articulo);
            detallesAlquiler.RemoveAt(detalleAlquiler.id - 1);
            MessageBox.Show("Devolución exitosa", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
            

            // Cambiar el estado del alquiler si ya no quedan detalles pendientes.
            if (detallesAlquiler.Count == 0)
            {
                alquiler.estado = "Devuelto";
                logicaAlquiler.Actualizar(alquiler);
            }
        }
        private bool ValidarCampoNumerico(TextBox textBox, out int valor, string nombreCampo)
        {
            valor = 0;
            if (string.IsNullOrWhiteSpace(textBox.Text) || !int.TryParse(textBox.Text, out valor) || valor < 0)
            {
                MessageBox.Show($"Ingrese un número válido para {nombreCampo} (0 o mayor).", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarSumaTotal(int malEstado, int cantidadTotal)
        {
            if (malEstado > cantidadTotal)
            {
                MessageBox.Show("La suma de los artículos deben ser igual a la cantidad total.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

    }
}
