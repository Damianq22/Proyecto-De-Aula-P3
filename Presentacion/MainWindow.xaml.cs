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
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Login login = new Login(); 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            if(login.Loguear(txtUsuario.Text,txtContraseña.Password)!=null)
            {
                VistaPrincipal principal = new VistaPrincipal();
                principal.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error usuario y contraseñas incorrectas\n Vuelva a intentarlo", "Informacion", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }
    }
}
