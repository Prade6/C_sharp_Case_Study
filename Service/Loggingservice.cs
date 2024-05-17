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
    internal class Logging_service : Iloggingservice
    {
        readonly ILogging_info _logging_Info;

        public Logging_service()
            {
            _logging_Info = new Logging_info();
            }
        public bool logging()
        {
            start:
            bool check = false;
            try
            {
                Console.WriteLine("Enter employee id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 1)
                {
                    throw new DataInvalidException("\nEmployee id can't be negative\n");
                }
                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();
                check = _logging_Info.logging(id, password);
                    if (check)
                    {
                        Console.WriteLine("Sucessfully logged....\n");
                    }
                    else
                    {
                        Console.WriteLine("Enter correct id and password");
                    }
            }
            catch (DataInvalidException e)
            {
                Console.WriteLine(e.Message);
                goto start;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                goto start;
            }

            return check;
        }

        public bool register()
        {
        start:
            bool status = false;
            try
            {
                Console.WriteLine("Enter your name:");
                string name = Console.ReadLine();
                if (name.Any(char.IsDigit) ||name.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("Name contain only alphabets\n");
                }

                Console.WriteLine("Enter department:");
                string dept = Console.ReadLine();
                if (dept.Any(char.IsDigit) || dept.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("Name contain only alphabets\n");
                }

                Console.WriteLine("Enter email:");
                string email = Console.ReadLine();
                if (email.Contains('@')|| email.Contains("gmail.com"))
                {
                        if (!email.Contains(" "))
                        {
                            //continue;
                        }
                        else
                        {
                            throw new InvalidDataException("email should not contain space\n");
                        }                  
                }
                else
                {
                    throw new InvalidDataException("email is not valid\n");
                }

                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();

                bool pass = _logging_Info.password(password);
                status = false;
                if (pass)
                {
                    int check = _logging_Info.register(name, dept, email, password);
                    if (check > 0)
                    {
                        status = true;
                        Console.WriteLine("Successfully registered!!!\n");
                    }
                    else
                    {
                        Console.WriteLine("Registration failed\n");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid password\n");
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
            return status;
        }

    }
}
