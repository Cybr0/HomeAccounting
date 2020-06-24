using Npgsql;
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
using System.Windows.Shapes;

namespace HomeAccounting
{
    /// <summary>
    /// Логика взаимодействия для ha_add_new_entry.xaml
    /// </summary>
    public partial class ha_add_new_entry : Window
    {
        int rb_main_category = 2;
        public ha_add_new_entry()
        {
            InitializeComponent();
        }

        public void SetContent(string content)
        {
            this.Content = content;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (Application.Current as App).New_entries.Remove(this);
        }

        private void Close_window_Button_Click(object sender, RoutedEventArgs e)
        {
            //todo
            Visibility = Visibility.Collapsed;
        }

        //TODO
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // подключение к бд(postgre) start
            string server = "localhost";
            string port = "5432";
            string user_id = "postgres";
            string password = "123";
            string database = "homeaccountingtest";
            string connection_string = $"Server={server};Port={port};" +
                             $"User Id={user_id};Password={password};" +
                             $"Database={database};";

            NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            string sql;
            NpgsqlCommand cmd;
            NpgsqlDataReader reader;
            // подключение к бд(postgre) end


            //тут получаю данные для поля категории
            Dictionary<string, string> categories = new Dictionary<string, string>();
            try
            {
                connection.Open();

                sql = $"select id, name from homeAccounting.NameCategory;";
                cmd = new NpgsqlCommand(sql, connection);
                reader = cmd.ExecuteReader();


                // тут загружаю категории в colname

                while (reader.Read())
                {
                    if (!categories.ContainsKey(reader[1].ToString()))
                    {
                        categories.Add(reader[1].ToString(), reader[0].ToString());
                    }

                }

                foreach (var item in categories)
                {
                    Console.WriteLine(item.Key + "  " + item.Value);
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                connection.Close();
            }

            // тут проверка наличия категории, если нет то добавляем
            if (categories.ContainsKey(tb_category.Text))
            {
                Console.WriteLine("да есть");
                Console.WriteLine(categories[cat]);
            }
            else
            {
                try
                {
                    Console.WriteLine(categories.Count);
                    connection.Open();
                    sql = $"select * from nameCategory_insert({rb_main_category}, '{cat}');";
                    NpgsqlCommand cmd2 = new NpgsqlCommand(sql, connection);
                    cmd2.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void rb_expense_Checked(object sender, RoutedEventArgs e)
        {
            rb_main_category = 2;
        }
        private void rb_income_Checked(object sender, RoutedEventArgs e)
        {
            rb_main_category = 1;
        }
    }
}
