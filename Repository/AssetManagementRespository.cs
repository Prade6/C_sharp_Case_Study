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

        //Adding new asset
        public int Addasset(asset assets)
        {
            int result = 0;      //rows affected
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "insert into assets values(@name,@type,@sid,@pdate,@location,default,null)";
                sqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = assets.Name;
                sqlCommand.Parameters.Add("@type", SqlDbType.VarChar).Value = assets.Type;
                sqlCommand.Parameters.Add("@pdate", SqlDbType.DateTime).Value = assets.Purchase_date;
                sqlCommand.Parameters.Add("@sid", SqlDbType.Int).Value = assets.Serial_number;
                sqlCommand.Parameters.Add("@location", SqlDbType.VarChar).Value = assets.Location;
                //sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = null;
                //sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = null;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                result = sqlCommand.ExecuteNonQuery();          

            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ResetColor();
            }
            sqlConnection.Close();
            return result;
        }



        //Updating asset information
        public bool Updateasset(string status, int asset_id,string location)
        {
            bool update = false;                //Check if asset updated
            bool asset_exists = Assetexists(asset_id);      //Check if asset is available

            try
            {
                if (asset_exists)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "update assets set status=@status , location=@location where asset_id=@id";
                    sqlCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = asset_id;
                    sqlCommand.Parameters.Add("@location", SqlDbType.VarChar).Value = location;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int check = sqlCommand.ExecuteNonQuery();                   //To store the count of rows modified
                 
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
            sqlConnection.Close();

            return update;

        }


        //Check if asset is available
        public bool Assetexists(int asset_id)
        {
            bool status = false;            
            sqlCommand.CommandText = "select * from assets";
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                if ((int)reader["asset_id"] == asset_id)
                {
                    status = true;
                    break;
                }

            }

            sqlConnection.Close();

            if (!status)
            {
                throw new AssetNotFoundException("Asset id not available");
            }
            
            return status;              //Returns true if asset available
        }


        //Deleting the asset
        public bool Deleteasset(int id)
        {
            bool result = false;                 //Check if asset deleted
            bool exists = Assetexists(id);      //Check if asset is available

            try
            {
                sqlCommand.Parameters.Clear();
                if (exists)
                {
                    Deleteallocation(id);               //Asset deleted from allocations
                    Deleteassetreservation(id);         //Asset deleted from reservations
                    Deletemaintenance(id);              //Asset deleted from maintenance
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = "delete from assets where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();

                    int res = sqlCommand.ExecuteNonQuery();
                    if(res>0)
                    {
                        result = true;
                    
                    }
                               
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

            sqlConnection.Close();
            return result;
        }


        //Delete from allocation
        public bool Deleteallocation(int id)
        {
            bool check = false;
            bool exists = Assetexists(id);      //Check if asset is available

            try
            {
                if (exists)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.CommandText = "delete from asset_allocations where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();

                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }
             

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
            sqlConnection.Close();
            return check;
        }


        //Delete from reservation
        public bool Deleteassetreservation(int id)
        {
            bool check = false;
            bool exists = Assetexists(id);      //Check if asset is available

            try
            {
                if(exists)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.CommandText = "delete from reservations where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();


                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }


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
            sqlConnection.Close();
            return check;
        }



        //Delete from maintenance
        public bool Deletemaintenance(int id)
        {
            bool check = false;
            bool exists = Assetexists(id);      //Check if asset is available

            try
            {
                if(exists)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.CommandText = "delete from maintenance_records where asset_id=@id";
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        check = true;
                    }

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
            sqlConnection.Close();
            return check;
        }


           // To display asset details
        public List<asset> Getassets()
        {
            List<asset> assets = new List<asset>();
            try
            {
               
                sqlCommand.CommandText = "select*from assets";
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    asset assets1 = new asset();
                    assets1.Asset_id = (int)reader["asset_id"];
                    assets1.Name = (string)reader["Name"];
                    assets1.Type = (string)reader["Type"];
                    assets1.Serial_number = (int)reader["serial_number"];
                    assets1.Purchase_date = (DateTime)reader["purchase_date"];
                    assets1.Location = (string)reader["location"];
                    assets1.Status = (string)reader["status"];
                    assets.Add(assets1);
                }
            }
            catch (Exception ex)
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
