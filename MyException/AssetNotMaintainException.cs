using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.MyException
{
    internal class AssetNotMaintainException:ApplicationException
    {
        public AssetNotMaintainException(string message) : base(message) { }
    }
}
