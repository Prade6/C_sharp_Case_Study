using Asset_management.Model;
using Asset_management.MyException;
using Asset_management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Service
{
    internal class employeeservice:Iemployeeservice
    {
        readonly Iemployeerepository _employee;
        readonly ILogging_info _logging;
        public employeeservice()
        {

            _employee = new employeerepository();
            _logging = new Logging_info();

        }

        public void deleteemployee()
        {
            try
            {
                Console.WriteLine("Enter employee id:");
                int id = int.Parse(Console.ReadLine());

                bool check = _employee.deleteemployee(id);

                if (check)
                {
                    Console.WriteLine("Deleted successfuly");
                }

                else
                {
                    Console.WriteLine("Not deleted ");
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void updateemployees()
        {start:
            try
            {

                Console.WriteLine("Enter employee id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 1)
                {
                    throw new DataInvalidException("Employee id can't be negative");
                }
                Console.WriteLine("Enter new password:");
                string pass = Console.ReadLine();
                bool status = _logging.password(pass);
                //email
                if(status)
                {
                    bool check = _employee.updateemployee(id, pass);

                    if (check)
                    {
                        Console.WriteLine("Updated successfuly");
                    }

                    else
                    {
                        Console.WriteLine("Not updated ");
                    }
                }
                else
                {
                    goto start;
                }
              

            }

            catch (DataInvalidException e)
            {
                Console.WriteLine(e.Message);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



        }

        public void viewemployees()
        {
            try
            {
                List<Employees> employee = _employee.viewemployees();

                if (employee.Count() > 0)
                {

                    foreach (Employees a in employee)
                    {
                        Console.WriteLine(a);
                    }
                }
                else
                {
                    Console.WriteLine("No details available");
                }

                }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


    }
}
