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

        AssetManagementRespository asset1=new AssetManagementRespository();

        public Assettrackingrepository()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();
        }

        public int allocateasset(int empid, int assid, DateTime date,DateTime enddate)/////////////
        {
            int check = 0;
            bool idexits = asset1.assetexists(assid);
            bool exists = checkreserved(assid);
            bool allocated = checkallocation(assid);

            try
            {
                if (idexits)
                {
                    if (!allocated)
                    {
                        if (exists)
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
                            assetstatus(assid, "in use");
                            reservestatus(assid, "approved");

                        }
                        else
                        {
                            throw new AssetNotFoundException("Asset should be reserved before allocation");
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
            catch(AssetNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }

        public int deallocate(int empid, int assid, DateTime date)
        {
            int check = 0;
            bool idexits = asset1.assetexists(assid);
            bool exists = checkreserved(assid);
            bool allocated=checkallocation(assid);
            try
            {
                if(idexits)
                {
                    if (exists && allocated)
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
                        assetstatus(assid, "available");

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
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }

        public bool checkallocation(int assid)
        {
            bool status = false;
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
            sqlConnection.Close();
            return status;
        }


        public bool checkreserved(int assid)
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
                Console.WriteLine(ex.Message);
            }

            sqlConnection.Close();
            return status;
        }

        public void assetstatus(int assid,string status)
        {
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "update assets set status=@status where asset_id=@id";
                sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = assid;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                int check = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void reservestatus(int assid, string status)
        {
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "update reservations set status=@status where asset_id=@id";
                sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = assid;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                int check = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<asset_allocations> allocationdetails()
        {
            List<asset_allocations> assets = new List<asset_allocations> ();
            try
            {
                sqlCommand.CommandText = "select*from asset_allocations ";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    asset_allocations asset = new asset_allocations();
                    asset.Asset_id = (int)reader["asset_id"];
                    asset.Employee_id = (int)reader["employee_id"];
                    asset.Allocation_date = (DateTime)reader["allocation_date"];
                    asset.Return_date = (DateTime)reader["return_date"];
                    assets.Add(asset);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
  
            sqlConnection.Close();

            return assets;
        }

    }
}
