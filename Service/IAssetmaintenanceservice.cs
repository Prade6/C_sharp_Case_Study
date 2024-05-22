using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Service
{
    internal interface IAssetmaintenanceservice
    {

        void Maintainasset();

        //void Removemaintenance();
        void Reserve();
        void Withrawreserve();
        void Getreserve();
        void Getmaintenance();

    }
}
