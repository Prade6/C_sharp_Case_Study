using Asset_management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    internal interface IAssettracking
    {
        int Allocateasset(int id, int aid, DateTime date,DateTime enddate);
        int Deallocate(int id, int aid, DateTime date);

        List<asset_allocation> Allocationdetails();

    }
}
