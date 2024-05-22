using Asset_management.Model;
using Asset_management.MyException;
using Asset_management.Utility;
using System.Data;
using System.Data.SqlClient;
namespace Asset_management.Repository
{
    internal class Assettrackingrepository:IAssettracking
    {
        SqlConnection sqlConnection = null;
        SqlCommand sqlCommand = null;


        //AssetMaintenanceRepository assetMaintenanceRepository = new AssetMaintenanceRepository();

        public Assettrackingrepository()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();
        }

        AssetManagementRespository asset1 = new AssetManagementRespository();
        Logindetail_info logginginfo = new Logindetail_info();


        //To allocated the asset
        public int Allocateasset(int empid, int assid, DateTime date,DateTime enddate)
        {
            int check = 0;
            bool emp_exists = logginginfo.Empoloyeeexists(empid);
            bool asset_exits = asset1.Assetexists(assid);      //Check if asset is available
            bool isreserved = Checkreserved(assid);            //Check if asset is reserved
            bool allocated = Checkallocation(assid);      //Check if asset is already allocated

            try
            {if(emp_exists)
                {
                    if (asset_exits) //Check if asset is available
                    {
                        if (!allocated)   //Check if asset is already allocated
                        {
                            if (isreserved)   //Check if asset is reserved
                            {
                                sqlCommand.Parameters.Clear();
                                sqlCommand.CommandText = "insert into asset_allocations values(@aid,@id,@date,@edate)";
                                sqlCommand.Parameters.Add("@aid", SqlDbType.Int).Value = assid;
                                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = empid;
                                sqlCommand.Parameters.Add("@date", SqlDbType.Date).Value = date;
                                sqlCommand.Parameters.Add("@edate", SqlDbType.Date).Value = enddate;
                                sqlCommand.Connection = sqlConnection;
                                sqlConnection.Open();
                                check = sqlCommand.ExecuteNonQuery();
                                sqlConnection.Close();
                                Assetstatus(assid, "in use");           //Updating asset status
                                Reservestatus(assid, "approved");      //Updating reservation status

                            }
                            else
                            {
                                throw new AssetNotFoundException("Asset should be reserved for allocation");
                            }
                        }
                        else
                        {
                            throw new AssetNotFoundException("Asset is already allocated");
                        }
                    }
                    else
                    {
                        throw new AssetNotFoundException("Asset not available");
                    }
                }
                else
                {
                    throw new EmployeenotfoundException("Employee id not available");
                }

            }
            catch(AssetNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            catch (EmployeenotfoundException ex)
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


        //To deallocate the asset
        public int Deallocate(int empid, int assid, DateTime date)
        {
            int check = 0;
            bool asset_exists = asset1.Assetexists(assid);      //Check if asset is available
            bool allocated=Checkallocation(assid);          //Check if asset is allocated
            try
            {
                if(asset_exists)                //Check if asset is available
                {
                    if (allocated)          //Check if asset is allocated
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.CommandText = "delete from asset_allocations where asset_id=@aid and employee_id=@id and return_date=@date";
                        sqlCommand.Parameters.Add("@aid", SqlDbType.Int).Value = assid;
                        sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = empid;
                        sqlCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
                        sqlCommand.Connection = sqlConnection;
                        sqlConnection.Open();
                        check = sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        Assetstatus(assid, "available");            //Updating asset status
                        asset1.Deleteassetreservation(assid);      //remove from reservation

                    }
                    else
                    {
                        throw new AssetNotFoundException("Asset is not allocated");
                    }
                }
                else
                {
                    throw new AssetNotFoundException("Asset is not available");
                }


            }
            catch (AssetNotFoundException e)
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

        //Check if asset is allocated
        public bool Checkallocation(int assid)
        {
            bool status = false;
            try
            {
                sqlCommand.CommandText = "select * from asset_allocations";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if ((int)reader["asset_id"] == assid)
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


        //Check if asset is reserved
        public bool Checkreserved(int assid)
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
                    if ((int)reader["asset_id"] == assid)
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


        //Updating asset status
        public void Assetstatus(int assid,string status)
        {
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "update assets set status=@status where asset_id=@id";
                sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = assid;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                int check = sqlCommand.ExecuteNonQuery();
               
            }

            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            sqlConnection.Close();
        }

        //Updating reservation status
        public void Reservestatus(int assid, string status)
        {
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "update reservations set status=@status where asset_id=@id";
                sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = assid;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                int check = sqlCommand.ExecuteNonQuery();
               
            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            sqlConnection.Close();
        }


        //Displaying allocation details
        public List<asset_allocation> Allocationdetails()
        {
            List<asset_allocation> assets = new List<asset_allocation> ();
            try
            {
                sqlCommand.CommandText = "select*from asset_allocations ";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    asset_allocation asset = new asset_allocation();
                    asset.Asset_id = (int)reader["asset_id"];
                    asset.Employee_id = (int)reader["employee_id"];
                    asset.Allocation_date = (DateTime)reader["allocation_date"];
                    asset.Return_date = (DateTime)reader["return_date"];
                    assets.Add(asset);

                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
  
            sqlConnection.Close();

            return assets;
        }

    }
}
