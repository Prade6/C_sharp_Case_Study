using Asset_management.Model;
using Asset_management.MyException;
using Asset_management.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    public class AssetMaintenanceRepository:IAssetmaintenance
    {
        SqlConnection sqlConnection=null;
        SqlCommand sqlCommand = null;

        Assettrackingrepository assettrackingrepository=new Assettrackingrepository();

        public AssetMaintenanceRepository()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();

        }



        public int maintenance(int assid, DateTime main_date, string description, decimal price)
        {
            int check = 0;
            try
            {
                sqlCommand.CommandText = "insert into  maintenance_records values(@id,@date,@des,@price)";
                sqlCommand.Parameters.Add("@price", SqlDbType.Decimal).Value = price;
                sqlCommand.Parameters.Add("@des", SqlDbType.VarChar).Value = description;
                sqlCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = main_date;
                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = assid;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                check = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                assettrackingrepository.assetstatus(assid, "under maintenance");

                //change status of asset


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }



        public int reserveasset(int id, int empid, DateTime res_date, DateTime sdate, DateTime edate,string status)
        {
            int check = 0;
            bool reserved= assettrackingrepository.checkreserved(id);
            try
            {
                if(!reserved)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "insert into reservations values(@id,@eid,@mdate,@sdate,@edate,@status)";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@eid", SqlDbType.Int).Value = empid;
                    sqlCommand.Parameters.Add("@mdate", SqlDbType.DateTime).Value = res_date;
                    sqlCommand.Parameters.Add("@sdate", SqlDbType.DateTime).Value = sdate;
                    sqlCommand.Parameters.Add("@edate", SqlDbType.DateTime).Value = edate;
                    sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    check = sqlCommand.ExecuteNonQuery();

                    assettrackingrepository.assetstatus(id, "reserved");
                }
                else
                {
                    throw new DuplicateReservationException("Asset is already reserved");
                }
            }
            catch(DuplicateReservationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sqlConnection.Close();
            return check;
        }


        public int withdraw_reservation(int id)
        {
            int check = 0, aid = 0;
            bool reserved = assettrackingrepository.checkreserved(id);
            try
            {
                if (reserved)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "select*from reservations where reservation_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        if (id == (int)reader["reservation_id"])
                        {
                            sqlCommand.CommandText = "delete from reservations where reservation_id=@id";
                            aid = (int)reader["asset_id"];
                            break;
                        }
                    }
                    check = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    assettrackingrepository.assetstatus(aid, "available");
                }
                else
                {
                    throw new DuplicateReservationException("Asset is not reserved");
                }

            }
            catch (DuplicateReservationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;

            //check reserved
        }


        public List<reservations> reservationdetails()
        {
            List<reservations> reserve = new List<reservations>();
            try
            {            
                sqlCommand.CommandText = "select*from reservations";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    reservations reservations = new reservations();
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
                Console.WriteLine(ex.Message);
            }
  
            sqlConnection.Close();
            return reserve;

        }

        public List<maintenance_records> maintenancedetails()
        {
            List<maintenance_records> maintenance_Records = new List<maintenance_records>();
            try
            {
                sqlCommand.CommandText = "select*from maintenance_records";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    maintenance_records maintenance = new maintenance_records();
                    maintenance.Asset_id = (int)reader["asset_id"];
                    maintenance.Maintenance_date = (DateTime)reader["maintenance_date"];
                    maintenance.Description = (string)reader["description"];
                    maintenance.Cost= (decimal)reader["cost"];
                    maintenance_Records.Add(maintenance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            sqlConnection.Close();
            return maintenance_Records;

        }


        public int withdraw_maintenance(int id)
        {
            int check = 0;
            bool exists = maintenanceexists(id);
            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "delete from maintenance_records where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    check = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    assettrackingrepository.assetstatus(id, "available");
                }
                else
                {
                    throw new DuplicateReservationException("Asset is not under maintenance");
                }

            }
            catch (DuplicateReservationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;

            //check reserved
        }

        public bool maintenanceexists(int id)
        {
            bool status = false;
            sqlCommand.CommandText = "select * from maintenance_records";
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                if ((int)reader["asset_id"] == id)
                {
                    status = true;
                    break;
                }

            }
            sqlConnection.Close();
            return status;
        }






    }
}
