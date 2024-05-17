using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Model
{
    internal class asset_allocations:Assets
    {
        int allocation_id;
        Assets assets;
        Employees employee;
        DateTime allocation_date;
        DateTime return_date;

        public asset_allocations() { }

        public int Allocation_id {  get { return allocation_id; } set {  allocation_id = value; } }
        public Assets Assets { get { return assets; } set { assets = value;} }
        public Employees Employee { get { return employee; } set {  employee = value;} }
        public DateTime Allocation_date {  get { return allocation_date; } set { allocation_date = value; } }
        public DateTime Return_date { get { return return_date; } set { return_date = value; }}

        public override string ToString()
        {
            return $"Asset_id:{Asset_id}\tEmployee_id:{Employee_id}\tAllocation:{allocation_date}\tReturn Date:{return_date}";

        }

    }
}
