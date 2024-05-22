using NUnit.Framework;
using Asset_management.Repository;
using Asset_management.Model;
using Microsoft.VisualBasic;
using System;
using Asset_management.MyException;
using NUnit.Framework.Legacy;

namespace Assetsapp.Tests

{
    public class Testing_asset
    {
        IAssetManagementmpl assetManagementRespository = new AssetManagementRespository();
        asset assets = new asset();

        IAssetmaintenance assetmaintenance = new AssetMaintenanceRepository();
        maintenance_record main = new maintenance_record();

        Logindetail_info login = new Logindetail_info();

        [Test]


        public void addasset()
        {

            assets.Name = "redmi";
            assets.Type = "phone";
            assets.Serial_number = 7;
            assets.Purchase_date = DateTime.Now;
            assets.Location = "karur";
            int result = assetManagementRespository.Addasset(assets);

            Assert.That(result, Is.EqualTo(1));

        }

        [Test]

        public void addmaintenance()
        {
            int Asset_id = 101;
            DateTime Maintenance_date = DateTime.Now;
            string Description = "System failure";
            decimal Cost = 5000;
            int result = assetmaintenance.Maintenance(Asset_id, Maintenance_date, Description, Cost);

            Assert.That(result, Is.EqualTo(1));

        }

        [Test]

        public void addreserve()
        {
            int id = 102;
            int empid = 3;
            DateTime res_date = Convert.ToDateTime("2024 - 04 - 20");
            DateTime sdate = Convert.ToDateTime("2024 - 05 - 20");
            DateTime edate = Convert.ToDateTime("2024 - 06 - 20");
            int result = assetmaintenance.Reserveasset(id, empid, res_date, sdate, edate);

            Assert.That(result, Is.EqualTo(1));
        }


        [Test]
        
        public void employeenotfoundexception()
        {
            int id = 110;
            var m = Assert.Throws<EmployeenotfoundException>(() => { login.Empoloyeeexists(id); });
            Assert.That(m.Message.ToString(),Is.EqualTo("\nEmployee id not found\n"));
        }


        [Test]

        public void assetnotfoundexception()
        {
            int id = 1190;
            var m = Assert.Throws<AssetNotFoundException>(() => { assetManagementRespository.Assetexists(id); });
            Assert.That(m.Message.ToString(), Is.EqualTo("Asset id not available"));
        }



    }
}
