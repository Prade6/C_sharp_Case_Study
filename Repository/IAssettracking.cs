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
        int allocateasset(int id, int aid, DateTime date,DateTime enddate);
        int deallocate(int id, int aid, DateTime date);

        List<asset_allocations> allocationdetails();

    }
}
