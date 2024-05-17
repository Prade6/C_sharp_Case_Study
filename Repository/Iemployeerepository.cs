using Asset_management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    internal interface Iemployeerepository
    {
        bool updateemployee(int id, string pass);
        List<Employees> viewemployees();
        bool deleteemployee(int id);

        //int addemployee(Employees employees);

    }
}
