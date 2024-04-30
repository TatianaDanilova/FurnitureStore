using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Data.Entity;

namespace FurnitureStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=desktop-4hjv1j2;Initial Catalog=Furniture;Integrated Security=True";

       

        public int CheckName(string login, string password)
        {
            int idUser = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("CheckName", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Добавляем параметры и указываем, что один из них является параметром для вывода
                command.Parameters.Add("@first_name", SqlDbType.NVarChar, 100).Value = login;
                command.Parameters.Add("@pass", SqlDbType.NVarChar, 100).Value = password;
                command.Parameters.Add("@client_id", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();

                object idUserObj = command.Parameters["@client_id"].Value;
                if (idUserObj != DBNull.Value)
                {
                    idUser = Convert.ToInt32(idUserObj);
                }
            }
            return idUser;
        }

        public void Check()
        {
            if (PasswordBox.Password.Contains(" "))
            {
                PasswordBox.BorderBrush = Brushes.Red;
                PasswordBox.ToolTip = "В пароле не должны использоваться пробелы";
            }
            else
            {
                PasswordBox.Background = Brushes.Transparent;
            }
        }



        private void RegestrationClick(object sender, RoutedEventArgs e)
        {

            Check();
            // Все данные введены корректно

            if (PasswordBox.Background == Brushes.Transparent)
            {

                // Пользователь есть в бд
                int userId = CheckName(LoginBox.Text, PasswordBox.Password);
                if (userId != 0)
                {
                    MessageBox.Show("Вы уже зарегестрированы");
                }
                // Пользователя нет в бд
                else
                {
                    // Добавление пользователя в бд
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand("AddClient", connection))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.NVarChar, 100)).Value = LoginBox.Text;
                            cmd.Parameters.Add(new SqlParameter("@pass", System.Data.SqlDbType.NVarChar, 100)).Value = PasswordBox.Password;

                            cmd.ExecuteNonQuery();
                        }
                        try
                        {
                            MessageBox.Show("Вы успешно зарегестрировались");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка: {ex.Message}");

                        }
                    }
                }
            }
        }
        private void EntryClick(object sender, RoutedEventArgs e)
        {
            Check();
            int userId = CheckName(LoginBox.Text, PasswordBox.Password);
            if (userId == -1)
            {
                AdminMenu menu = new AdminMenu();
                menu.Show();
                this.Hide();
            }
            else if (userId != 0)
            {
                AppSettings.UserId = userId;

                Menu menu = new Menu();
                menu.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
