using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Model
{
    public class reservations:Assets
    {
        int reservation_id;
        Assets asset;
        Employees employee;
        DateTime reservation_date;
        DateTime start_date;
        DateTime end_date;
        string status;

        public reservations() { }

        public int Reservation_id {  get { return reservation_id; } set { reservation_id = value; } } 
        public Assets Asset { get { return asset; }set { asset = value; } }
        public Employees Employee { get { return employee; } set {  employee = value; } }
        public DateTime Start_date { get { return start_date; } set { start_date = value; } }
        public DateTime End_date { get { return end_date; } set { end_date = value; }}
        public string Status { get { return status; } set { status = value; } }
        public DateTime Reservation_date { get { return reservation_date; } set { reservation_date = value; } }


        public override string ToString()
        {
            return $"Asset_id:{Asset_id}\tEmployee_id:{Employee_id}\tStart Date:{Start_date}\tEnd_Date:{End_date}\tStatus:{Status}\tReservation_date:{Reservation_date}";
        }


    }

    
}
