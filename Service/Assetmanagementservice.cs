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
        asset assets=new asset();
        employee employees=new employee();
     

        public Assetmanagementservice()
        {
            _assetManagementmpl = new AssetManagementRespository();

        }

        //Adds a new asset to the system
        public void Addasset()                                     
        {start:
            
            try
            {
                Console.WriteLine("Enter asset name:");
                assets.Name = Console.ReadLine();
                if(assets.Name.Any(char.IsDigit)|| assets.Name.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("\nName should contain only letters\n");
                }

                Console.WriteLine("Enter asset type:");
                assets.Type = Console.ReadLine();
                if (assets.Type.Any(char.IsDigit) || assets.Type.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("\nAsset type should contain only alphabets\n");
                }

                Console.WriteLine("Enter serial number:");
                assets.Serial_number = int.Parse(Console.ReadLine());
                if (assets.Serial_number < 1)
                {
                    throw new DataInvalidException("\nSerial number must not be less than 0\n");
                }

                Console.WriteLine("Enter purchase date");
                assets.Purchase_date = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Enter location:");
                assets.Location = Console.ReadLine();
                if (assets.Location.Any(char.IsDigit) || assets.Location.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("\nLocation should contain only alphabets\n");
                }

                //Console.WriteLine("Enter status(in use, decommissioned, under maintenance,available,reserved):");
                //assets.Status = Console.ReadLine();

                //Console.WriteLine("Enter owner id:");
                //assets.Owner_id= int.Parse(Console.ReadLine());
                //if (assets.Owner_id< 1)
                //{
                //    throw new DataInvalidException("Owner id can't be negative\n");
                //}

                int check=_assetManagementmpl.Addasset(assets);
            
                if (check == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nAsset is not added\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nSucessfully added\n");
                    Console.ResetColor();
                    Getassets();
                }
            }
         catch (DataInvalidException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message,Console.ForegroundColor);
                Console.ResetColor();
                goto start;
            }
        catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }


        }

        // Updates information about an existing asset.

        public void Updateasset()                                      
        {start:
            try
            {
                Console.WriteLine("Enter asset id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 100)
                {
                    throw new DataInvalidException("\nAsset id can't be negative and not less tha 100\n");
                }
                Console.WriteLine("Enter status(in use, decommissioned, under maintenance,available,reserved):");
                string status = Console.ReadLine();
                Console.WriteLine("Enter location:");
                string location = Console.ReadLine();
                if (location.Any(char.IsDigit) || location.Any(char.IsSymbol))
                {
                    throw new DataInvalidException("\nLocation contain only alphabets\n");
                }
                bool check = _assetManagementmpl.Updateasset(status, id,location);
                if (check)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nAsset updated successfully\n");
                    Console.ResetColor();
                    Getassets();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nAsset not updated\n");
                    Console.ResetColor();
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

        //Deletes an asset from the system based on its ID.
        public void Deleteasset()                                       
        {
            try
            {
                Console.WriteLine("Enter asset id:");
                int id = int.Parse(Console.ReadLine());
                if (id < 100)
                {
                    throw new DataInvalidException("\nAsset id can't be negative and not less than 100\n");
                }
                bool check = _assetManagementmpl.Deleteasset(id);
                if (check)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nAsset is deleted Successfully\n");
                    Console.ResetColor();
                    Getassets();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nAsset is not deleted\n");
                    Console.ResetColor();
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

        public void Getassets()
        {
            try
            {
                List<asset> check = _assetManagementmpl.Getassets();
                if (check.Count==0)
                {

                    Console.WriteLine("\nThere are no assets available\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("\n**************ASSETS*************");
                    Console.ResetColor();
                    Console.WriteLine("\n");
                    foreach (asset a in check)
                    {
                        Console.WriteLine(a);
                    }
                    Console.WriteLine("\n*********************************\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         
        }

   
        


    }
}
