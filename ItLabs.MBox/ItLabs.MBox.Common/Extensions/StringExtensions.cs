using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StructureMap.AutoMocking;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateUrl(this string fileNameWithExt)
        {
            //var s3Manager = new IS3Manager();
            //return s3Manager.GetImageLink(fileNameWithExt);
            return "";
        }
    }
}
