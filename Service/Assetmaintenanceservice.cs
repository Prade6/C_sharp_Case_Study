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


        public void maintainasset()
        {start:
            try
            {
                Console.WriteLine("Enter asset id");
                int id = int.Parse(Console.ReadLine());
                if (id < 0)
                {
                    throw new DataInvalidException("Asset id can't be negative and less than 100");
                }
                Console.WriteLine("Enter maintance date:");
                DateTime date = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Enter description:");
                string des = Console.ReadLine();
                Console.WriteLine("Enter amount:");
                decimal price = Convert.ToDecimal(Console.ReadLine());
                int check = _assetmaintenance.maintenance(id, date, des, price);
                if (check == 0)
                {
                    throw new AssetNotFoundException("Asset id not found");
                }
                else
                {
                    Console.WriteLine("Maintenance updated");
                    getmaintenance();
                }
            }
            catch (DataInvalidException ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void reserve()
        {start:
            try
            {
                Console.WriteLine("Enter asset id");
                int id = int.Parse(Console.ReadLine());
                if (id < 0)
                {
                    throw new DataInvalidException("Asset id can't be negative and less than 100");
                }
                Console.WriteLine("Enter employee id");
                int emp_id = int.Parse(Console.ReadLine());
                if (emp_id < 0)
                {
                    throw new DataInvalidException("Employee id can't be negative");
                }
                Console.WriteLine("Enter reservation date:");
                DateTime res_date = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Enter start date:");
                DateTime sdate = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Enter end date:");
                DateTime edate = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Enter status of the assets(approved,pending,cancelled):");
                string status=Console.ReadLine();
                int check = _assetmaintenance.reserveasset(id, emp_id, res_date, sdate, edate,status);
                if (check == 0)
                {
                    throw new AssetNotFoundException("Asset id not found");
                }
                else
                {
                    Console.WriteLine("Maintenance updated");
                    getreserve();
                }
            }
            catch (DataInvalidException ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void removemaintenance()
        {
            try
            {
                Console.WriteLine("Enter asset id:");
                int id = int.Parse(Console.ReadLine());
                int check = _assetmaintenance.withdraw_maintenance(id);
                if (check == 0)
                {
                    throw new AssetNotFoundException("Not updated");
                }
                else
                {
                    Console.WriteLine("removed");
                    getmaintenance();
                }
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public void withrawasset()
        {
            try
            {
                Console.WriteLine("Enter reservation id:");
                int id = int.Parse(Console.ReadLine());
                int check = _assetmaintenance.withdraw_reservation(id);
                if (check == 0)
                {
                    throw new AssetNotFoundException("Reservation id not found");
                }
                else
                {
                    Console.WriteLine("Withdrawed");
                    getreserve();
                }
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void getreserve()
        {
            try
            {
                List<reservations> check = _assetmaintenance.reservationdetails();
                if (check.Count == 0)
                {
                    Console.WriteLine(check.Count);
                    Console.WriteLine("There are no reservations available");
                }
                else
                {
                    Console.WriteLine("\n\n**************RESERVATIONS*************\n\n");
                    foreach (reservations a in check)
                    {
                        Console.WriteLine(a);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        public void getmaintenance()
        {
            try
            {
                List<maintenance_records> check = _assetmaintenance.maintenancedetails();
                if (check.Count == 0)
                {
                    Console.WriteLine("There are no assets are under maintenance");
                }
                else
                {
                    Console.WriteLine("\n\n**************MAINTENANCE RECORDS*************\n\n");
                    foreach (maintenance_records a in check)
                    {
                        Console.WriteLine(a);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



    }
}
