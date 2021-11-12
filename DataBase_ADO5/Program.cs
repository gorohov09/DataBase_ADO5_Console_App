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

                        case "search":

                            if (commandArray[1].Equals("fio"))
                            {
                                sqlCommand = new SqlCommand($"SELECT * FROM Student WHERE FIO LIKE N'%{commandArray[2]}%'", sqlConnection);
                            }
                            else if (commandArray[1].Equals("birthday"))
                            {
                                sqlCommand = new SqlCommand($"SELECT * FROM Student WHERE Birthday = '{commandArray[2]}'", sqlConnection);
                            }
                            else
                            {
                                Console.WriteLine($"Аргумент {commandArray[1]} некорректен!");
                            }

                            try
                            {
                                sqlDataReader = sqlCommand.ExecuteReader();
                                SelectDateBase(sqlDataReader);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка: {ex.Message}");
                            }
                            finally
                            {
                                if (sqlDataReader != null)
                                {
                                    sqlDataReader.Close();
                                }
                            }

                            break;

                        case "clear":
                            Console.Clear();
                            break;

                        case "min":
                            sqlCommand = new SqlCommand("SELECT MIN(AverageScore) FROM Student", sqlConnection);
                            Console.WriteLine($"Минимальный средний балл: {sqlCommand.ExecuteScalar()}");
                            break;
                        case "max":
                            sqlCommand = new SqlCommand("SELECT MAX(AverageScore) FROM Student", sqlConnection);
                            Console.WriteLine($"Максимальный средний балл: {sqlCommand.ExecuteScalar()}");
                            break;
                        case "avg":
                            sqlCommand = new SqlCommand("SELECT AVG(AverageScore) FROM Student", sqlConnection);
                            Console.WriteLine($"Средний балл: {sqlCommand.ExecuteScalar()}");
                            break;
                        case "sum":
                            sqlCommand = new SqlCommand("SELECT SUM(AverageScore) FROM Student", sqlConnection);
                            Console.WriteLine($"Суммарный балл: {sqlCommand.ExecuteScalar()}");
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
