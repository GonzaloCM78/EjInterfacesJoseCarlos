using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace EjInterfacesJoseCarlos
{
    public partial class MainWindow : Window
    {
        private Database db; // Instancia de la clase Database

        public MainWindow()
        {
            InitializeComponent();
            db = new Database(); // Inicializamos la base de datos
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contraseña = txtPassword.Password;

            if (ValidarUsuario(usuario, contraseña))
            {
                MessageBox.Show("Inicio de sesión exitoso");
                VentanaMenu ventanaMenu = new VentanaMenu();
                ventanaMenu.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }

        private bool ValidarUsuario(string usuario, string contraseña)
        {
            bool isValid = false;
            try
            {
                db.OpenConnection();
                string query = "SELECT COUNT(*) FROM usuarios WHERE username = @usuario AND password = @contraseña";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contraseña", contraseña);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
            return isValid;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
