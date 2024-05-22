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
        bool Updateemployee(int id, string pass,string email);
        List<employee> Viewemployees();
        bool Deleteemployee(int id);

    }
}
