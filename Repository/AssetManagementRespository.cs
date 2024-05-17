using Asset_management.Model;
using Asset_management.MyException;
using Asset_management.Utility;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Asset_management.Repository
{
    public class AssetManagementRespository : IAssetManagementmpl
    {
        SqlConnection sqlConnection = null;
        SqlCommand sqlCommand = null;
        public AssetManagementRespository()
        {
            sqlConnection = new SqlConnection(UDbconnect.Getconnectstring());
            sqlCommand = new SqlCommand();

        }


        public int addasset(Assets assets)
        {
            int check = 0;
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "insert into assets values(@name,@type,@sid,@pdate,@location,@status,@id)";
                sqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = assets.Name;
                sqlCommand.Parameters.Add("@type", SqlDbType.VarChar).Value = assets.Type;
                sqlCommand.Parameters.Add("@pdate", SqlDbType.DateTime).Value = assets.Purchase_date;
                sqlCommand.Parameters.Add("@sid", SqlDbType.Int).Value = assets.Serial_number;
                sqlCommand.Parameters.Add("@location", SqlDbType.VarChar).Value = assets.Location;
                sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = assets.Status;
                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = assets.Owner_id ;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                check = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
     
            return check;
        }

        public bool updateasset(string status, int id,string location)
        {
            bool update = false;
            bool exists = assetexists(id);

            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "update assets set status=@status , location=@location where asset_id=@id";
                    sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                    sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    sqlCommand.Parameters.Add("@location", SqlDbType.VarChar).Value = location;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int check = sqlCommand.ExecuteNonQuery();
                  

                    if (check > 0)
                    {
                        update = true;
                    }
                }
                else
                {
                    throw new AssetNotFoundException("Asset id not available");
                }
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sqlConnection.Close();

            return update;

        }

        public bool assetexists(int id)
        {
            bool status = false;
            sqlCommand.CommandText = "select * from assets";
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


        public bool deleteasset(int id)//asset
        {
            bool check = false;
            bool exists = assetexists(id);

            try
            {
                sqlCommand.Parameters.Clear();
                if (exists)
                {
                    deleteassetreservation(id);
                    deleteallocation(id);
                    deletemaintenance(id);
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "delete from assets where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();

                    int res = sqlCommand.ExecuteNonQuery();
                    if(res>0)
                    {
                        check = true;
                    
                    }
                    sqlConnection.Close();               

                }
                else
                {
                    throw new AssetNotFoundException("Asset id not available");
                }
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
  
            return check;
        }


        public bool deleteallocation(int id)//allocation
        {
            bool check = false;
            bool exists = assetexists(id);

            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.CommandText = "delete from asset_allocations where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();

                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }
                    sqlConnection.Close();

                }
                else
                {
                    throw new AssetNotFoundException("Asset id not available");
                }

            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }

        public bool deleteassetreservation(int id)//reservation
        {
            bool check = false;
            bool exists = assetexists(id);

            try
            {
                if(exists)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.CommandText = "delete from reservations where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();


                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }
                    sqlConnection.Close();

                }
                else
                {
                    throw new AssetNotFoundException("Asset id not available");
                }
            }
       
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }


        public bool deletemaintenance(int id)//maintenance
        {
            bool check = false;
            bool exists = assetexists(id);

            try
            {
                if(exists)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.CommandText = "delete from maintenance_records where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }
                    sqlConnection.Close();
                }
                else
                {
                    throw new AssetNotFoundException("Asset id not available");
                }


            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return check;
        }




        public List<Assets> getassets()
        {
                List<Assets> assets = new List<Assets>();
                sqlCommand.CommandText = "select*from assets";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                Assets assets1 = new Assets();
                assets1.Asset_id = (int)reader["asset_id"];
                assets1.Name = (string)reader["Name"];  
                assets1.Type= (string)reader["Type"];
                assets1.Serial_number = (int)reader["serial_number"];
                assets1.Purchase_date = (DateTime)reader["purchase_date"];
                assets1.Location = (string)reader["location"];
                assets1.Status = (string)reader["status"];
                assets.Add(assets1);
                }
                sqlConnection.Close();
                return assets;
                         
        }




     

 
    }
}
