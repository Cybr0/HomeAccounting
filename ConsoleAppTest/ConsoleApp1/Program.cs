using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
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

            //////////////пример
            string cat = "нет";

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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            finally
            {
                connection.Close();
            }

            // тут проверка наличия категории, если нет то добавляем
            if (categories.ContainsKey(cat))
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
                    sql = $"select * from nameCategory_insert(1, '{cat}');";
                    NpgsqlCommand cmd2 = new NpgsqlCommand(sql, connection);
                    cmd2.ExecuteNonQuery();
                    
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }
    }
}
