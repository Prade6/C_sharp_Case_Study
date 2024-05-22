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
    internal class AssetTrackingservice:IAssetTrackingservice
    {
        readonly IAssettracking _assettracking;

        public AssetTrackingservice()
        {
            _assettracking = new Assettrackingrepository();
        }
        public void Allocateasset()
        {
            try
            {
                Console.WriteLine("Enter employee id:");
                int eid = int.Parse(Console.ReadLine());
                if (eid < 0)
                {
                    throw new DataInvalidException("Employee id can't be negative\n");
                }

                Console.WriteLine("Enter asset id:");
                int aid = int.Parse(Console.ReadLine());
                if (aid < 0)
                {
                    throw new DataInvalidException("Asset id can't be negative and less than 100\n");
                }

                Console.WriteLine("Enter Allocation date:");
                DateTime date = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter return date:");
                DateTime enddate = Convert.ToDateTime(Console.ReadLine());

                int check = _assettracking.Allocateasset(eid, aid, date,enddate);
                if (check == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Allocation failed\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Asset is allocated\n");
                    Console.ResetColor();
                    Allocationdetails();
                }
            }
            catch (DataInvalidException ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



        public void Deallocateasset()
        {
            try
            {
                Console.WriteLine("Enter employee id:");
                int eid = int.Parse(Console.ReadLine());
                if (eid < 0)
                {
                    throw new DataInvalidException("\nEmployee id can't be negative\n");
                }
                Console.WriteLine("Enter asset id:");
                int aid = int.Parse(Console.ReadLine());
                if (aid < 0)
                {
                    throw new DataInvalidException("\nAsset id can't be negative and less than 100\n");
                }
                Console.WriteLine("Enter return date:");
                DateTime date = Convert.ToDateTime(Console.ReadLine());
                int check = _assettracking.Deallocate(eid, aid, date);
                if (check == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDeallocation fails\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nAsset is deallocated\n");
                    Console.ResetColor();
                    Allocationdetails();
                }
            }
            catch (DataInvalidException ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void Allocationdetails()
        {
            try
            {
                List<asset_allocation> check = _assettracking.Allocationdetails();
                if (check.Count == 0)
                {
                    Console.WriteLine("\nThere are no assets available\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("\n**************ASSETES ALLOCATIONS*************\n");
                    Console.ResetColor();
                    foreach (asset_allocation a in check)
                    {
                        Console.WriteLine(a);
                    }
                    Console.WriteLine("\n***********************************************\n");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
