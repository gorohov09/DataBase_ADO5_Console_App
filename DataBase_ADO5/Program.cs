using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

#region User Manual
    /* User Manual
     1. Выход exit
     
     */
#endregion

namespace DataBase_ADO5
{
    class Program
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;

        private static SqlConnection sqlConnection = null;

        static void SelectDateBase(SqlDataReader sqlDataReader)
        {
            while (sqlDataReader.Read())
            {
                Console.WriteLine($"{sqlDataReader["Id"]} {sqlDataReader["FIO"]} {sqlDataReader["Birthday"]}" +
                    $" {sqlDataReader["University"]} {sqlDataReader["GroupNumber"]} {sqlDataReader["Course"]}" +
                    $" {sqlDataReader["AverageScore"]}");

                Console.WriteLine(new string('-', 30));
            }

            if (sqlDataReader != null)
            {
                sqlDataReader.Close();
            }
        }

        static void Main(string[] args)
        {
            sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            Console.WriteLine("StudentsApp");

            SqlDataReader sqlDataReader = null;

            string command = string.Empty;

            while (true)
            {
                try
                {
                    Console.Write("> ");
                    command = Console.ReadLine();

                    #region Exit
                    if (command.ToLower().Equals("exit"))
                    {
                        if (sqlConnection.State == ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }

                        if (sqlDataReader != null)
                        {
                            sqlDataReader.Close();
                        }

                        break;
                    }
                    #endregion

                    SqlCommand sqlCommand = null;

                    string[] commandArray = command.ToLower().Split(' ');

                    switch (commandArray[0])
                    {
                        case "selectall":
                            sqlCommand = new SqlCommand("SELECT * FROM Student", sqlConnection);

                            sqlDataReader = sqlCommand.ExecuteReader();

                            SelectDateBase(sqlDataReader);
                            break;

                        case "select":

                            sqlCommand = new SqlCommand(command, sqlConnection);

                            sqlDataReader = sqlCommand.ExecuteReader();

                            SelectDateBase(sqlDataReader);
                            break;

                        case "insert":
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            Console.WriteLine($"Добавлено: {sqlCommand.ExecuteNonQuery()} строк");
                            break;

                        case "update":
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            Console.WriteLine($"Изменено: {sqlCommand.ExecuteNonQuery()} строк");
                            break;

                        case "delete":
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            Console.WriteLine($"Удалено: {sqlCommand.ExecuteNonQuery()} строк");
                            break;

                        case "sortby":
                            //sortby fio asc
                            sqlCommand = new SqlCommand($"SELECT * FROM Student ORDER BY {commandArray[1]} {commandArray[2]}", sqlConnection);
                            sqlDataReader = sqlCommand.ExecuteReader();
                            SelectDateBase(sqlDataReader);
                            break;

                        default:
                            Console.WriteLine($"Команда {command} некорректна!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("Для продолжения нажмите любую клавишу ...");
            Console.ReadKey();

        }
    }
}
