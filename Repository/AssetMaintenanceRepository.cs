using Asset_management.Model;
using Asset_management.MyException;
using Asset_management.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    public class AssetMaintenanceRepository:IAssetmaintenance
    {
        SqlConnection sqlConnection=null;
        SqlCommand sqlCommand = null;


        public AssetMaintenanceRepository()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();

        }

        Assettrackingrepository assettrackingrepository = new Assettrackingrepository();
        AssetManagementRespository managementRespository = new AssetManagementRespository();
        Logindetail_info logginginfo = new Logindetail_info();

        //Add maintenance details
        public int Maintenance(int assid, DateTime main_date, string description, decimal price)
        {
            bool asset_exists = managementRespository.Assetexists(assid);      //Check if asset is available
            int check = 0;
            try
            {
                if(asset_exists)          //Check if asset is available
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "insert into  maintenance_records values(@id,@date,@des,@price)";
                    sqlCommand.Parameters.Add("@price", SqlDbType.Decimal).Value = price;
                    sqlCommand.Parameters.Add("@des", SqlDbType.VarChar).Value = description;
                    sqlCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = main_date;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = assid;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    check = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    assettrackingrepository.Assetstatus(assid, "under maintenance");      //Updating asset status
                }
                else
                {
                    throw new AssetNotFoundException("Asset id not available");
                }
            }

            catch (AssetNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

        
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            return check;
        }

        //Reserving the asset

        public int Reserveasset(int asset_id, int empid, DateTime res_date, DateTime sdate, DateTime edate)
        {
            int check = 0;
            bool emp_exists = logginginfo.Empoloyeeexists(empid);      //Check if employee exists
            //bool exists = managementRespository.Assetexists(id);            //Check if asset exists
            bool asset_maintenance=Checkmaintenance(asset_id);                    //Check if assest is properly maintained
            bool reserved= assettrackingrepository.Checkreserved(asset_id);      //Check if asset is reserved
            bool asset_status=Checkassetstatus(asset_id);                        //Check if asset is in available status for reserving
            //asset status-available
            try
            {
                if(emp_exists)
                {
                    if (asset_status)                //Check if asset is in available status for reserving
                    {
                        if (asset_maintenance)       //Check if assest is properly maintained
                        {
                            if (!reserved)              //Check if asset is reserved
                            {
                                sqlCommand.Parameters.Clear();
                                sqlCommand.CommandText = "insert into reservations values(@id,@eid,@mdate,@sdate,@edate,default)";
                                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = asset_id;
                                sqlCommand.Parameters.Add("@eid", SqlDbType.Int).Value = empid;
                                sqlCommand.Parameters.Add("@mdate", SqlDbType.DateTime).Value = res_date;
                                sqlCommand.Parameters.Add("@sdate", SqlDbType.DateTime).Value = sdate;
                                sqlCommand.Parameters.Add("@edate", SqlDbType.DateTime).Value = edate;
                                //sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                                sqlCommand.Connection = sqlConnection;
                                sqlConnection.Open();
                                check = sqlCommand.ExecuteNonQuery();
                                sqlConnection.Close();
                                assettrackingrepository.Assetstatus(asset_id, "reserved");      //Updating asset status from available to reserved
                            }

                            else
                            {
                                throw new DuplicateReservationException("Asset is already reserved");
                            }
                        }
                        else
                        {
                            throw new AssetNotMaintainException("Asset is not maintained for 2 years");
                        }
                    }
                    else
                    {
                        throw new AssetNotFoundException("Asset is not available");
                    }
                }
                else
                {
                    throw new EmployeenotfoundException("Employee id not available");
                }

            }
            catch (EmployeenotfoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            catch (AssetNotMaintainException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            catch (DuplicateReservationException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            catch (AssetNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
         
            return check;
        }


        
        //Withdrawing reserve

        public int Withdraw_reservation(int id)
        {
            int check = 0, aid = 0;
            bool reserved = Reservationexists(id);      //Check if asset is reserved
            try
            {
                if(reserved)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "select asset_id from reservations where reservation_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    if (sqlCommand.ExecuteScalar != null)
                    {
                        aid = (int)sqlCommand.ExecuteScalar();
                    }
                    sqlCommand.CommandText = "delete from reservations where reservation_id=@id";

                    check = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                    if (check != 0)
                    {
                        assettrackingrepository.Assetstatus(aid, "available");      //Updating asset status to available
                        managementRespository.Deleteallocation(aid);
                    }
                }
             
                else
                { 
                    throw new DuplicateReservationException("\nReservation id not found\n");
                }

            }
            catch (DuplicateReservationException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            return check;


        }


        //Displaying reservation details
        public List<reservation> Reservationdetails()
        {
            List<reservation> reserve = new List<reservation>();
            try
            {            
                sqlCommand.CommandText = "select*from reservations";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    reservation reservations = new reservation();
                    reservations.Reservation_id = (int)reader["reservation_id"];
                    reservations.Asset_id = (int)reader["asset_id"];
                    reservations.Employee_id = (int)reader["employee_id"];
                    reservations.Reservation_date = (DateTime)reader["reservation_date"];
                    reservations.Start_date = (DateTime)reader["start_date"];
                    reservations.End_date = (DateTime)reader["end_date"];
                    reservations.Status = (string)reader["status"];
                    reserve.Add(reservations);
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
  
            sqlConnection.Close();
            return reserve;

        }


        //Displaying maintenance details
        public List<maintenance_record> Maintenancedetails()
        {
            List<maintenance_record> maintenance_Records = new List<maintenance_record>();
            try
            {
                sqlCommand.CommandText = "select*from maintenance_records";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    maintenance_record maintenance = new maintenance_record();
                    maintenance.Asset_id = (int)reader["asset_id"];
                    maintenance.Maintenance_date = (DateTime)reader["maintenance_date"];
                    maintenance.Description = (string)reader["description"];
                    maintenance.Cost= (decimal)reader["cost"];
                    maintenance_Records.Add(maintenance);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            sqlConnection.Close();
            return maintenance_Records;

        }


        //Check if reservationid exists
        public bool Reservationexists(int id)
        {
            
            bool status = false;
            try
            {
                sqlCommand.CommandText = "select * from reservations";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if ((int)reader["reservation_id"] == id)
                    {
                        status = true;
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            sqlConnection.Close();
            return status;
        }

        //Check if asset is available for reserving
        public bool Checkassetstatus(int id)
        {
            bool asset_status=false;
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "select * from assets where asset_id=@id";
                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    if (((string)sqlDataReader["status"]).ToLower()=="available")
                    {
                        asset_status= true; break;
                    }
                }

            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            sqlConnection.Close();
            return asset_status;
        }


        //Check if asset is properly maintained

        public bool Checkmaintenance(int id)
        {
            int check = 0;
            int? year=0;
            bool asset_maintenance = true;
            bool asset_exists = managementRespository.Assetexists(id);      //Check if asset is available
            try
            {
                if(asset_exists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "select top 1 DateDiff(year,maintenance_date,getdate()) from maintenance_records where asset_id=@id order by maintenance_date desc";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    if (sqlCommand.ExecuteScalar() == null)
                    {
                        sqlCommand.CommandText = "select DateDiff(year,purchase_date,getdate()) from assets where asset_id=@id";
                        if (sqlCommand.ExecuteScalar != null)
                        {
                            year = (int)sqlCommand.ExecuteScalar();
                            if (year >= 2)
                            {
                                asset_maintenance = false;

                            }
                        }
                    }
                    else
                    {
                        year = (int)sqlCommand.ExecuteScalar();
                        if (year >= 2)
                        {
                            asset_maintenance = false;

                        }
                    }
                }
                else
                {
                    asset_maintenance=false;
                }
     
              
            }
           
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }

            sqlConnection.Close();
            return asset_maintenance;

        }




    }
}
