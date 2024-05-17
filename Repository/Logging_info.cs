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
    internal class Logging_info:ILogging_info
    {
        SqlConnection sqlConnection = null;
        SqlCommand sqlCommand = null;

        public Logging_info()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();
        }
        public int register(string name, string dept, string email, string password)
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
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }

        public bool logging(int id, string pass)
        {
            bool check = false;
            bool idexists=Empoloyeeexists(id);

                if (idexists)
                {
                    try
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
                        sqlConnection.Close();
                       

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            
        
            return check;
        }

        public bool Empoloyeeexists(int id)
        {
            bool status = false;
            try
            {
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
                if(!status)
                {
                    throw new EmployeenotfoundException("\nEmployee id not found\n");
                }
            }
            catch(EmployeenotfoundException e)
            {
                Console.WriteLine(e.Message);
            }
           
            return status;
        }


        public bool password(string password)
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
                            Console.WriteLine("Should contain special charcter");
                        }
                    }
                    else
                    {

                        Console.WriteLine("Should contain uppercase");

                    }
                }
                else
                {
                    Console.WriteLine("Should contain digit");
                }

            }
            else
            {
                Console.WriteLine("Length should be atleast 8");
            }
            return check;
        }
    }
}
