using Asset_management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    public interface IAssetmaintenance
    {
        int maintenance(int id, DateTime date, string des, decimal price);
        int withdraw_reservation(int id);
        int reserveasset(int id, int ied, DateTime res_date, DateTime sdate, DateTime edate,string status);

         List<maintenance_records> maintenancedetails();
         List<reservations> reservationdetails();

         int withdraw_maintenance(int id);


    }
}
