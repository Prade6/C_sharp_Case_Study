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
        public void allocateasset()
        {
            try
            {
                Console.WriteLine("Enter employee id:");
                int eid = int.Parse(Console.ReadLine());
                if (eid < 0)
                {
                    throw new DataInvalidException("Employee id can't be negative");
                }

                Console.WriteLine("Enter asset id:");
                int aid = int.Parse(Console.ReadLine());
                if (aid < 0)
                {
                    throw new DataInvalidException("Asset id can't be negative and less than 100");
                }

                Console.WriteLine("Enter Allocation date:");
                DateTime date = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter return date:");
                DateTime enddate = Convert.ToDateTime(Console.ReadLine());

                int check = _assettracking.allocateasset(eid, aid, date,enddate);
                if (check == 0)
                {
                    throw new Exception("Allocation failed");
                }
                else
                {
                    Console.WriteLine("Asset is allocated\n");
                    allocationdetails();
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



        public void deallocateasset()
        {
            try
            {
                Console.WriteLine("Enter employee id:");
                int eid = int.Parse(Console.ReadLine());
                if (eid < 0)
                {
                    throw new DataInvalidException("Employee id can't be negative");
                }
                Console.WriteLine("Enter asset id:");
                int aid = int.Parse(Console.ReadLine());
                if (aid < 0)
                {
                    throw new DataInvalidException("Asset id can't be negative and less than 100");
                }
                Console.WriteLine("Enter Deallocation date:");
                DateTime date = Convert.ToDateTime(Console.ReadLine());
                int check = _assettracking.deallocate(eid, aid, date);
                if (check == 0)
                {
                    throw new Exception("Deallocation fails\n");
                }
                else
                {
                    Console.WriteLine("Asset is deallocated\n");
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

        public void allocationdetails()
        {
            try
            {
                List<asset_allocations> check = _assettracking.allocationdetails();
                if (check.Count == 0)
                {
                    Console.WriteLine("There are no assets available\n");
                }
                else
                {
                    Console.WriteLine("**************Assets*************");
                    foreach (asset_allocations a in check)
                    {
                        Console.WriteLine(a);
                    }
                    Console.WriteLine("*********************************");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
