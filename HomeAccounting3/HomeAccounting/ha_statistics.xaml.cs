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

        public ha_statistics()
        {
            
            InitializeComponent();
        }

        private void ha_statistics_Loaded(object sender, RoutedEventArgs e)
        {
            Select();
        }

        //Load data from posgre db func
        public void Select()
        {
            //месяца для таблицы
            Dictionary<int, string> allMonthsNames = new Dictionary<int, string>();
            allMonthsNames.Add(1, "Январь");
            allMonthsNames.Add(2, "Февраль");
            allMonthsNames.Add(3, "Март");
            allMonthsNames.Add(4, "Апрель");
            allMonthsNames.Add(5, "Май");
            allMonthsNames.Add(6, "Июнь");
            allMonthsNames.Add(7, "Июль");
            allMonthsNames.Add(8, "Август");
            allMonthsNames.Add(9, "Сентябрь");
            allMonthsNames.Add(10, "Октябрь");
            allMonthsNames.Add(11, "Ноябрь");
            allMonthsNames.Add(12, "Декабрь");


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


            //тут получаю данные для column
            List<string> colname = new List<string>();
            try
            {
                connection.Open();

                sql = $"select name from homeAccounting.NameCategory;";
                cmd = new NpgsqlCommand(sql, connection);
                reader = cmd.ExecuteReader();


                // тут узнаю сколько будет колонок в datagrid
                colname.Add("Месяц");
                while (reader.Read())
                {
                    colname.Add(reader[0].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            finally
            {
                connection.Close();
            }


            // List<DataGridTextColumn> columns = new List<DataGridTextColumn>();
            // в table создаю колонки
            DataTable table = new DataTable();
            //table.Columns.Add("Месяц");
            foreach (var item in colname)
            {
                table.Columns.Add(item);
            }



            // создаю строки для table
            int monthCount = 12;
            List<Dictionary<string, string>> pairsRowMonthsInfo = new List<Dictionary<string, string>>();

            // тут добавляю месяца
            for (int i = 0; i < allMonthsNames.Count; i++)
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                pairs.Add("Месяц", allMonthsNames[(i + 1)]);
                pairsRowMonthsInfo.Add(pairs);
            }


            // тут добавляю остальную информацию по категориям
            for (int i = 0; i < monthCount; i++)
            {
                foreach (var item in colname)
                {
                    if (item != "Месяц")
                        pairsRowMonthsInfo[i].Add(item, "0");
                }
            }


            // тут добавляю остальную значения по категориям из бд
            for (int i = 0; i < monthCount; i++)
            {
                connection.Open();
                sql = $"select * from test_select({i + 1},2020);";
                cmd = new NpgsqlCommand(sql, connection);
                reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //i-1 потому, что отчет с нуля

                    pairsRowMonthsInfo[i][reader[0].ToString()] = reader[1].ToString();
                }
                connection.Close();
            }


            // тут уже добавляю в table строки
            List<DataRow> rows = new List<DataRow>();

            foreach (var pairsRowMonth in pairsRowMonthsInfo)
            {
                DataRow row = table.NewRow();
                foreach (var item in pairsRowMonth)
                {
                    row[item.Key] = item.Value;
                }
                rows.Add(row);
            }

            foreach (var tmpRow in rows)
            {
                table.Rows.Add(tmpRow);
            }


            // загружаю все в DataGrid
            dg.ItemsSource = table.DefaultView;

            connection.Close();

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
