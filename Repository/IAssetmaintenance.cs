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
        int Maintenance(int id, DateTime date, string des, decimal price);
        int Withdraw_reservation(int id);
        int Reserveasset(int id, int ied, DateTime res_date, DateTime sdate, DateTime edate);

         List<maintenance_record> Maintenancedetails();
         List<reservation> Reservationdetails();

         //int Withdraw_maintenance(int id);


    }
}
