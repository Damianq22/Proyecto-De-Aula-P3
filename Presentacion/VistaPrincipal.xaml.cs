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
using System.Windows.Shapes;

namespace Presentacion
{
    /// <summary>
    /// Lógica de interacción para VistaPrincipal.xaml
    /// </summary>
    public partial class VistaPrincipal : Window
    {
        private const double OriginalWidth = 1400;
        private const double OriginalHeight = 720;
        private const double MaximizedWidth = 1200;
        private const double MaximizedHeight = 720;
        public VistaPrincipal()
        {
            InitializeComponent();
        }

        //Control
        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //CERRAR
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Escalado
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                
                double scaleX = MaximizedWidth / OriginalWidth;
                double scaleY = MaximizedHeight / OriginalHeight;
                double scale = Math.Min(scaleX, scaleY);

                gridScaleTransform.ScaleX = scale;
                gridScaleTransform.ScaleY = scale;
            }
            else
            {
                gridScaleTransform.ScaleX = 1;
                gridScaleTransform.ScaleY = 1;
            }
        }


        private void btnArticulos_Click(object sender, RoutedEventArgs e)
        {
            FrameVistaPrincipal.Navigate(new Uri("VistaArticulo.xaml", UriKind.Relative));
        }


        private void btnAlquiler_Click(object sender, RoutedEventArgs e)
        {
            FrameVistaPrincipal.Navigate(new Uri("VistaAlquiler.xaml", UriKind.Relative));
        }

        private void btnProveedor_Click(object sender, RoutedEventArgs e)
        {
            FrameVistaPrincipal.Navigate(new Uri("VistaProveedor.xaml", UriKind.Relative));
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            FrameVistaPrincipal.Navigate(new Uri("VistaUsuario.xaml", UriKind.Relative));
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            FrameVistaPrincipal.Navigate(new Uri("VistaCategoria.xaml", UriKind.Relative));

        }

        private void btnCliente_Click_1(object sender, RoutedEventArgs e)
        {
            FrameVistaPrincipal.Navigate(new Uri("VistaCliente.xaml", UriKind.Relative));

        }

        private void btnFacturas_Click(object sender, RoutedEventArgs e)
        {
            FrameVistaPrincipal.Navigate(new Uri("VistaFacturas.xaml", UriKind.Relative));

        }
    }
}
