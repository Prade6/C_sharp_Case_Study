using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.MyException
{
    internal class DuplicateReservationException:ApplicationException
    {
        public DuplicateReservationException(string message) : base(message) { }
    }
}
