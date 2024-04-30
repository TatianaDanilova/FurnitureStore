using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FurnitureStore
{
    internal class Manager
    {
        public void AddToOrder(string dish_name, int quantity)
        {
            string connectionString = "Data Source=desktop-4hjv1j2;Initial Catalog=Furniture;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("AddOrderItem", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.NVarChar, 100));
                    cmd.Parameters.Add(new SqlParameter("@quantity", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@client_id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@name"].Value = dish_name;
                    cmd.Parameters["@quantity"].Value = quantity;
                    cmd.Parameters["@client_id"].Value = AppSettings.UserId;

                    cmd.ExecuteNonQuery();
                }
                try
                {
                    MessageBox.Show("Позиция успешно добавлена в заказ.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");

                }

            }
        }
    }
}
