using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    internal interface ILogindetail_info
    {
        bool Password(string password);
        int Register(string name, string dept, string email, string password);
        bool Logging(int id, string pass);
    }
}
