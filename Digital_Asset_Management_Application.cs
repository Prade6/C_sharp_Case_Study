using Asset_management.Repository;
using Asset_management.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Asset_management
{
    internal class Digital_Asset_Management_Application
    {
        IAssetmanagementservice assetmanagementservice;

        Iloggingservice loggingservice;

        IAssetmaintenanceservice assetmaintenanceservice;

        IAssetTrackingservice assettrackingservice;
        Iemployeeservice employeeservice;

        public Digital_Asset_Management_Application()
        { 
            assetmanagementservice = new Assetmanagementservice();
            loggingservice=new Logging_service();
            assetmaintenanceservice=new Assetmaintenanceservice();
            assettrackingservice=new AssetTrackingservice();
            employeeservice=new employeeservice();
        }

        public void run()
        {
            bool check=false;
            int i = 0;
            Console.WriteLine("*************Welcome to Digital_Asset_Management***************\n");
            start:
            try
            {
                Console.WriteLine("1.Login\n2.Register\n");
                int option = int.Parse(Console.ReadLine());
                if (option == 1)
                {
                    check = loggingservice.logging();
                   
                    i++;
                }
                else if (option == 2)
                {
                    check = loggingservice.register();
                }
                else
                {
                    Console.WriteLine("LOGIN OR REGISTER");

                }
                if (i == 3 && check==true)
                {
                    Console.WriteLine("Retry after 5 sec!!");
                    //Task.Delay(5000);
                    goto start;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                goto start;
            }


            if (check)
            {
            menu:
                try
                {
                    Console.WriteLine("*************************\n1----Asset Management\n2----Asset tracking\n" +
                        "3----Asset maintenance\n4----Employee account management\n5----Log out\n**********************************\n");
                    int options = int.Parse(Console.ReadLine());
                    switch (options)
                    {
                        case 1:
                            {
                                menu1:
                                Console.WriteLine("\n1----Add asset\n2----Update asset\n3----Delete asset\n4----View assets\n");
                                int opt = int.Parse(Console.ReadLine());
                                {
                                    switch (opt)
                                    {
                                        case 1:
                                            assetmanagementservice.getassets();
                                            assetmanagementservice.addasset();
                                            break;
                                        case 2:
                                            assetmanagementservice.getassets();
                                            assetmanagementservice.updateasset();
                                            break;
                                        case 3:
                                            assetmanagementservice.getassets();
                                            assetmanagementservice.deleteasset();
                                            break;
                                        case 4:
                                            assetmanagementservice.getassets();
                                            break;
                                        default:
                                            Console.WriteLine("Enter correct option");
                                            goto menu1;
                                            break;
                                    }

                                }
                                goto menu;
                                break;
                            }
                        case 2:
                            {menu2:
                                Console.WriteLine("\n1-----Allocate asset\n2----Deallocate asset\n3----View allocation details\n");
                                int opt = int.Parse(Console.ReadLine());
                                {
                                    switch (opt)
                                    {
                                        case 1:
                                            assetmaintenanceservice.getreserve();
                                            assettrackingservice.allocateasset(); break;
                                        case 2:
                                            assettrackingservice.allocationdetails();
                                            assettrackingservice.deallocateasset(); break;
                                        case 3:
                                            assettrackingservice.allocationdetails();
                                            break;
                                         
                                       default:
                                            Console.WriteLine("Enter correct option");
                                            goto menu2;
                                            break;

                                    }

                                }

                            }
                            goto menu;
                            break;
                        case 3:
                            {menu3:
                                Console.WriteLine("1----Maintanence\n2----Reserve asset\n3----Withdraw reserve\n4----View maintenance details\n5----View Reservation details\n6----Remove maintenance");
                                int opt = int.Parse(Console.ReadLine());
                                {
                                    switch (opt)
                                    {
                                        case 1:
                                            assetmanagementservice.getassets();
                                            assetmaintenanceservice.maintainasset();
                                            break;
                                        case 2:
                                            assetmanagementservice.getassets();
                                            assetmaintenanceservice.reserve();
                                            break;
                                        case 3:
                                            assetmanagementservice.getassets();
                                            assetmaintenanceservice.withrawasset(); break;
                                        case 4:
                                            assetmaintenanceservice.getmaintenance();
                                            break;
                                        case 5:
                                            assetmaintenanceservice.getreserve();
                                            break;
                                        case 6:
                                            assetmanagementservice.getassets();
                                            assetmaintenanceservice.removemaintenance();
                                            break;
                                        default:
                                            Console.WriteLine("Enter correct option");
                                            goto menu3;
                                            break;
                                    }
                                }
                            }
                            goto menu;
                            break;

                        case 4:
                            {
                                Console.WriteLine("1.Get detail\n2.Update employee details\n3.Delete account\n.Addemployee");
                                int opt = int.Parse(Console.ReadLine());
                                {
                                    switch (opt)
                                    {
                                        case 1:
                                            employeeservice.viewemployees();
                                            break;
                                        case 2:
                                            employeeservice.updateemployees();
                                            employeeservice.viewemployees();
                                            break;
                                        case 3:
                                            employeeservice.deleteemployee();
                                            employeeservice.viewemployees();
                                            break;
                                        case 4:
                                            loggingservice.register();
                                            break;
                                    }
                                }
                                goto menu;
                                break;
                            }
                        case 5:
                            Console.WriteLine("Logging out...");
                            break;

                        default:
                            Console.WriteLine("Enter valid option!!");
                            break;
                            goto menu;

                    }
                   
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    goto menu;
                }
            }

            else
            {
                goto start;
            }
          
        }

    }
}
