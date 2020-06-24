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
using System.Data;
using System.ComponentModel;

namespace HomeAccounting
{
    /// <summary>
    /// Логика взаимодействия для ha_statistics.xaml
    /// </summary>
    public partial class ha_statistics : Window
    {
        private string connection_string;
        private NpgsqlConnection connection;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        public ha_statistics()
        {
            string server = "localhost";
            string port = "5432";
            string user_id = "postgres";
            string password = "123";
            string database = "homeaccountingtest";

            connection_string = $"Server={server};Port={port};" +
                                 $"User Id={user_id};Password={password};" +
                                 $"Database={database};";

            InitializeComponent();
        }

        private void ha_statistics_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new NpgsqlConnection(connection_string);
            Select();
        }

        private void Select()
        {
            try
            {
                connection.Open();
                sql = $"select * from test_select({4},2020);";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();


                //int i = 0;
                BindingList<Dictionary<string, string>> months = new BindingList<Dictionary<string, string>>();
                Dictionary<string,string> month = new Dictionary<string, string>();
                while (reader.Read())
                {
                   
                    string key = reader[0].ToString();
                    string value = reader[1].ToString();
                    month.Add(key, value);
                }
                months.Add(month);
                dg.ItemsSource = month;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                
            }
            finally
            {
                connection.Close();
            }

        }

        public void SetContent(string content)
        {
            this.Content = content;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (Application.Current as App).Statistics.Remove(this);
        }
    }
}
