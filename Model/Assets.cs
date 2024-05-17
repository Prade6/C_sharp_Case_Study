using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Asset_management.Model
{
    public class Assets:Employees
    {
        int asset_id;
        string name;
        string type;
        int serial_number;
        DateTime purchase_date;
        string location;
        string status;
        int owner_id;


        public Assets() { }

        public int Asset_id {  get { return asset_id; } set {  asset_id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Type { get { return type; } set { type = value; } }
        public int Serial_number { get { return serial_number; }set { serial_number = value; } }    
        public string Location { get { return location; } set { location = value; } }
        public string Status { get { return status; } set { status = value; } }
        public int Owner_id { get { return owner_id; } set { owner_id = value;} }
        public DateTime Purchase_date { get {  return purchase_date; } set {  purchase_date = value; } }

        public override string ToString()
        {
            return $"Asset_id:{asset_id}\tName:{name}\tType:{type}\tSerial_number:{serial_number}\tLocation:{location}\tPurchase date:{purchase_date}\tStatus:{status}";
        }
    }


}
