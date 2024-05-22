using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asset_management.Utility;
using Asset_management.MyException;

namespace Asset_management.Repository
{
    public class Logindetail_info:ILogindetail_info
    {
        SqlConnection sqlConnection = null;
        SqlCommand sqlCommand = null;

        public Logindetail_info()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();
        }

        //To add new employee
        public int Register(string name, string dept, string email, string password)
        {
            int check = 0;
            try
            {
                sqlCommand.CommandText = "insert into employees values(@name,@dept,@email,@pass)";
                sqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                sqlCommand.Parameters.Add("@dept", SqlDbType.VarChar).Value = dept;
                sqlCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                sqlCommand.Parameters.Add("@pass", SqlDbType.VarChar).Value = password;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                check = sqlCommand.ExecuteNonQuery();
               
            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            sqlConnection.Close();

            return check;
        }

        //To login into the application
        public bool Logging(int id, string pass)
        {
            bool check = false;
            bool idexists=Empoloyeeexists(id);                //Check if employee exists    

            try
            {
                if (idexists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "Select*from employees where employee_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        if (pass == (string)sqlDataReader["password"])
                        {
                            check = true;
                        }
                    }
                }
                else
                {
                    throw new EmployeenotfoundException("\nEmployee id not found\n");
                }

            }
            catch (EmployeenotfoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
                
                


            sqlConnection.Close();
            return check;
        }


        //Check if employee exists
        public bool Empoloyeeexists(int id)
        {
            bool status = false;

                sqlCommand.CommandText = "select * from employees";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if ((int)reader["employee_id"] == id)
                    {
                        status = true;
                        break;
                    }

                }

            sqlConnection.Close();
            if (!status)
                {
                    throw new EmployeenotfoundException("\nEmployee id not found\n");
                }

           
            return status;
        }

        //Check if password is valid
        public bool Password(string password)
        {
            string a = password;

            bool digit = a.Any(char.IsDigit);
            bool upper = a.Any(char.IsUpper);
            bool spl = a.Any(char.IsSymbol);
            bool check = false;

            if (a.Length > 6)
            {
                if (digit == true)
                {
                    if (upper == true)
                    {
                        if (a.Contains('#') || a.Contains('@') || a.Contains('&') || a.Contains('*'))
                        {
                            Console.WriteLine("\n");
                            check = true;
                        }
                        else
                        {
                            Console.WriteLine("\nShould contain special charcter\n");
                        }
                    }
                    else
                    {

                        Console.WriteLine("\nShould contain uppercase\n");

                    }
                }
                else
                {
                    Console.WriteLine("\nShould contain digit\n");
                }

            }
            else
            {
                Console.WriteLine("\nLength should be atleast 8\n");
            }
            return check;
        }
    }
}
