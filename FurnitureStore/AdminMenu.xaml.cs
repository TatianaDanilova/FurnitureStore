using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace FurnitureStore
{
    /// <summary>
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        string connectionString = "Data Source=desktop-4hjv1j2;Initial Catalog=Furniture;Integrated Security=True";

        public AdminMenu()
        {
            InitializeComponent();
            LoadMenuDetails();
        }

        private void LoadMenuDetails()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("GetMenuDetails", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    MenuGrid.ItemsSource = dataTable.DefaultView;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void ButtonToDelete(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = (DataRowView)((Button)e.Source).DataContext;

            string caption = "";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Удалить позицию из меню?", caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DeleteOrderItem(rowView);
                LoadMenuDetails();
                MessageBox.Show("Позиция удалена из меню.");
            }
        }
        private void ButtonToAdd(object sender, RoutedEventArgs e)
        {
            this.Close();
            AdminAdd add = new AdminAdd();
            add.Show();
            this.Hide();
        }

        private void DeleteOrderItem(DataRowView rowView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DeleteMenuItem", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@furniture_name", System.Data.SqlDbType.NVarChar, 100));
                    cmd.Parameters["@furniture_name"].Value = rowView["Название"];

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ButtonToExit(object sender, RoutedEventArgs e)
        {
            string caption = "";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из профиля?", caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
