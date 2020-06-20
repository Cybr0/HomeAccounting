using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ConsoleAppTest
{
    class Program
    {

        
        static void Main(string[] args)
        {
            string server = "localhost";
            string port = "5432";
            string user_id = "postgres";
            string password = "123";
            string database = "homeaccounting";

            string connection_string = $"Server={server};Port={port};" +
                                 $"User Id={user_id};Password={password};" +
                                 $"Database={database};";

            NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            string sql;
            NpgsqlCommand cmd;

           

            try
            {
                connection.Open();
                sql = @"select * from Categories;";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Console.WriteLine(reader.GetName(5));
                Console.WriteLine(reader[5]);
                //while (reader.Read())
                //{
                //    for (int i = 0; i < reader.FieldCount; i++)
                //    {
                //        Console.WriteLine(reader.GetName(i) + ": " + reader[i]);
                //    }
                //}

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
