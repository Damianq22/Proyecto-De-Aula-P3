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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentacion
{
    /// <summary>
    /// Lógica de interacción para VistaFacturas.xaml
    /// </summary>
    public partial class VistaFacturas : UserControl
    {
        LogicaAlquiler logicaAlquiler = new LogicaAlquiler();
        ObservableCollection<Alquiler> alquileres;
        public VistaFacturas()
        {
            InitializeComponent();
           
            alquileres = new ObservableCollection<Alquiler>(logicaAlquiler.leer());
            tablaAlquiler.ItemsSource = alquileres;
            cbEstado.Items.Add("Alquilado");
            cbEstado.Items.Add("Pendiente");
            cbEstado.Items.Add("Devuelto");
            if (logicaAlquiler.NotificarDevolucion() != null)
            {
                MessageBox.Show(logicaAlquiler.NotificarDevolucion(), "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            

        }
        private void tablaAlquiler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tablaAlquiler.SelectedItem != null)
            {
                Alquiler alquiler = (Alquiler)tablaAlquiler.SelectedItem;
                VistaDetalles vistaDetalles = new VistaDetalles(alquiler);
                vistaDetalles.Show();
            }
        }

        private void cbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbEstado.SelectedItem != null)
            {
                string estado = cbEstado.SelectedItem.ToString();
                ObservableCollection<Alquiler> filtrado = new ObservableCollection<Alquiler>(logicaAlquiler.leer().Where(a => a.estado == estado).ToList());
                tablaAlquiler.ItemsSource=filtrado;
            }
        }
        public void Actualizar()
        {
            cbEstado.SelectedItem= null;
            txtBuscarFactura.Clear();
            tablaAlquiler.ItemsSource = alquileres;
            dpFechaFinal.SelectedDate = null;
            dpFechaInicial.SelectedDate = null;
        }
        private void btnBuscarFactura_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtBuscarFactura.Text, out int facturaId))
            {
                Alquiler alquiler = logicaAlquiler.Buscar(facturaId);

                if (alquiler != null)
                {
                    ObservableCollection<Alquiler> alquileres = new ObservableCollection<Alquiler> { alquiler };
                    tablaAlquiler.ItemsSource = alquileres;
                }
                else
                {
                    MessageBox.Show("No se encontró el alquiler con el ID proporcionado.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un número válido.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();

        }

        private void btnEntregar_Click(object sender, RoutedEventArgs e)
        {
            if (logicaAlquiler.EntregarPendientes() != null)
            {
                MessageBox.Show(logicaAlquiler.EntregarPendientes(), "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No existen facturas pendientes para hoy", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

       
        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dpFechaInicial.SelectedDate !=null && dpFechaFinal.SelectedDate != null)
            {
                DateTime fechaInicial = dpFechaInicial.SelectedDate.Value;
                DateTime fechaFinal = dpFechaFinal.SelectedDate.Value;
                if(fechaFinal<fechaInicial)
                {
                    MessageBox.Show("La fecha final no debe ser menor a la fecha incial", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var rango = logicaAlquiler.leer().Where(a => a.fechaEntrega >=fechaInicial && a.fechaEntrega <=fechaFinal).ToList();
                    ObservableCollection<Alquiler> filtrado = new ObservableCollection<Alquiler>(rango);
                    tablaAlquiler.ItemsSource=filtrado;
                }
            }
        }
    }
}
