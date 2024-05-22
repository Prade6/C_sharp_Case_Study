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
        readonly ILogindetail_info _logging;
        public employeeservice()
        {

            _employee = new employeerepository();
            _logging = new Logindetail_info();

        }

        public void Deleteemployee()
        {start:
            try
            {
                Console.WriteLine("Enter employee id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 1)
                {
                    throw new DataInvalidException("\nEmployee id can't be negative\n");
                }
                bool check = _employee.Deleteemployee(id);

                if (check)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nEmployee detail deleted successfuly\n");
                    Console.ResetColor();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEmployee detail not deleted\n");
                    Console.ResetColor();
                }

            }
            catch (DataInvalidException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
                goto start;
            }

            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

        }

        public void Updateemployees()
        {start:
            try
            {

                Console.WriteLine("Enter employee id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 1)
                {
                    throw new DataInvalidException("\nEmployee id can't be negative\n");
                }
                Console.WriteLine("Enter email:");
                string email = Console.ReadLine();
                if (email.Contains('@') || email.Contains("gmail.com"))
                {
                    if (!email.Contains(" "))
                    {
                        //continue;
                    }
                    else
                    {
                        throw new InvalidDataException("\nemail should not contain space\n");
                    }
                }
                Console.WriteLine("Enter new password:");
                string pass = Console.ReadLine();
                bool status = _logging.Password(pass);

                if (status)
                {
                    bool check = _employee.Updateemployee(id, pass,email);

                    if (check)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nEmployee detail updated successfully\n");
                        Console.ResetColor();
                        Viewemployees();

                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nEmployee detail not updated\n");
                        Console.ResetColor();
                    }
                }
                else
                {
                    goto start;
                }
              

            }

            catch (DataInvalidException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }



        }

        public void Viewemployees()
        {
            try
            {
                List<employee> employee = _employee.Viewemployees();

                if (employee.Count() > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("\n**************EMPLOYEE DETAILS*************");
                    Console.ResetColor();
                    foreach (employee a in employee)
                    {
                        Console.WriteLine(a);
                    }

                    Console.WriteLine("\n*******************************************");
                }
                else
                {
                    Console.WriteLine("\nNo details available\n");
                }

            }

            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

        }



    }
}
