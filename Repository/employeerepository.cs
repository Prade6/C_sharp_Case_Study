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

        public bool deleteemployee(int id)
        {
            bool check = false;
            bool exists = employeeexists(id);

            try
            {
                sqlCommand.Parameters.Clear();
                if (exists)
                {
                    sqlCommand.CommandText = "delete from employees where employee_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();

                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                        //deleteallocation(id);
                        //deletemaintenance(id);
                        //deleteassetreservation(id);
                    }
            

                }
                else
                {
                    throw new EmployeenotfoundException("Employee id not available");
                }
            }
            catch (EmployeenotfoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            sqlConnection.Close();
            return check;
        }

        public bool updateemployee(int id, string pass)
        {
            bool update = false;
            bool exists = employeeexists(id);

            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "update employees set password=@pass where employee_id =@id";
                    sqlCommand.Parameters.Add("@pass", SqlDbType.VarChar).Value = pass;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
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
                    throw new EmployeenotfoundException("Employee id not available");
                }
            }
            catch (EmployeenotfoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            sqlConnection.Close();
            return update;
        }

        public List<Employees> viewemployees()
        {
            List<Employees> employees = new List<Employees>();
            try
            {
                sqlCommand.CommandText = "select*from employees";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Employees employee = new Employees();
                    employee.Employee_id = (int)reader["employee_id"];
                    employee.Name = (string)reader["Name"];
                    employee.Email = (string)reader["email"];
                    employee.Password = (string)reader["Password"];
                    employees.Add(employee);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sqlConnection.Close();
            return employees;
        }

        public bool employeeexists(int id)
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
                if (!status)
                {
                    throw new EmployeenotfoundException("\nEmployee id not found\n");
                }
            }
            catch (EmployeenotfoundException e)
            {
                Console.WriteLine(e.Message);
            }

            return status;
        }

        //public int addemployee(Employees employees)
        //{
        //    int check = 0;
        //    try
        //    {
        //        sqlCommand.Parameters.Clear();
        //        sqlCommand.CommandText = "insert into employees values(@name,@dept,@email,@pass)";
        //        sqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = employees.Name;
        //        sqlCommand.Parameters.Add("@dept", SqlDbType.VarChar).Value = employees.Department;
        //        sqlCommand.Parameters.Add("@email", SqlDbType.DateTime).Value = employees.Email;
        //        sqlCommand.Parameters.Add("@pass", SqlDbType.Int).Value = employees.Password;
        //        sqlCommand.Connection = sqlConnection;
        //        sqlConnection.Open();
        //        check = sqlCommand.ExecuteNonQuery();
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    sqlConnection.Close();
        //    return check;
        //}

    
    }
}
