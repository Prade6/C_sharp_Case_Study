﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Service
{
    internal interface IAssetTrackingservice
    {
        void Deallocateasset();
        void Allocateasset();

        void Allocationdetails();


    }
}
