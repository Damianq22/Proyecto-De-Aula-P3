using Entidades;
using Logica;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentacion
{
    public partial class VistaAlquiler : UserControl
    {
        ObservableCollection<DetalleAlquiler> detalles = new ObservableCollection<DetalleAlquiler>();
        LogicaArticulo logicaArticulo = new LogicaArticulo();
        LogicaCategoria logicaCategoria = new LogicaCategoria();
        LogicaAlquiler logicaAlquiler = new LogicaAlquiler();
        LogicaCliente logicaCliente = new LogicaCliente();
        int i = 0;
        double total = 0;
        public VistaAlquiler()
        {
            InitializeComponent();
            cbCategoria.ItemsSource = logicaCategoria.leer();
            tablaArticulos.ItemsSource = detalles;
        }

        private void cbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Categoria categoria = (Categoria)cbCategoria.SelectedItem;
            if (categoria != null)
            {
                cbArticulos.ItemsSource = logicaArticulo.leer().Where(a => a.categoria.id == categoria.id).ToList();

            }

        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if(dpFechaDevolucion.SelectedDate.Value!=null && dpFechaDeEntrega.SelectedDate.Value != null)
            {
                if (ValidarFechas(dpFechaDeEntrega.SelectedDate.Value, dpFechaDevolucion.SelectedDate.Value))
                {
                    TimeSpan diferencia = dpFechaDevolucion.SelectedDate.Value - dpFechaDeEntrega.SelectedDate.Value;

                    int diasDiferencia = diferencia.Days;
                    if (diasDiferencia == 0) diasDiferencia = 1;
                    DetalleAlquiler detalleAlquiler = new DetalleAlquiler();
                    if (cbArticulos.SelectedItem == null)
                    {
                        MessageBox.Show("Por favor, seleccione un artículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
                    {
                        MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    detalleAlquiler.articulo = (Articulo)cbArticulos.SelectedItem;
                    detalleAlquiler.cantidad = cantidad;
                    if (detalleAlquiler.cantidad > detalleAlquiler.articulo.disponible)
                    {
                        MessageBox.Show("Error: No hay suficientes artículos disponibles para lo solicitado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (i == 0)
                    {
                        i += 1;
                        detalleAlquiler.id = i;

                    }
                    else
                    {
                        i += 1;
                        detalleAlquiler.id = i++;
                    }

                    detalleAlquiler.CalcularSubtotal();

                    detalles.Add(detalleAlquiler);

                    total += detalleAlquiler.subTotal;
                    total = total * diasDiferencia;
                    lbTotal.Content = total.ToString();
                    AddLimpiar();
                }
            }
            else
            {
                MessageBox.Show("Seleccione las fechas", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            

           
        }

        private void btnRegistrarAlquiler_Click(object sender, RoutedEventArgs e)
        {
            if (!CamposCompletos()) return;
            if (!ValidarFechas(dpFechaDeEntrega.SelectedDate, dpFechaDevolucion.SelectedDate)) return;

            Alquiler alquiler = new Alquiler
            {
                id = logicaAlquiler.leer().Count+1,
                fechaEntrega = dpFechaDeEntrega.SelectedDate.Value,
                fechaDevolucion = dpFechaDevolucion.SelectedDate.Value,
                detalles = detalles.Count > 0 ? detalles.ToList() : null
            };

            if (alquiler.detalles == null)
            {
                MessageBox.Show("No hay artículos ingresados para alquilar.","Informacion",MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }

            alquiler.CalcularTotal();
            alquiler.TotalPorDias();
            if(alquiler.fechaEntrega.Date == DateTime.Now.Date)
            {
                alquiler.estado = "Alquilado";
            }
            if (alquiler.fechaEntrega.Date > DateTime.Now.Date)
            {
                alquiler.estado = "Pendiente";
            }
            if (logicaAlquiler.ValidarPendientes(alquiler) == null)
            {
                alquiler.cliente = logicaCliente.Buscar(int.Parse(txtBuscarCliente.Text));
                alquiler.usuario = Login.user;
                logicaAlquiler.Add(alquiler);
                MessageBox.Show("Registro existoso","Confirmacion",MessageBoxButton.OK, MessageBoxImage.Information);
                AlquilerLimpiar();
            }
            else
            {
                MessageBox.Show(logicaAlquiler.ValidarPendientes(alquiler),"Informacion",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            
        }
        
        private bool CamposCompletos()
        {
            if (!dpFechaDeEntrega.SelectedDate.HasValue ||
                !dpFechaDevolucion.SelectedDate.HasValue)
            {
                MessageBox.Show("Todos los campos deben estar completos.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public bool ValidarCedula(string cedulaTexto)
        {
            if (int.TryParse(cedulaTexto, out int cedula) && cedula > 0)
                return true;
            MessageBox.Show("Ingrese una cédula válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        

        public bool ValidarFechas(DateTime? fechaEntrega, DateTime? fechaDevolucion)
        {
            if (fechaEntrega.HasValue && fechaDevolucion.HasValue && fechaDevolucion >= fechaEntrega && fechaEntrega>=DateTime.Now.Date)
                return true;
            MessageBox.Show("Verifique las fechas de entrega y devolución.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            return false;
        }

        private void cbArticulos_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (cbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una categoria primero", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (cbArticulos.Items.Count == 0)
                {
                    MessageBox.Show("No existen articulos registrados con la categoria seleccionada", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaArticulos.SelectedItem != null)
            {
                DetalleAlquiler detalleAlquiler = (DetalleAlquiler)tablaArticulos.SelectedItem;
                detalles.RemoveAt(detalleAlquiler.id-1);
                total -= detalleAlquiler.subTotal;
                lbTotal.Content = total;
                MessageBox.Show("Eliminado correctamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                i = detalles.Count;
            }
            else
            {
                MessageBox.Show("Seleccione un registro a eliminar", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
        public void AddLimpiar()
        {
            txtCantidad.Clear();
            cbArticulos.SelectedItem = null;
            cbCategoria.SelectedItem = null;
        }
        public void AlquilerLimpiar()
        {
            AddLimpiar();
            txtBuscarCliente.Clear();
            txtRecibido.Clear();
            lbCambio.Content = " ";
            lblCliente.Content = " ";
            lbTotal.Content = " ";
            dpFechaDeEntrega.SelectedDate = null;
            dpFechaDevolucion.SelectedDate = null;
            total = 0;
            i = 0;
        }
        private void txtRecibido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Llamar al método de validación
                if (!EsDineroValido(txtRecibido.Text, out double recibido))
                {
                    MessageBox.Show("Por favor, ingrese una cantidad válida de dinero.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (total == 0)
                {
                    MessageBox.Show("Registre artículos para alquilar", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (recibido < total)
                {
                    MessageBox.Show("Cantidad recibida menor al pago total", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Calcular y mostrar el cambio
                lbCambio.Content = recibido - total;
            }
        }

        private bool EsDineroValido(string texto, out double valor)
        {
            return double.TryParse(texto, out valor) && valor >= 0;
        }

        private void btnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            if(ValidarCedula(txtBuscarCliente.Text))
            {
                Cliente cliente = logicaCliente.Buscar(int.Parse(txtBuscarCliente.Text));
                if (cliente != null)
                {
                    lblCliente.Content = cliente.nombre;
                }
                else
                {
                    MessageBox.Show("Cliente no existente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
