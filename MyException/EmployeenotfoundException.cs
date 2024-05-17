using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.MyException
{
    internal class EmployeenotfoundException:ApplicationException
    {
        public EmployeenotfoundException(string message) : base(message) { }
    }
}
