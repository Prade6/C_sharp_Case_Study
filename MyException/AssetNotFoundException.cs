using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.MyException
{
    internal class AssetNotFoundException:ApplicationException
    {
        public AssetNotFoundException(string message) : base(message) { }
    }
}
