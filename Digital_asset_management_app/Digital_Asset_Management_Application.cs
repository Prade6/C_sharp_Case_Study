using Asset_management.Repository;
using Asset_management.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Asset_management.Digital_asset_management_app
{
    internal class Digital_Asset_Management_Application
    {
        IAssetmanagementservice assetmanagementservice;

        Iloginservice loggingservice;

        IAssetmaintenanceservice assetmaintenanceservice;

        IAssetTrackingservice assettrackingservice;
        Iemployeeservice employeeservice;

        public Digital_Asset_Management_Application()
        {
            assetmanagementservice = new Assetmanagementservice();
            loggingservice = new Logging_service();
            assetmaintenanceservice = new Assetmaintenanceservice();
            assettrackingservice = new AssetTrackingservice();
            employeeservice = new employeeservice();
        }

        public void run()
        {
            bool check = false;
            int i = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("                           *************WELCOME TO DIGITAL ASSET MANAGEMENT***************                           \n");
            Console.ResetColor();
        start:
            try
            {               
                    Console.WriteLine("1----Login\n2----Register\n");
                    int option = int.Parse(Console.ReadLine());
                    if (option == 1)
                    {
                        check = loggingservice.Logging();
                        i++;
                    }
                    else if (option == 2)
                    {
                        check = loggingservice.Register();
                    }
                    else
                    {
                        Console.WriteLine("LOGIN OR REGISTER");

                    }
                    if (i == 3 && check == false)
                    {
                        Console.WriteLine("Retry after 5 sec!!");
                        //Thread.Sleep(5000);
                        goto start;
                    }             
             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                goto start;
            }


            if (check)
            {
            menu:
                try
                {
                    Console.WriteLine("**********************************\n1----Asset Management\n2----Asset tracking\n" +
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
                                            assetmanagementservice.Getassets();     //Call function to display assets
                                            assetmanagementservice.Addasset();      //Call function to add asset
                                            break;
                                        case 2:
                                            assetmanagementservice.Getassets();     //Call function to display assets
                                            assetmanagementservice.Updateasset();     //Call function to update assets
                                            break;
                                        case 3:
                                            assetmanagementservice.Getassets();     //Call function to display assets
                                            assetmanagementservice.Deleteasset();     //Call function to delete assets
                                            break;
                                        case 4:
                                            assetmanagementservice.Getassets();     //Call function to display assets
                                            break;
                                        default:
                                            Console.WriteLine("Enter correct option");
                                            goto menu1;
                                            break;
                                    }

                                }
                                goto menu;                                          //Go back to main menu
                                break;
                            }
                        case 2:
                            {
                            menu2:
                                Console.WriteLine("\n1-----Allocate asset\n2----Deallocate asset\n3----View allocation details\n");
                                int opt = int.Parse(Console.ReadLine());
                                {
                                    switch (opt)
                                    {
                                        case 1:
                                            assetmaintenanceservice.Getreserve();               //Call function to display reservation
                                            assettrackingservice.Allocateasset(); break;        //Call function to allocate assets
                                        case 2:
                                            assettrackingservice.Allocationdetails();           //Call function to display allocations
                                            assettrackingservice.Deallocateasset(); break;     //Call function to delete allocation
                                        case 3:
                                            assettrackingservice.Allocationdetails();           //Call function to display allocations
                                            break;

                                        default:
                                            Console.WriteLine("Enter correct option");
                                            goto menu2;
                                            break;

                                    }

                                }

                            }
                            goto menu;                                         //Go back to main menu
                            break;
                        case 3:
                            {
                            menu3:
                                Console.WriteLine("\n\n1----Add Maintanence Detail\n2----Reserve asset\n3----Withdraw reservation\n" +
                                    "4----View maintenance details\n5----View Reservation details\n");
                                int opt = int.Parse(Console.ReadLine());
                                {
                                    switch (opt)
                                    {
                                        case 1:
                                            assetmanagementservice.Getassets();                 //Call function to display assets
                                            assetmaintenanceservice.Maintainasset();            //Call function to add asset to maintenance
                                            break;
                                        case 2:
                                            assetmanagementservice.Getassets();                 //Call function to display assets
                                            assetmaintenanceservice.Reserve();                  //Call function to made reservation
                                            break;
                                        case 3:
                                            assetmaintenanceservice.Getreserve();               //Call function to display reservation
                                            assetmaintenanceservice.Withrawreserve(); break;      //Call function to withdraw reservation
                                        case 4:
                                            assetmaintenanceservice.Getmaintenance();           //Call function to display maintenance details
                                            break;
                                        case 5:
                                            assetmaintenanceservice.Getreserve();               //Call function to display reservation details
                                            break;

                                        default:
                                            Console.WriteLine("Enter correct option");
                                            goto menu3;
                                            break;
                                    }
                                }
                            }
                            goto menu;                                                              //Go back to main menu
                            break;

                        case 4:
                            {
                                Console.WriteLine("1.View employee detail\n2.Update employee details\n3.Delete account\n4.Addemployee");
                                int opt = int.Parse(Console.ReadLine());
                                {
                                    switch (opt)
                                    {
                                        case 1:
                                            employeeservice.Viewemployees();                            //Call function to display employee details
                                            break;
                                        case 2:
                                            employeeservice.Viewemployees();                            //Call function to display employee details
                                            employeeservice.Updateemployees();                          ////Call function to update employee details
                                            break;
                                        case 3:
                                            employeeservice.Viewemployees();                            //Call function to display employee details
                                            employeeservice.Deleteemployee();                            //Call function to delete employee 
                                            employeeservice.Viewemployees();                             //Call function to display employee details
                                            break;
                                        case 4:
                                            loggingservice.Register();                                    //Call function to add employee
                                            break;
                                    }
                                }
                                goto menu;
                                break;
                            }
                        case 5:                                                                         //To log out from the application
                            Console.WriteLine("Logging out...");
                            break;

                        default:
                            Console.WriteLine("Enter valid option!!");
                            goto menu;                                                                     //Go back to main menu if option entered is not valid
                            break;



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
