using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTResourcesTools
{
    class ResourceTools
    {

        public Cache cache;

        public ResourceTools()
        {
            Initialize();
        }

        private void Initialize()
        {
            cache = new Cache();

        }


        
        public void OnInitialize_Click()
        {
            try
            {
                Tools.Initialize(cache.pathData);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }
        public void OnExportTriple_Click()
        {
            try
            {
                Tools.CopyToSpecial(cache.pathData.targetPath);
                Tools.CopyToFirst(cache.pathData.targetPath);
                Tools.CopyToSecond(cache.pathData.targetPath);

                Tools.PackSpecialResource(cache.pathData.targetPath);
                Tools.PackFirstResource(cache.pathData.targetPath);
                Tools.PackSecondResource(cache.pathData.targetPath);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
            Process.Start("explorer.exe", cache.pathData.targetPath);
        }

        public void OnExportSubList_Click()
        {
            try
            {
                Tools.ExportSubResourceList(cache.pathData.targetPath);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }
        public void OnExportFullResList_Click()
        { 
            try
            {
                Tools.ExportFullResourceList(cache.pathData.targetPath);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }

        public void OnCombineResLists_Click()
        {
            try
            {
                Tools.ExportCombinedResourceList(cache.pathData.resourceListsFolder);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }


        private void label_ResourcePath_Click()
        {

        }

        public void OnExportFullMD5ResList_Click()
        {
            try
            {
                Tools.ExportFullMD5ResourceList(cache.pathData.targetPath);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }
        public void OnExportSubMD5List_Click()
        {
            try
            {
                Tools.ExportSubMD5ResourceList(cache.pathData.targetPath);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }

        private void OnExportDifferenceList()
        {
            try
            {
                string oldFilter = "";
                string newFilter = "";
                Tools.ExportDifMD5ResourceList(cache.pathData.targetPath, oldFilter, newFilter);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }

        private void OnExportNoApkList()
        {
            try
            {
                string Filter = "";
                Tools.ExportNoApkResourceList(cache.pathData.targetPath, Filter);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return;
            }
        }

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    string oldFilter = "";
        //    string newFilter = "";
        //    ChooseFileDialog("Choose Old Resource Filter", "json file (*.txt)|*.txt", ref oldFilter);
        //    ChooseFileDialog("Choose New Resource Filter", "json file (*.txt)|*.txt", ref newFilter);

        //    string[] strArrold = File.ReadAllLines(oldFilter);
        //    string[] strArrnew = File.ReadAllLines(newFilter);
        //    Tools.ExportDifflist(cache.pathData.targetPath, strArrold, strArrnew);


        //}
    }
}
