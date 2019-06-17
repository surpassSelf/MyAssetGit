using System;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTResourcesTools
{
    class PathData
    {
        public string resourcePath;
        public string targetPath;
        public string subResFilter;
        public string firstResFilter;
        public string specialResFilter;
        public string independentResFilter;
        public string preDownloadFilter;
        public string resourceOrderList;
        public string resourceListsFolder;
        public string zipTool;
    }

    class Cache
    {
        private string curFolder;
        private string cachePath;

        public PathData pathData;

        public Cache()
        {
            curFolder = Environment.CurrentDirectory;
            cachePath = curFolder + "\\Cache.json";
            pathData = new PathData();
            Initialize();
        }

        private void Initialize()
        {
            LoadCache();
        }

        private void ResetCache()
        {
            pathData.resourcePath = string.Empty;
            pathData.targetPath = string.Empty;
            pathData.subResFilter = string.Empty;
            pathData.firstResFilter = string.Empty;
            pathData.specialResFilter = string.Empty;
            pathData.independentResFilter = string.Empty;
            pathData.preDownloadFilter = string.Empty;
            pathData.resourceOrderList = string.Empty;
            pathData.resourceListsFolder = string.Empty;
            pathData.zipTool = string.Empty;
            SaveCache();
        }

        private void LoadCache()
        {
            if (string.IsNullOrEmpty(cachePath) || !File.Exists(cachePath))
            {
                ResetCache();
                return;
            }

            using (FileStream stream = new FileStream(cachePath, FileMode.Open))
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                string strData = Encoding.UTF8.GetString(bytes);
                ParseCache(strData);
            }
        }

        private void ParseCache(string strData)
        {
            if (string.IsNullOrEmpty(strData))
            {
                ResetCache();
                return;
            }

            try
            {
                pathData = JsonMapper.ToObject<PathData>(strData);
            }
            catch
            {
                ResetCache();
                return;
            }
        }

        public void SaveCache()
        {
            if (File.Exists(cachePath))
            {
                File.Delete(cachePath);
            }

            string jsonStr = JsonMapper.ToJson(pathData);
            using (FileStream stream = new FileStream(cachePath, FileMode.Create))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }

            Tools.cacheData = pathData;
        }
    }
}
