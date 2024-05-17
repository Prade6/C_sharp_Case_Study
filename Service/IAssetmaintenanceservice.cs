using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Service
{
    internal interface IAssetmaintenanceservice
    {

        void maintainasset();

        void removemaintenance();
        void reserve();
        void withrawasset();
        void getreserve();
        void getmaintenance();

    }
}
