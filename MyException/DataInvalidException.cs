using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.MyException
{
    internal class DataInvalidException:ApplicationException
    {
        public DataInvalidException(string message) : base(message) { }
    }
}
