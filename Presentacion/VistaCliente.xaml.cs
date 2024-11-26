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
    /// Lógica de interacción para VistaCliente.xaml
    /// </summary>
    public partial class VistaCliente : UserControl
    { 
        LogicaCliente logicaCliente = new LogicaCliente();
        public VistaCliente()
        {
            InitializeComponent();
            tablaCliente.DataContext = logicaCliente.leer();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("El ID debe ser un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ValidarNombre(txtNombre.Text) ||
                !ValidarTelefono(txtTelefono.Text) ||
                !ValidarDireccion(txtDireccion.Text))
            {
                return;
            }

            
            Cliente cliente = new Cliente
            {
                Id = id,
                nombre = txtNombre.Text.Trim(),
                telefono = txtTelefono.Text.Trim(),
                direccion = txtDireccion.Text.Trim()
            };

            ActualizarTabla();
            Limpiar();
            MessageBox.Show(logicaCliente.Add(cliente), "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public bool ValidarNombre(string nombre)
        {
            if (!string.IsNullOrWhiteSpace(nombre) && System.Text.RegularExpressions.Regex.IsMatch(nombre, @"^[a-zA-Z\s]+$"))
            {
                return true;
            }

            MessageBox.Show("El nombre solo debe contener letras y espacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        public bool ValidarTelefono(string telefono)
        {
            if (!string.IsNullOrWhiteSpace(telefono) && telefono.All(char.IsDigit) && telefono.Length == 10)
            {
                return true;
            }

            MessageBox.Show("El teléfono debe contener exactamente 10 dígitos numéricos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        public bool ValidarDireccion(string direccion)
        {
            if (!string.IsNullOrWhiteSpace(direccion))
                return true;
            MessageBox.Show("Ingrese una dirección válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        public void Limpiar()
        {
            txtDireccion.Clear();
            txtId.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtBuscar.Clear();
            
        }
        public void ActualizarTabla()
        {
            tablaCliente.DataContext = logicaCliente.leer();
            txtBuscar.Clear();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(tablaCliente.SelectedItem!= null)
            {
                Cliente cliente = (Cliente)tablaCliente.SelectedItem;
                MessageBox.Show(logicaCliente.Eliminar(cliente.Id), "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                ActualizarTabla();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void refrescar_Click(object sender, RoutedEventArgs e)
        {
            ActualizarTabla();
        }

        private void buscar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtBuscar.Text, out int cedula))
            {
                Cliente cliente = logicaCliente.Buscar(cedula);

                if (cliente != null)
                {
                    List<Cliente> clientes = new List<Cliente> {cliente};
                    tablaCliente.DataContext = clientes;
                }
                else
                {
                    MessageBox.Show("No se encontró el cliente con la cedula proporcionada.","Informacion",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tablaCliente_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.Row.Item is Cliente clienteEditado)
            {
                var editingElement = e.EditingElement as TextBox;
                string nuevoValor = editingElement?.Text;
                switch (e.Column.Header.ToString())
                {
                    case "Telefono":
                        e.Cancel = !ValidarYActualizarTelefono(clienteEditado, nuevoValor);
                        break;

                    case "Direccion":
                        e.Cancel = !ValidarYActualizarDireccion(clienteEditado, nuevoValor);
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }

                if (!e.Cancel)
                {
                    MessageBox.Show(logicaCliente.Actualizar(clienteEditado), "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }
        private bool ValidarYActualizarTelefono(Cliente cliente, string nuevoTelefono)
        {
            // Validar que el teléfono sea exactamente de 10 dígitos numéricos
            if (!string.IsNullOrWhiteSpace(nuevoTelefono) &&
                nuevoTelefono.All(char.IsDigit) &&
                nuevoTelefono.Length == 10)
            {
                cliente.telefono = nuevoTelefono; // Actualizar el teléfono del cliente
                return true;
            }

            MessageBox.Show("El teléfono debe contener exactamente 10 dígitos numéricos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool ValidarYActualizarDireccion(Cliente cliente, string nuevaDireccion)
        {
            // Validar que la dirección no esté vacía y tenga al menos 5 caracteres
            if (!string.IsNullOrWhiteSpace(nuevaDireccion) && nuevaDireccion.Length >= 5)
            {
                cliente.direccion = nuevaDireccion; // Actualizar la dirección del cliente
                return true;
            }

            MessageBox.Show("La dirección debe contener al menos 5 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }


        private void tablaCliente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var dataGrid = (DataGrid)sender;
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                e.Handled = true; 
            }

        }
    }
    }
