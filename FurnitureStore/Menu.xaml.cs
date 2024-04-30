using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            MenuGrid.ItemsSource = FurnitureEntities.GetContext().MenuItems.ToList();
        }
        private void ButtonToOrder(object sender, RoutedEventArgs e)
        {
            this.Close();
            Order order = new Order();
            order.Show();
            this.Hide();
        }

        private void AddToOrder(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный объект мебели из элемента управления
            MenuItems selectedItem = MenuGrid.SelectedItem as MenuItems;

            if (selectedItem != null)
            {
                // Используем имя мебели для передачи в хранимую процедуру
                string furnitureName = selectedItem.furniture_name;

                // Вызываем метод хранимой процедуры с передачей названия мебели
                Manager manager = new Manager();
                manager.AddToOrder(furnitureName, 1);
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
