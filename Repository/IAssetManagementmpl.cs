using Asset_management.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Asset_management.Repository
{
    public interface IAssetManagementmpl
    {
        int addasset(Assets assets);
        bool updateasset(string status,int id,string location);
        bool deleteasset(int id);

        List<Assets> getassets();

        bool assetexists(int id);






    }
}
