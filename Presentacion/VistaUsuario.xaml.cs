using Entidades;
using Logica;
using System.Windows;
using System.Windows.Controls;

namespace Presentacion
{
    /// <summary>
    /// Lógica de interacción para VistaUsuario.xaml
    /// </summary>
    public partial class VistaUsuario : UserControl
    {
        LogicaUsuario logicaUsuario = new LogicaUsuario();
        public VistaUsuario()
        {
            InitializeComponent();
            tablaUsuario.DataContext = logicaUsuario.Leer();
        }

        public void ActualizarTabla()
        {
            tablaUsuario.DataContext = logicaUsuario.Leer();
        }
        public void Limpiar()
        {
            txtContraseña.Clear();
            txtNombre.Clear();
            txtUsuario.Clear();
        }
        private bool ValidarTexto(string texto, string campo, out string resultado)
        {
            resultado = string.Empty;
            if (string.IsNullOrWhiteSpace(texto))
            {
                MessageBox.Show($"El campo {campo} no puede estar vacío.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            resultado = texto;
            return true;
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = new Usuario();
            if (!ValidarTexto(txtUsuario.Text, "Usuario", out string usuarioNombre))
            {
                return;
            }
            usuario.usuario = usuarioNombre;
            if (!ValidarTexto(txtContraseña.Text, "Contraseña", out string contraseña))
            {
                return;
            }
            usuario.contraseña = contraseña;

            if (!ValidarTexto(txtNombre.Text, "Nombre", out string nombre))
            {
                return;
            }
            usuario.nombre = nombre;
            logicaUsuario.Add(usuario);
            ActualizarTabla();
            Limpiar();
            MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    
}


