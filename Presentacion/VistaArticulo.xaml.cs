using Entidades;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Presentacion
{
    /// <summary>
    /// Lógica de interacción para VistaArticulo.xaml
    /// </summary>
    public partial class VistaArticulo : System.Windows.Controls.UserControl
    {
        LogicaArticulo logicaArticulo = new LogicaArticulo();
        LogicaCategoria logicaCategoria = new LogicaCategoria();
        LogicaProveedor logicaProveedor = new LogicaProveedor();
        public VistaArticulo()
        {
            InitializeComponent();
            tablaArticulos.DataContext = logicaArticulo.leer();
            cbCategoria.ItemsSource=logicaCategoria.leer();
            cbProveedor.ItemsSource = logicaProveedor.leer();
            cbFiltrar.ItemsSource = logicaCategoria.leer();
        }

        private void refrescar_Click(object sender, RoutedEventArgs e)
        {
            ActualizarTabla();
        }

        private void CbFiltrar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbFiltrar.SelectedItem != null)
            {
                Categoria categoria = (Categoria)cbFiltrar.SelectedItem;
                var articulosFiltrados = logicaArticulo.leer().Where(a => a.categoria.id == categoria.id).ToList();
                tablaArticulos.DataContext = articulosFiltrados;
            } 
        }
        public string ValidarCampos()
        {
            string resultado;

            resultado = ValidarCategoria();
            if (resultado != null) return resultado;

            resultado = ValidarProveedor();
            if (resultado != null) return resultado;

            resultado = ValidarDescripcion();
            if (resultado != null) return resultado;

            resultado = ValidarPrecios();
            if (resultado != null) return resultado;

            resultado = ValidarCantidad();
            if (resultado != null) return resultado;

            return null; // Todo es válido
        }

        

        private string ValidarCategoria()
        {
            if (cbCategoria.SelectedItem == null)
            {
                return "Debe seleccionar una categoría.";
            }
            return null;
        }

        private string ValidarProveedor()
        {
            if (cbProveedor.SelectedItem == null)
            {
                return "Debe seleccionar un proveedor.";
            }
            return null;
        }

        private string ValidarDescripcion()
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                return "El campo 'Descripción' está vacío.";
            }
            return null;
        }

        private string ValidarPrecios()
        {
            if (string.IsNullOrWhiteSpace(txtPrecioAlquiler.Text))
            {
                return "El campo 'Precio Alquiler' está vacío.";
            }
            if (!double.TryParse(txtPrecioAlquiler.Text, out _))
            {
                return "El campo 'Precio Alquiler' debe ser un número válido.";
            }
            if (string.IsNullOrWhiteSpace(txtPrecioProveedor.Text))
            {
                return "El campo 'Precio Proveedor' está vacío.";
            }
            if (!double.TryParse(txtPrecioProveedor.Text, out _))
            {
                return "El campo 'Precio Proveedor' debe ser un número válido.";
            }
            return null;
        }

        private string ValidarCantidad()
        {
            if (string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                return "El campo 'Cantidad' está vacío.";
            }
            if (!int.TryParse(txtCantidad.Text, out _))
            {
                return "El campo 'Cantidad' debe ser un número válido.";
            }
            return null;
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = ValidarCampos();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Articulo articulo = new Articulo
                {
                    categoria = (Categoria)cbCategoria.SelectedItem,
                    proveedor = (Proveedor)cbProveedor.SelectedItem,
                    descripcion = txtDescripcion.Text,
                    precioAlquiler = double.Parse(txtPrecioAlquiler.Text),
                    precioProveedor = double.Parse(txtPrecioProveedor.Text),
                    cantidad = int.Parse(txtCantidad.Text),
                    disponible = int.Parse(txtCantidad.Text)
                };
                if (articulo.precioAlquiler >= articulo.precioProveedor)
                {
                    MessageBox.Show("El precio de alquiler debe ser menor al precio de compra", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (logicaArticulo.Buscar(articulo.id) != null)
                {
                    MessageBox.Show("Articulo ya existente", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                logicaArticulo.Add(articulo);

                MessageBox.Show("Registro exitoso", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();
                ActualizarTabla();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar el artículo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Limpiar()
        {
            cbCategoria.SelectedItem = null;
            cbProveedor.SelectedItem = null;
            txtDescripcion.Clear();
            txtPrecioProveedor.Clear();
            txtPrecioAlquiler.Clear();
            txtCantidad.Clear();
        }
        public void ActualizarTabla()
        {
            cbFiltrar.SelectedItem = null ;
            tablaArticulos.DataContext = logicaArticulo.leer();
        }

        

        private void BuscarArticulo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Articulo articuloBuscado = logicaArticulo.Buscar(int.Parse(txtBuscar.Text));
                if(articuloBuscado != null)
                {
                    tablaArticulos.DataContext = null;
                    List<Articulo> list = new List<Articulo>() { articuloBuscado};
                    tablaArticulos.DataContext = list;
                }
                else
                {
                    MessageBox.Show("Articulo no existente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void tablaArticulos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is Articulo articuloEditado)
            {
                var editingElement = e.EditingElement as TextBox;
                string nuevoValor = editingElement?.Text;
                switch (e.Column.Header.ToString())
                {
                    case "Precio Alquiler":
                        e.Cancel = !ValidarYActualizarPrecioAlquiler(articuloEditado, nuevoValor);
                        break;

                    case "Precio Compra":
                        e.Cancel = !ValidarYActualizarPrecioCompra(articuloEditado, nuevoValor);
                        break;

                    case "Cantidad":
                        e.Cancel = !ValidarYActualizarCantidad(articuloEditado, nuevoValor);
                        break;

                    case "Reparacion":
                        e.Cancel = !ValidarYActualizarReparacion(articuloEditado, nuevoValor);
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }

                if (!e.Cancel)
                {
                    MessageBox.Show(logicaArticulo.Actualizar(articuloEditado), "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    ActualizarTabla();
                }

            }
            }
        private bool ValidarYActualizarPrecioAlquiler(Articulo articulo, string nuevoValor)
        {
            if (string.IsNullOrWhiteSpace(nuevoValor))
            {
                MessageBox.Show("El precio de alquiler no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (double.TryParse(nuevoValor, out double nuevoPrecio) && nuevoPrecio > 0)
            {
                if (nuevoPrecio >= articulo.precioProveedor)
                {
                    MessageBox.Show("El precio de alquiler debe ser menor que el precio de compra.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                articulo.precioAlquiler = nuevoPrecio;
                return true;
            }

            MessageBox.Show("El precio de alquiler debe ser un número válido y mayor que 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        private bool ValidarYActualizarPrecioCompra(Articulo articulo, string nuevoValor)
        {
            if (string.IsNullOrWhiteSpace(nuevoValor))
            {
                MessageBox.Show("El precio de compra no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (double.TryParse(nuevoValor, out double nuevoPrecio) && nuevoPrecio > 0)
            {
                if (nuevoPrecio <= articulo.precioAlquiler)
                {
                    MessageBox.Show("El precio de compra debe ser mayor que el precio de alquiler.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                articulo.precioProveedor = nuevoPrecio;
                return true;
            }

            MessageBox.Show("El precio de compra debe ser un número válido y mayor que 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        private bool ValidarYActualizarCantidad(Articulo articulo, string nuevoValor)
        {
            if (string.IsNullOrWhiteSpace(nuevoValor))
            {
                MessageBox.Show("La cantidad no puede estar vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (int.TryParse(nuevoValor, out int nuevaCantidad) && nuevaCantidad >= 0)
            {
                int diferencia = nuevaCantidad - articulo.cantidad;
                articulo.cantidad = nuevaCantidad;
                articulo.disponible += diferencia; // Ajustar disponibles si la cantidad cambia
                return true;
            }

            MessageBox.Show("La cantidad debe ser un número válido y no puede ser negativa.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        
        private bool ValidarYActualizarReparacion(Articulo articulo, string nuevoValor)
        {
            if (string.IsNullOrWhiteSpace(nuevoValor))
            {
                MessageBox.Show("El valor de 'Reparación' no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (int.TryParse(nuevoValor, out int nuevaReparacion) && nuevaReparacion >= 0 && nuevaReparacion <= articulo.cantidad)
            {
                int diferencia = nuevaReparacion - articulo.reparacion;
                articulo.reparacion = nuevaReparacion;
                articulo.disponible -= diferencia; // Ajustar disponibles si reparación cambia
                return true;
            }

            MessageBox.Show("El valor de 'Reparación' debe ser un número válido y no puede exceder la cantidad total.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private void tablaArticulos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var dataGrid = (DataGrid)sender;
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                e.Handled = true; // Evita el comportamiento predeterminado de moverse a la siguiente celda
            }

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaArticulos.SelectedItem != null)
            {
                var result = MessageBox.Show("¿Estás seguro de que deseas eliminar este registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Articulo articuloEliminar = (Articulo)tablaArticulos.SelectedItem;
                    MessageBox.Show(logicaArticulo.Eliminar(articuloEliminar.id), "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    ActualizarTabla();
                }
            }
        }
    }
}
