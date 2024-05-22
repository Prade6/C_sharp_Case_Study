using Asset_management.Model;
using Asset_management.MyException;
using Asset_management.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    internal class employeerepository:Iemployeerepository
    {
        SqlConnection sqlConnection = null;
        SqlCommand sqlCommand = null;


        
        public employeerepository()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();

        }

        Logindetail_info logginginfo = new Logindetail_info();


        //To delete employee account
        public bool Deleteemployee(int id)
        {
            bool check = false;
            bool exists = logginginfo.Empoloyeeexists(id);      //Check if employee exists

            try
            {
                sqlCommand.Parameters.Clear();
                if (exists)
                {
                    Deleteemployeeassetreservation(id);
                    Deleteemployeeallocation(id);

                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "delete from employees where employee_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();

                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;

                    }           

                }
                else
                {
                    throw new EmployeenotfoundException("\nEmployee id not available\n");
                }
            }
            catch (EmployeenotfoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
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

        //Delete from allocation
        public bool Deleteemployeeallocation(int id)
        {
            bool check = false;
            bool exists = logginginfo.Empoloyeeexists(id);      //Check if employee exists

            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "delete from asset_allocations where employee_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();

                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }
                }
                else
                {
                    throw new EmployeenotfoundException("\nEmployee id not available\n");
                }
            }
            catch (EmployeenotfoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
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


        //Delete from reservation
        public bool Deleteemployeeassetreservation(int id)
        {
            bool check = false;
            bool exists = logginginfo.Empoloyeeexists(id);      //Check if employee exists

            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.CommandText = "delete from reservations where employee_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();


                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }


                }
                else
                {
                    throw new EmployeenotfoundException("\nEmployee id not available\n");
                }
            }
            catch (EmployeenotfoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
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


        //Update employee details
        public bool Updateemployee(int id, string pass,string email)
        {
            bool update = false;
            bool exists = logginginfo.Empoloyeeexists(id);      //Check if employee exists

            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "update employees set password=@pass,email=@email where employee_id =@id";
                    sqlCommand.Parameters.Add("@pass", SqlDbType.VarChar).Value = pass;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int check = sqlCommand.ExecuteNonQuery();


                    if (check > 0)
                    {
                        update = true;
                    }
                }
                else
                {
                    throw new EmployeenotfoundException("\nEmployee id not available\n");
                }
            }
            catch (EmployeenotfoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            sqlConnection.Close();
            return update;
        }


        //To retrieve all employee details
        public List<employee> Viewemployees()
        {
            List<employee> employees = new List<employee>();
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "select*from employees";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    employee employee = new employee();
                    employee.Employee_id = (int)reader["employee_id"];
                    employee.Name = (string)reader["Name"];
                    employee.Email = (string)reader["email"];
                    employee.Department = (string)reader["department"];
                    employee.Password = (string)reader["Password"];
                    employees.Add(employee);

                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex .Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            sqlConnection.Close();
            return employees;
        }





    
    }
}
