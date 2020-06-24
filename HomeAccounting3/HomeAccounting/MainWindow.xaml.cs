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
        int rb_expense_income_check = 2;


        private string connection_string;
        private NpgsqlConnection connection;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;




        public MainWindow()
        {
            string server = "localhost";
            string port = "5432";
            string user_id = "homeaccountingUser";
            string password = "123";
            string database = "homeaccountingtest";

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
                sql = "select to_char(Дата,'dd-mm-yyyy') as \"Дата\", \"Основная категория\", Категория,  Стоимость, Комментарий, id from mainSelect()";
                cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);

                dt = new DataTable("mainSelect()");
                dataAdapter.Fill(dt);
                dg.ItemsSource = dt.DefaultView;

                connection.Close();


            }
            catch (Exception ex)
            {

                connection.Close();
            }

        }

        
        private void Select_test_tmp()
        {
            /*
            try
            {
                connection = new NpgsqlConnection(connection_string);
                connection.Open();
                sql = "select id, \"Основная категория\", Категория, to_char(Дата,'dd-mm-yyyy') as \"Дата\", Стоимость, Комментарий from mainSelect()";

                if (chb_income.IsChecked == true && chb_expense.IsChecked == true)
                {
                    sql = "select id, \"Основная категория\", Категория, to_char(Дата,'dd-mm-yyyy') as \"Дата\", Стоимость, Комментарий from mainSelect()";
                }
                else if (chb_income.IsChecked == true && chb_expense.IsChecked == false)
                {
                    sql = "select id, \"Основная категория\", Категория, to_char(Дата,'dd-mm-yyyy') as \"Дата\", Стоимость, Комментарий from mainSelect() where main_category = 1";
                }
                else if (chb_expense.IsChecked == true && chb_income.IsChecked == false)
                {
                    sql = "select id, \"Основная категория\", Категория, to_char(Дата,'dd-mm-yyyy') as \"Дата\", Стоимость, Комментарий from mainSelect() where main_category = 2";
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
            
            */
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Data.DataRowView row = (System.Data.DataRowView)dg.SelectedItems[0];
            tb_data.Text = (string)row["Дата"];
            tb_category.Text = (string)row["Категория"];
            string rb_check_income_or_expense = (string)row["Основная категория"];
            if (rb_check_income_or_expense == "Доход")
            {
                //MessageBox.Show(rb_income.Content.ToString());
                rb_income.IsChecked = true;
            }
            else if (rb_check_income_or_expense == "Расход")
            {
                rb_expense.IsChecked = true;
            }
            decimal tmp_sum = (decimal)row["Стоимость"];
            tb_sum.Text = tmp_sum.ToString();
            tb_comment.Text = (string)row["Комментарий"];
        }

        private void chb_income_Checked(object sender, RoutedEventArgs e)
        {
            //Select_test_tmp();
        }

        private void chb_expense_Checked(object sender, RoutedEventArgs e)
        {
            //Select_test_tmp();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ha_statistics statistics = new ha_statistics();
            statistics.Owner = this;
            statistics.Show();
            (Application.Current as App).Statistics.Add(statistics);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ha_add_new_entry new_entry = new ha_add_new_entry();
            new_entry.Owner = this;
            new_entry.Show();
            (Application.Current as App).New_entries.Add(new_entry);
        }

        private void Delete_Data_Button_Click(object sender, RoutedEventArgs e)
        {
            // подключение к бд(postgre)


            try
            {
                System.Data.DataRowView row = (System.Data.DataRowView)dg.SelectedItems[0];

                connection.Open();
                NpgsqlCommand cmdTmp;
                string sql = $"delete from homeAccounting.Entry where id = {row["id"].ToString()}";
                cmdTmp = new NpgsqlCommand(sql, connection);
                cmdTmp.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                connection.Close();
            }
            MessageBox.Show("Запись удалена");
            Select();

        }

        private void Upload_Data_Button_Click(object sender, RoutedEventArgs e)
        {


            //тут получаю данные для поля категории
            Dictionary<string, string> categories = new Dictionary<string, string>();

            try
            {
                connection.Open();

                sql = $"select id, name from homeAccounting.NameCategory;";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();


                // тут загружаю категории в categories

                while (reader.Read())
                {
                    if (!categories.ContainsKey(reader[1].ToString()))
                    {
                        categories.Add(reader[1].ToString(), reader[0].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                connection.Close();
            }


            // Update к бд(postgre)

            try
            {
                System.Data.DataRowView row = (System.Data.DataRowView)dg.SelectedItems[0];

                connection.Open();
                NpgsqlCommand cmdTmp;

                //проверка колонки деньги на минус
                //check_money_collunm_to_sign();


                    string sql = $"update homeAccounting.Entry set main_category = {rb_expense_income_check.ToString()}, name_category = {categories[tb_category.Text]}, date = to_date('{tb_data.Text}', 'dd-mm-yyyy'), cost = {tb_sum.Text}, comment = '{tb_comment.Text}' where id = {row["id"].ToString()};";
                cmdTmp = new NpgsqlCommand(sql, connection);
                cmdTmp.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                connection.Close();
            }
            MessageBox.Show("Запись Обновлена");
            Select();

        }

        //проверка колонки на минус
        private void check_money_collunm_to_sign()
        {
            //проверка колонки на минус
            if (tb_sum.Text.StartsWith("-") && rb_expense_income_check == 1)
            {
                string tmp = "-" + tb_sum.Text;
                tb_sum.Text = tmp;
            }
            else if (!(tb_sum.Text.StartsWith("-")) && rb_expense_income_check == 2)
            {
                string tmp =  tb_sum.Text.Substring(1);
                tb_sum.Text = tmp;
            }
        }
        

        private void rb_expense_Checked(object sender, RoutedEventArgs e)
        {
            rb_expense_income_check = 2;
        }

        private void rb_income_Checked(object sender, RoutedEventArgs e)
        {
            rb_expense_income_check = 1;
        }

        private void reload_Data_Button_Click(object sender, RoutedEventArgs e)
        {
            Select();
        }
    }
}
