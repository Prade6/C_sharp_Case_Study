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
    internal class Assetmaintenanceservice: IAssetmaintenanceservice
    {
        readonly IAssetmaintenance _assetmaintenance;

        public Assetmaintenanceservice()
        {
            _assetmaintenance=new AssetMaintenanceRepository();
        }


        public void Maintainasset()
        {start:
            try
            {
                Console.WriteLine("Enter asset id");
                int id = int.Parse(Console.ReadLine());
                if (id < 0)
                {
                    throw new DataInvalidException("\nAsset id can't be negative and less than 100\n");
                }
                Console.WriteLine("Enter maintance date:");
                DateTime date = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Enter description:");
                string des = Console.ReadLine();
                Console.WriteLine("Enter amount:");
                decimal price = Convert.ToDecimal(Console.ReadLine());
                int check = _assetmaintenance.Maintenance(id, date, des, price);
                if (check == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nMaintenance detail is not added\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nMaintenance detail added\n");
                    Console.ResetColor();
                    Getmaintenance();
                }
            }
            catch (DataInvalidException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
                goto start;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

        }

        public void Reserve()
        {start:
            try
            {
                Console.WriteLine("Enter asset id");
                int id = int.Parse(Console.ReadLine());
                if (id < 0)
                {
                    throw new DataInvalidException("\nAsset id can't be negative and less than 100\n");
                }
                Console.WriteLine("Enter employee id");
                int emp_id = int.Parse(Console.ReadLine());
                if (emp_id < 0)
                {
                    throw new DataInvalidException("\nEmployee id can't be negative\n");
                }
                Console.WriteLine("Enter reservation date:");
                DateTime res_date = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Enter start date:");
                DateTime sdate = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Enter end date:");
                DateTime edate = Convert.ToDateTime(Console.ReadLine());
                //Console.WriteLine("Enter status of the assets(approved,pending,cancelled):");
                //string status=Console.ReadLine();
                int check = _assetmaintenance.Reserveasset(id, emp_id, res_date, sdate, edate);
                if (check == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nAsset id not reserved\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nAsset is reserved\n");
                    Console.ResetColor();
                    Getreserve();
                }
            }
            catch (DataInvalidException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
                goto start;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
        }


        //public void Removemaintenance()
        //{
        //    try
        //    {
        //        Console.WriteLine("Enter asset id:");
        //        int id = int.Parse(Console.ReadLine());
        //        int check = _assetmaintenance.Withdraw_maintenance(id);
        //        if (check == 0)
        //        {
        //            throw new AssetNotFoundException("Not updated");
        //        }
        //        else
        //        {
        //            Console.WriteLine("removed");
        //            Getmaintenance();
        //        }
        //    }
        //    catch (AssetNotFoundException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}



        public void Withrawreserve()
        {
            try
            {
                Console.WriteLine("Enter reservation id:");
                int id = int.Parse(Console.ReadLine());
                int check = _assetmaintenance.Withdraw_reservation(id);
                if (check == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nReservation is not withdrawed\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nReservation sucessfully Withdrawed\n");
                    Console.ResetColor();
                    Getreserve();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
        }


        public void Getreserve()
        {
            try
            {
                List<reservation> check = _assetmaintenance.Reservationdetails();
                if (check.Count == 0)
                {
                    //Console.WriteLine(check.Count);
                    Console.WriteLine("\nThere are no reservations available\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("\n**************RESERVATIONS*************\n");
                    Console.ResetColor();
                    foreach (reservation a in check)
                    {
                        Console.WriteLine(a);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

        }


        public void Getmaintenance()
        {
            try
            {
                List<maintenance_record> check = _assetmaintenance.Maintenancedetails();
                if (check.Count == 0)
                {
                    Console.WriteLine("\nThere are no assets are under maintenance\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("\n**************MAINTENANCE RECORDS*************");
                    Console.ResetColor();
                    foreach (maintenance_record a in check)
                    {
                        Console.WriteLine(a);
                    }
                    Console.WriteLine("\n**********************************************\n");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

        }



    }
}
