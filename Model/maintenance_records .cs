using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Model
{
    public class maintenance_records:Assets
    {
        int maintenance_id;
        Assets asset;
        DateTime maintenance_date;
        string description;
        decimal cost;

        public maintenance_records() { }

        public int Maintenance_records {  get { return maintenance_id; } set { maintenance_id = value; } }
        public Assets Asset { get { return asset; } set { asset = value;} }
        public string Description { get { return description; } set { description = value; } }
        public decimal Cost { get { return cost; } set { cost = value; } }
        public DateTime  Maintenance_date { get { return maintenance_date;}set { maintenance_date = value; } }

        public override string ToString()
        {
            return $"Asset_id:{Asset_id}Maintenance_date:{maintenance_date}Description:{description}Cost:{cost}";
        }

    }
}
