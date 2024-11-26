using Entidades;
using Logica;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para VistaProveedor.xaml
    /// </summary>
    public partial class VistaProveedor : UserControl
    {
        LogicaProveedor logicaProveedor = new LogicaProveedor();
        public VistaProveedor()
        {
            InitializeComponent();
            ActualizarTabla();
        }
        public string ValidarCampos()
        {
            
            // Verifica si el campo de nombre está vacío
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                return "El campo 'Nombre' está vacío.";
            }

            // Verifica si el campo de teléfono está vacío
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                return "El campo 'Teléfono' está vacío.";
            }

            // Verifica si el teléfono tiene exactamente 10 caracteres numéricos
            if (txtTelefono.Text.Length != 10 || !txtTelefono.Text.All(char.IsDigit))
            {
                return "El campo 'Teléfono' debe contener exactamente 10 caracteres numéricos.";
            }

            return null;  // Si todos los campos son válidos, retorna null
        }


        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            // Llama a ValidarCampos para obtener el mensaje de error si existe
            string errorMessage = ValidarCampos();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Detiene la ejecución si hay un error
            }

            try
            {
                Proveedor proveedor = new Proveedor
                {
                    nombre = txtNombre.Text,
                    telefono = txtTelefono.Text
                };
                if (logicaProveedor.Buscar(proveedor.id)!=null) 
                {
                    MessageBox.Show("Proveedor existente", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                logicaProveedor.Add(proveedor);

                MessageBox.Show("Registro exitoso", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                ActualizarTabla();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar el proveedor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Limpiar()
        {
            txtTelefono.Clear();
            txtNombre.Clear();
        }
        public void ActualizarTabla()
        {
            tablaProveedor.DataContext = logicaProveedor.leer();
            txtBuscar.Clear();
        }

        private void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            ActualizarTabla();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(tablaProveedor.SelectedItem != null) { 
                Proveedor proveedor = (Proveedor) tablaProveedor.SelectedItem;
                MessageBox.Show(logicaProveedor.Eliminar(proveedor.id),"Informacion",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Seleccione un proveedor", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void tablaProveedor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var dataGrid = (DataGrid)sender;
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                e.Handled = true;
            }
        }

        private void tablaProveedor_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is Proveedor proveedorEditado)
            {
                var editingElement = e.EditingElement as TextBox;
                string nuevoValor = editingElement?.Text;
                switch (e.Column.Header.ToString())
                {
                    case "Telefono":
                        e.Cancel = !ValidarYActualizarTelefono(proveedorEditado, nuevoValor);
                        break;

                    case "Nombre":
                        e.Cancel = !ValidarYActualizarNombre(proveedorEditado, nuevoValor);
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }

                if (!e.Cancel)
                {
                    MessageBox.Show(logicaProveedor.Actualizar(proveedorEditado), "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }
        private bool ValidarYActualizarNombre(Proveedor proveedor, string nuevoNombre)
        {
            if (!string.IsNullOrWhiteSpace(nuevoNombre) &&
                nuevoNombre.Length >= 3 &&
                System.Text.RegularExpressions.Regex.IsMatch(nuevoNombre, @"^[a-zA-Z\s]+$"))
            {
                proveedor.nombre = nuevoNombre;
                return true;
            }

            MessageBox.Show("El nombre debe contener al menos 3 caracteres y solo incluir letras y espacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool ValidarYActualizarTelefono(Proveedor proveedor, string nuevoTelefono)
        {
            if (!string.IsNullOrWhiteSpace(nuevoTelefono) &&
                nuevoTelefono.All(char.IsDigit) &&
                nuevoTelefono.Length == 10)
            {
                proveedor.telefono = nuevoTelefono;
                return true;
            }

            MessageBox.Show("El teléfono debe contener exactamente 10 dígitos numéricos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
          
                if (int.TryParse(txtBuscar.Text, out int id))
                {
                    Proveedor proveedor = logicaProveedor.Buscar(id);

                    if (proveedor != null)
                    {
                        List<Proveedor> proveedores = new List<Proveedor> { proveedor };
                        tablaProveedor.DataContext = proveedores;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el proveedor.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            
        }
    }
}
