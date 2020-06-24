using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.ComponentModel;
using System.Data;

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
            string database = "homeaccountingtest";

            string connection_string = $"Server={server};Port={port};" +
                                 $"User Id={user_id};Password={password};" +
                                 $"Database={database};";

            NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            string sql;
            NpgsqlCommand cmd;


            try
            {
                connection.Open();
                
                sql = $"select name from homeAccounting.NameCategory;";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();


                // тут узнаю сколько будет колонок в datagrid
                List<string> colname = new List<string>();

                while (reader.Read())
                {
                    colname.Add(reader[0].ToString());
                }



                DataTable table = new DataTable();
                foreach (var item in colname)
                {
                    table.Columns.Add(item);
                }

                foreach (var item in colname)
                {
                    table.Rows.Add(item);
                }


                foreach (var item in table.Rows)
                {
                    Console.WriteLine(item.ToString());
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
        }
    }
}
