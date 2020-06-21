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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace HomeAccounting
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connection_string;
        private NpgsqlConnection connection;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;




        public MainWindow()
        {
            string server = "localhost";
            string port = "5432";
            string user_id = "postgres";
            string password = "123";
            string database = "homeaccounting";

            connection_string = $"Server={server};Port={port};" +
                                 $"User Id={user_id};Password={password};" +
                                 $"Database={database};";
            InitializeComponent();
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new NpgsqlConnection(connection_string);
            Select();
        }

        private void Select()
        {
            try
            {
                connection.Open();
                sql = @"select * from Categories";
                cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);

                dt = new DataTable("Categories");
                dataAdapter.Fill(dt);
                dg.ItemsSource = dt.DefaultView;

                connection.Close();
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                connection.Close();
            }
           
        }

        private void Select_test_tmp()
        {
            try
            {
                connection = new NpgsqlConnection(connection_string);
                connection.Open();
                sql = @"select * from Categories where c_category = 'ashhsa'";

                if (chb_income.IsChecked == true && chb_expense.IsChecked == true)
                {
                    sql = @"select * from Categories";
                }
                else if (chb_income.IsChecked == true)
                {
                    sql = @"select * from Categories where c_category like '%income%'";
                }
                else if (chb_expense.IsChecked == true)
                {
                    sql = @"select * from Categories where c_category like '%out%'";
                }

                cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);

                dt = new DataTable("Categories");
                dataAdapter.Fill(dt);
                dg.ItemsSource = dt.DefaultView;

                connection.Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                connection.Close();
            }

        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Data.DataRowView row = (System.Data.DataRowView)dg.SelectedItems[0];
            tb_data.Text = (string)row["c_date"];
            tb_category.Text = (string)row["c_name"];
            string rb_check_income_or_expense = (string)row["c_category"];
            if (rb_check_income_or_expense == "income")
            {
                //MessageBox.Show(rb_income.Content.ToString());
                rb_income.IsChecked = true;
            }
            else if(rb_check_income_or_expense == "out")
            {
                rb_expense.IsChecked = true;
            }
            int tmp_sum = (int)row["c_sum"];
            tb_sum.Text = tmp_sum.ToString();
            tb_comment.Text = (string)row["c_comment"];
        }

        private void dg_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void rb_expense_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rb_income_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void chb_income_Checked(object sender, RoutedEventArgs e)
        {
            Select_test_tmp();
        }

        private void chb_expense_Checked(object sender, RoutedEventArgs e)
        {
           // Select_test_tmp();
        }
    }
}
