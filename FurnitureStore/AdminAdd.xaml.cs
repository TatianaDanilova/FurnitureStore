using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AdminAdd.xaml
    /// </summary>
    public partial class AdminAdd : Window
    {
        public AdminAdd()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=desktop-4hjv1j2;Initial Catalog=Furniture;Integrated Security=True";

        private void AddClick(object sender, RoutedEventArgs e)
        {
            // Добавление пользователя в бд
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("AddMenuItem", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.NVarChar, 100)).Value = NameBox.Text;
                    cmd.Parameters.Add(new SqlParameter("@price", System.Data.SqlDbType.Int)).Value = Convert.ToInt32(PriceBox.Text);

                    cmd.ExecuteNonQuery();
                }
                try
                {
                    MessageBox.Show("Вы успешно добавили позицию в каталог.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");

                }
            }
            AdminMenu menu = new AdminMenu();
            menu.Show();
            this.Hide();
        }
    }
}
