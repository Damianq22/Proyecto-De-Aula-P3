using Entidades;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Lógica de interacción para VistaCategoria.xaml
    /// </summary>
    public partial class VistaCategoria : UserControl
    {
        LogicaCategoria  logicaCategoria = new LogicaCategoria(); 
        public VistaCategoria()
        {
            InitializeComponent();
            tablaCategoria.DataContext= logicaCategoria.leer();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos(txtDescripcion.Text))
            {
                Categoria categoria = new Categoria();
                categoria.descripcion = txtDescripcion.Text;
                logicaCategoria.Add(categoria);
                ActualizarTabla();
                MessageBox.Show("Registro exitoso", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                txtDescripcion.Clear();
            }
        }
        public bool ValidarCampos(string campo)
        {
            if (string.IsNullOrEmpty(campo))
            {
                MessageBox.Show("Campo vacío", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else
            {
               return true;
            }
        }
        public void ActualizarTabla()
        {
            tablaCategoria.DataContext = logicaCategoria.leer();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(tablaCategoria.SelectedItem!= null)
            {
                Categoria categoria = (Categoria) tablaCategoria.SelectedItem;
                MessageBox.Show(logicaCategoria.Eliminar(categoria.id),"Informacion",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Seleccione una categoria","Informacion",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }


    }
}
