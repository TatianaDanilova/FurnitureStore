using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        string connectionString = "Data Source=desktop-4hjv1j2;Initial Catalog=Furniture;Integrated Security=True";

        public Order()
        {
            InitializeComponent();
            LoadOrderDetails();

        }
        private void LoadOrderDetails()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("GetOrderDetails", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@client_id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@client_id"].Value = AppSettings.UserId;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    OrderDetailsGrid.ItemsSource = dataTable.DefaultView;
                    cmd.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("CalculateTotalCost", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@client_id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@client_id"].Value = AppSettings.UserId;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    TotalCost.ItemsSource = dataTable.DefaultView;
                    cmd.ExecuteNonQuery();
                }
            }
        }



        private void ButtonToMenu(object sender, RoutedEventArgs e)
        {
            this.Close();
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

        private void Break(object sender, RoutedEventArgs e)
        {
            string caption = "";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Вы уверены в том, что хотите завершить заказ?", caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Вы успешно завершили заказ! До встречи!");
                System.Windows.Application.Current.Shutdown();

            }
        }


        private void PlusQuantityClick(object sender, RoutedEventArgs e)
        {
            // Получаем текущую строку в DataGrid, на которой находится кнопка
            DataRowView rowView = (DataRowView)((Button)e.Source).DataContext;

            // Получаем текущее значение количества порций из TextBox
            int currentQuantity = Convert.ToInt32(rowView["Количество"]);

            // Уменьшаем количество порций на 1
            currentQuantity++;

            // Обновляем значение количества порций в TextBox
            rowView["Количество"] = currentQuantity;

            // Обновляем значение количества порций в таблице OrderItems
            UpdateQuantityInDatabase(currentQuantity, rowView);

            LoadOrderDetails();
        }

        private void MinusQuantityButtonClick(object sender, RoutedEventArgs e)
        {
            // Получаем текущую строку в DataGrid, на которой находится кнопка
            DataRowView rowView = (DataRowView)((Button)e.Source).DataContext;

            // Получаем текущее значение количества порций из TextBox
            int currentQuantity = Convert.ToInt32(rowView["Количество"]);

            if (currentQuantity > 1)
            {
                // Уменьшаем количество порций на 1
                currentQuantity--;

                // Обновляем значение количества порций в TextBox
                rowView["Количество"] = currentQuantity;

                // Обновляем значение количества порций в таблице OrderItems
                
                UpdateQuantityInDatabase(currentQuantity, rowView);

                LoadOrderDetails();
            }
            else
            {
                string caption = "";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show("Удалить позицию из заказа?", caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteOrderItem(rowView);
                    LoadOrderDetails();
                    MessageBox.Show("Позиция удалена из заказа.");
                }
                else
                {
                    currentQuantity = 1;
                }
               
            }
        }

        private void UpdateQuantityInDatabase(int newQuantity, DataRowView rowView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("UpdateOrderItemQuantity", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@furniture_name", System.Data.SqlDbType.NVarChar, 100));
                    cmd.Parameters.Add(new SqlParameter("@newQuantity", System.Data.SqlDbType.Int));
                    cmd.Parameters["@furniture_name"].Value = rowView["Название"];
                    cmd.Parameters["@newQuantity"].Value = newQuantity;

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void DeleteOrderItem(DataRowView rowView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DeleteOrderItem", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@furniture_name", System.Data.SqlDbType.NVarChar, 100));
                    cmd.Parameters["@furniture_name"].Value = rowView["Название"];

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}