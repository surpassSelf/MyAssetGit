using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTResourcesTools
{
    class Program
    {
        static string strTargetPath = "";
        static string strResourcePath = "";
        static void Main(string[] args)
        {
            ResourceTools resTools = new ResourceTools();

            Console.WriteLine("Start Build Res!!!");
            Console.WriteLine(resTools.cache.pathData.resourcePath);
            resTools.OnInitialize_Click();
            Console.WriteLine("OnExportResZip:");
            resTools.OnExportTriple_Click();
            Console.WriteLine("OnExportResSubList:");
            resTools.OnExportSubList_Click();
            Console.WriteLine("OnExportResFullList:");
            resTools.OnExportFullResList_Click();
            Console.WriteLine("OnExportResSubMd5List:");
            resTools.OnExportSubMD5List_Click();
            Console.WriteLine("OnExportResFullMd5List:");
            resTools.OnExportFullMD5ResList_Click();
            Console.WriteLine("Build Res Success!!!");
            Console.ReadLine();
        }
    }
}
