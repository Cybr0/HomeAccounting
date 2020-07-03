using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting
{
    class ComboBoxViewModel
    {
        public Dictionary<string, string> categoryNameForComboBox;
        public Dictionary<string, string> incomeCategoryNameForComboBox;
        public Dictionary<string, string> expenseCategoryNameForComboBox;
        public List<string> CategoryNameCollection { get; set; }
        public List<string> IncomeCategoryNameCollection { get; set; }
        public List<string> ExpenseCategoryNameCollection { get; set; }
        
        public ComboBoxViewModel()
        {
            incomeCategoryNameForComboBox = new Dictionary<string, string>();
            expenseCategoryNameForComboBox = new Dictionary<string, string>();
            CategoryNameCollection = new List<string>();
            IncomeCategoryNameCollection = new List<string>();
            ExpenseCategoryNameCollection = new List<string>();
            InitializerMethod();

            foreach (var item in categoryNameForComboBox)
            {
                CategoryNameCollection.Add(item.Key);
            }

            IncomeCategoryNameCollection.Add("Другое");
            foreach (var item in incomeCategoryNameForComboBox)
            {
                IncomeCategoryNameCollection.Add(item.Key);
            }
            

            foreach (var item in expenseCategoryNameForComboBox)
            {
                ExpenseCategoryNameCollection.Add(item.Key);
            }

        }
        
        private void InitializerMethod()
        {
            // подключение к бд(postgre) start
            string server = "localhost";
            string port = "5432";
            string user_id = "homeaccountingUser";
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

                sql = $"select id, name, main_category from homeAccounting.NameCategory;";
                cmd = new NpgsqlCommand(sql, connection);
                reader = cmd.ExecuteReader();


                // тут загружаю категории в colname

                while (reader.Read())
                {
                    if (!categories.ContainsKey(reader[1].ToString()))
                    {
                        categories.Add(reader[1].ToString(), reader[0].ToString());
                        //проверка катерогии на доходы
                        if (reader[2].ToString() == "1")
                        {
                            incomeCategoryNameForComboBox.Add(reader[1].ToString(), reader[0].ToString());
                        }
                        //иначе катерогия расходы
                        else
                        {
                            expenseCategoryNameForComboBox.Add(reader[1].ToString(), reader[0].ToString());
                        }
                    }

                }
                categoryNameForComboBox = new Dictionary<string, string>(categories);
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
}
