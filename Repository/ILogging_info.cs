using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Repository
{
    internal interface ILogging_info
    {
        bool password(string password);
        int register(string name, string dept, string email, string password);
        bool logging(int id, string pass);
    }
}
