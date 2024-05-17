using Asset_management.Model;
using Asset_management.MyException;
using Asset_management.Repository;
using System;
using System.Buffers.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Asset_management.Service
{
    internal class Assetmanagementservice : IAssetmanagementservice
    {
        readonly IAssetManagementmpl _assetManagementmpl;
        Assets assets=new Assets();
        Employees employees=new Employees();
     

        public Assetmanagementservice()
        {
            _assetManagementmpl = new AssetManagementRespository();

        }

        //Adds a new asset to the system
        public void addasset()                                     
        {
            start:
            try
            {
                Console.WriteLine("Enter asset name:");
                assets.Name = Console.ReadLine();
                if(assets.Name.Any(char.IsDigit)|| assets.Name.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("Name contain only alphabets\n");
                }

                Console.WriteLine("Enter asset type:");
                assets.Type = Console.ReadLine();
                if (assets.Type.Any(char.IsDigit) || assets.Type.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("Asset type should contain only alphabets\n");
                }

                Console.WriteLine("Enter serial number:");
                assets.Serial_number = int.Parse(Console.ReadLine());
                if (assets.Serial_number < 1)
                {
                    throw new DataInvalidException("Serial number must not be less than 0\n");
                }

                Console.WriteLine("Enter purchase date");
                assets.Purchase_date = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter location:");
                assets.Location = Console.ReadLine();
                if (assets.Location.Any(char.IsDigit) || assets.Location.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("Location contain only alphabets\n");
                }

                Console.WriteLine("Enter status(in use, decommissioned, under maintenance,available,reserved):");
                assets.Status = Console.ReadLine();

                Console.WriteLine("Enter owner id:");
                assets.Owner_id= int.Parse(Console.ReadLine());
                if (assets.Owner_id< 1)
                {
                    throw new DataInvalidException("Owner id can't be negative\n");
                }

                int check=_assetManagementmpl.addasset(assets);
            
                if (check == 0)
                {
                    throw new Exception("Asset not added");
                }
                else
                {
                    Console.WriteLine("Sucessfully added");
                    getassets();
                }
            }
         catch (DataInvalidException ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
        catch(Exception ex)
            {
                Console.WriteLine(ex.Message); goto start;
            }


        }

        // Updates information about an existing asset.

        public void updateasset()                                      
        {start:
            try
            {
                Console.WriteLine("Enter asset id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 100)
                {
                    throw new DataInvalidException("Asset id can't be negative and not less tha 100");
                }
                Console.WriteLine("Enter status(in use, decommissioned, under maintenance,available,reserved):");
                string status = Console.ReadLine();
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();
                if (location.Any(char.IsDigit) || location.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("Location contain only alphabets");
                }
                bool check = _assetManagementmpl.updateasset(status, id,location);
                if (check)
                {
                    Console.WriteLine("Asset updated successfully");
                    getassets();
                }
                else
                {
                    Console.WriteLine("Asset not updated\nRetry");
                }

            }
            catch (DataInvalidException ex)
            {
                Console.WriteLine(ex.Message);
                goto start;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); goto start;
            }
        }

        //Deletes an asset from the system based on its ID.
        public void deleteasset()                                       
        {
            try
            {
                Console.WriteLine("Enter asset id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 100)
                {
                    throw new DataInvalidException("Asset id can't be negative and not less than 100");
                }
                bool check = _assetManagementmpl.deleteasset(id);
                if (check)
                {
                    Console.WriteLine("Deleted Successfully");
                    getassets();
                }
                else
                {
                    Console.WriteLine("Asset not deleted");
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

        public void getassets()
        {
            try
            {
                List<Assets> check = _assetManagementmpl.getassets();
                if (check.Count==0)
                {
                    Console.WriteLine("There are no assets available");
                }
                else
                {
                    Console.WriteLine("\n\n**************Assets*************\n\n");
                    foreach (Assets a in check)
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
