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
        int Addasset(asset assets);
        bool Updateasset(string status,int id,string location);
        bool Deleteasset(int id);

        List<asset> Getassets();

        bool Assetexists(int id);






    }
}
