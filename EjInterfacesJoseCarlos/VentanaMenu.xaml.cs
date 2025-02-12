using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace EjInterfacesJoseCarlos
{
    public partial class VentanaMenu : Window
    {
        private Database db;

        public VentanaMenu()
        {
            InitializeComponent();
            db = new Database();
            CargarProductos();
            CargarCategorias();
        }

        private void CargarProductos()
        {
            try
            {
                db.OpenConnection();
                string query = "SELECT ProductName, CategoryID FROM products";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ProductosDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void CargarCategorias()
        {
            try
            {
                db.OpenConnection();
                string query = "SELECT CategoryID, CategoryName FROM categories";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                CategoriasDataGrid.ItemsSource = dt.DefaultView;
                ModificarCategoriasDataGrid.ItemsSource = dt.DefaultView;

                CategoriasComboBox.ItemsSource = dt.DefaultView;
                CategoriasComboBox.DisplayMemberPath = "CategoryName";
                CategoriasComboBox.SelectedValuePath = "CategoryID";

                ModificarCategoriasComboBox.ItemsSource = dt.DefaultView;
                ModificarCategoriasComboBox.DisplayMemberPath = "CategoryName";
                ModificarCategoriasComboBox.SelectedValuePath = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {
            InicioPanel.Visibility = Visibility.Visible;
            AgregarPanel.Visibility = Visibility.Collapsed;
            ModificarPanel.Visibility = Visibility.Collapsed;
            BorrarPanel.Visibility = Visibility.Collapsed;
            CargarProductos();
            CargarCategorias();
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            InicioPanel.Visibility = Visibility.Collapsed;
            AgregarPanel.Visibility = Visibility.Visible;
            ModificarPanel.Visibility = Visibility.Collapsed;
            BorrarPanel.Visibility = Visibility.Collapsed;

            CargarProductos();
            CargarCategorias();

            AgregarProductosDataGrid.ItemsSource = ProductosDataGrid.ItemsSource;
            AgregarCategoriasDataGrid.ItemsSource = CategoriasDataGrid.ItemsSource;
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            InicioPanel.Visibility = Visibility.Collapsed;
            AgregarPanel.Visibility = Visibility.Collapsed;
            BorrarPanel.Visibility = Visibility.Collapsed;
            ModificarPanel.Visibility = Visibility.Visible;

            CargarProductos();
            CargarCategorias();

            ModificarProductosDataGrid.ItemsSource = ProductosDataGrid.ItemsSource;
            ModificarCategoriasDataGrid.ItemsSource = CategoriasDataGrid.ItemsSource;
        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            InicioPanel.Visibility = Visibility.Collapsed;
            AgregarPanel.Visibility = Visibility.Collapsed;
            ModificarPanel.Visibility = Visibility.Collapsed;
            BorrarPanel.Visibility = Visibility.Visible;

            CargarProductosBorrar();
            CargarCategoriasBorrar();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButonModificar_Click(object sender, RoutedEventArgs e)
        {
            string productoAntiguo = ProductoAntiguoTextBox.Text.Trim();
            string productoNuevo = ProductoNuevoTextBox.Text.Trim();
            string categoriaSeleccionada = ModificarCategoriasComboBox.SelectedValue?.ToString();

            if (string.IsNullOrEmpty(productoAntiguo) || string.IsNullOrEmpty(productoNuevo) || string.IsNullOrEmpty(categoriaSeleccionada))
            {
                MessageBox.Show("Por favor, ingresa el nombre del producto antiguo, el nuevo y selecciona una categoría.");
                return;
            }

            try
            {
                db.OpenConnection();
                string query = "UPDATE products SET ProductName = @productoNuevo, CategoryID = @categoriaID WHERE ProductName = @productoAntiguo";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@productoNuevo", productoNuevo);
                cmd.Parameters.AddWithValue("@categoriaID", categoriaSeleccionada);
                cmd.Parameters.AddWithValue("@productoAntiguo", productoAntiguo);

                int filasAfectadas = cmd.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto modificado correctamente.");
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("No se encontró el producto a modificar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el producto: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            string nuevoProducto = ProductoTextBox.Text;
            string categoriaSeleccionada = CategoriasComboBox.SelectedValue?.ToString();

            if (string.IsNullOrEmpty(nuevoProducto) || string.IsNullOrEmpty(categoriaSeleccionada))
            {
                MessageBox.Show("Debes ingresar un producto y seleccionar una categoría.");
                return;
            }

            try
            {
                db.OpenConnection();
                string query = "INSERT INTO products (ProductName, CategoryID) VALUES (@nombre, @categoria)";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@nombre", nuevoProducto);
                cmd.Parameters.AddWithValue("@categoria", categoriaSeleccionada);

                int filasAfectadas = cmd.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto agregado correctamente.");
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("Error al agregar el producto.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void CargarProductosBorrar()
        {
            try
            {
                db.OpenConnection();
                string query = "SELECT ProductName, CategoryID FROM products";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ProductosBorrarDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void CargarCategoriasBorrar()
        {
            try
            {
                db.OpenConnection();
                string query = "SELECT CategoryID, CategoryName FROM categories";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                CategoriasBorrarDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void BtnBorrarProducto_Click(object sender, RoutedEventArgs e)
        {
            string productoAEliminar = ProductoBorrarTextBox.Text.Trim();

            if (string.IsNullOrEmpty(productoAEliminar))
            {
                MessageBox.Show("Por favor, ingresa el nombre del producto a eliminar.");
                return;
            }

            try
            {
                db.OpenConnection();
                string query = "DELETE FROM products WHERE ProductName = @nombre";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@nombre", productoAEliminar);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto eliminado correctamente.");
                CargarProductosBorrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar producto: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}
