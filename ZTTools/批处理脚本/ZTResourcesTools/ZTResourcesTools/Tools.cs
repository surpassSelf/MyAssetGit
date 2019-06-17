using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTResourcesTools
{
    class Tools
    { 

        public static PathData cacheData;

        private static List<Resource> fullResourceList;
        private static List<Resource> subResourceList;
        private static List<Resource> secondResourceList;
        private static List<Resource> firstResourceList;
        private static List<Resource> specialResourceList;
        private static Dictionary<string, Resource> resourceOrderDic;
        private static Dictionary<string, Resource> resourceDicForCombine;

        private static ResourceFilter subFilter;
        private static ResourceFilter firstFilter;
        private static ResourceFilter specialFilter;
        private static ResourceFilter IndependentFilter;
        public static ResourceFilter preFilter;

        public static void Clear()
        {
            fullResourceList.Clear();
            subResourceList.Clear();
            secondResourceList.Clear();
            firstResourceList.Clear();
            specialResourceList.Clear();
            resourceOrderDic.Clear();
            resourceDicForCombine.Clear();
        }

        #region Initialize
        public static void Initialize(PathData pathData)
        {
            cacheData = pathData;

            InitializeFilter();
            InitializeResourceList();
            if (resourceOrderDic == null)
                resourceOrderDic = new Dictionary<string, Resource>();
            else
                resourceOrderDic.Clear();

            if (resourceDicForCombine == null)
                resourceDicForCombine = new Dictionary<string, Resource>();
            else
                resourceDicForCombine.Clear();

            IndependentFilter = null;
            preFilter = null;
        }

        private static void InitializeFilter()
        {
            InitializeSubResourceFilter();
            InitializeFirstResourceFilter();
            InitializeSpecialResourceFilter();
        }
        private static void InitializeSubResourceFilter()
        {
            subFilter = new ResourceFilter(cacheData.subResFilter);
        }
        private static void InitializeFirstResourceFilter()
        {
            firstFilter = new ResourceFilter(cacheData.firstResFilter);
        }
        private static void InitializeSpecialResourceFilter()
        {
            specialFilter = new ResourceFilter(cacheData.specialResFilter);
        }

        private static void InitializeResourceList()
        {
            InitializeFullResList();
            InitializeSubResList();
            InitializeTrippleZipResList();
        }
        private static void InitializeFullResList()
        {
            if (fullResourceList == null)
                fullResourceList = new List<Resource>();
            else
                fullResourceList.Clear();

            string[] files = Directory.GetFiles(cacheData.resourcePath, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                if (file.Contains(".manifest"))
                    continue;
                Resource res = new Resource(file, cacheData.resourcePath);
                fullResourceList.Add(res);
            }
        }
        private static void InitializeSubResList()
        {
            if (subResourceList == null)
                subResourceList = new List<Resource>();
            else
                subResourceList.Clear();

            for (int i = 0; i < fullResourceList.Count; i++)
            {
                Resource res = fullResourceList[i];
                if (subFilter == null)
                    subFilter = new ResourceFilter(cacheData.subResFilter);

                if (subFilter.IsPassed(res.RelatePath))
                    subResourceList.Add(res);
            }
        }
        private static void InitializeTrippleZipResList()
        {
            if (firstResourceList == null)
                firstResourceList = new List<Resource>();
            else
                firstResourceList.Clear();
            if (secondResourceList == null)
                secondResourceList = new List<Resource>();
            else
                secondResourceList.Clear();
            if (specialResourceList == null)
                specialResourceList = new List<Resource>();
            else
                specialResourceList.Clear();

            for (int i = 0; i < subResourceList.Count; i++)
            {
                Resource res = subResourceList[i];

                if (firstFilter == null)
                    firstFilter = new ResourceFilter(cacheData.firstResFilter);
                if (firstFilter.IsPassed(res.RelatePath))
                {
                    if (specialFilter == null)
                        specialFilter = new ResourceFilter(cacheData.specialResFilter);
                    if (specialFilter.IsPassed(res.RelatePath))
                        specialResourceList.Add(res);
                    else
                        firstResourceList.Add(res);
                }
                else
                {
                    secondResourceList.Add(res);
                }
            }
        }
        #endregion

        public static void CopyToSpecial(string targetPath)
        {
            targetPath += "\\SpecialRes\\";

            for (int i = 0; i < specialResourceList.Count; i++)
            {
                Resource res = specialResourceList[i];
                res.CopyTo(targetPath);
            }
        }

        public static void CopyToFirst(string targetPath)
        {
            targetPath += "\\First\\";

            for (int i = 0; i < firstResourceList.Count; i++)
            {
                Resource res = firstResourceList[i];
                res.CopyTo(targetPath);
            }
        }

        public static void CopyToSecond(string targetPath)
        {
            targetPath += "\\Second\\";

            for (int i = 0; i < secondResourceList.Count; i++)
            {
                Resource res = secondResourceList[i];
                res.CopyTo(targetPath);
            }
        }

        public static void PackSpecialResource(string targetPath)
        {
            string target = targetPath + "\\SpecialRes.zip";
            string source = targetPath + "\\SpecialRes\\*";
            Compress(target, source);
        }
        public static void PackFirstResource(string targetPath)
        {
            string target = targetPath + "\\First.zip";
            string source = targetPath + "\\First\\*";
            Compress(target, source);
        }
        public static void PackSecondResource(string targetPath)
        {
            string target = targetPath + "\\Second.zip";
            string source = targetPath + "\\Second\\*";
            Compress(target, source);
        }
        private static void Compress(string target, string source)
        {
            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(source))
                return;

            Process process = new Process();
            process.StartInfo.FileName = cacheData.zipTool;
            process.StartInfo.Arguments = " a -tzip " + target + " -r " + source;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public static void PackFullResource()
        { }

        public static List<Resource> GetSubResources()
        {
            return subResourceList;
        }
        public static List<Resource> GetFirstResources()
        {
            return firstResourceList;
        }
        public static List<Resource> GetSecondResources()
        {
            return secondResourceList;
        }
        public static List<Resource> GetSpecialResources()
        {
            return specialResourceList;
        }

        public static void ExportSubResourceList(string targetPath)
        {
            ExportResourceList(subResourceList, targetPath, "\\ResListInApk.txt");
        }
        public static void ExportSubMD5ResourceList(string targetPath)
        {
            ExportResourceList(subResourceList, targetPath, "\\ResListMD5InApk.txt", false, true);
        }
        public static void ExportDifMD5ResourceList(string targetPath, string oldFilter, string newFilter)
        {
            Dictionary<string, string> resourceOld = LoadMD5Resource(oldFilter);
            Dictionary<string, string> resourceNew = LoadMD5Resource(newFilter);
            ExportDiffMd5ResourceList(resourceOld, resourceNew, targetPath, "\\ResListDiffMD5.txt");
        }
        public static void ExportNoApkResourceList(string targetPath, string filter)
        {
            OnExportNoApkResourceList(filter, targetPath, "\\ResListNoApk.txt");
        }


        public static void ExportFullResourceList(string targetPath)
        {
            LoadResourceOrder();
            Sort(fullResourceList);
            SetResourcesIndex(fullResourceList);
            ExportResourceList(fullResourceList, targetPath, "\\ResourceList.txt", true);
        }
        public static void ExportFullMD5ResourceList(string targetPath)
        {
            LoadResourceOrder();
            Sort(fullResourceList);
            SetResourcesIndex(fullResourceList);
            ExportResourceList(fullResourceList, targetPath, "\\ResourceMD5List.txt", true, true);
        }

        public static void ExportCombinedResourceList(string targetPath)
        {
            LoadResourceOrder();
            LoadAllResourceLists();
            List<Resource> resourceList = new List<Resource>(resourceDicForCombine.Values);
            Sort(resourceList);
            SetResourcesIndex(resourceList);
            ExportResourceList(resourceList, targetPath, "\\UpdateList.txt", true);
        }
        private static void LoadResourceOrder()
        {
            resourceOrderDic.Clear();

            using (FileStream fileStream = new FileStream(cacheData.resourceOrderList, FileMode.Open))
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, (int)fileStream.Length);
                string strData = Encoding.UTF8.GetString(bytes);
                string[] strArr = strData.Split('\n');
                for (int i = 0; i < strArr.Length; i++)
                {
                    string str = strArr[i];
                    if (str.Contains(".manifest"))
                        continue;
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str.Replace("\r", "");
                        if (!resourceOrderDic.ContainsKey(str))
                        {
                            Resource res = new Resource(str, i + 1);
                            resourceOrderDic.Add(str, res);
                        }
                    }
                }
            }
        }
        private static void Sort(List<Resource> resourceList)
        {
            resourceList.Sort((x, y) =>
            {
                if (x.IsImportant && !y.IsImportant)
                {
                    return -1;
                }
                else if (x.IsImportant && y.IsImportant)
                {
                    return x.RelatePath.CompareTo(y.RelatePath);
                }
                else if (!x.IsImportant && y.IsImportant)
                {
                    return 1;
                }
                else
                {
                    if (resourceOrderDic.ContainsKey(x.RelatePath) && resourceOrderDic.ContainsKey(y.RelatePath))
                    {
                        return resourceOrderDic[x.RelatePath].Index.CompareTo(resourceOrderDic[y.RelatePath].Index);
                    }
                    else if (resourceOrderDic.ContainsKey(x.RelatePath) && !resourceOrderDic.ContainsKey(y.RelatePath))
                    {
                        return -1;
                    }
                    else if (!resourceOrderDic.ContainsKey(x.RelatePath) && resourceOrderDic.ContainsKey(y.RelatePath))
                    {
                        return 1;
                    }
                    else
                    {
                        return x.RelatePath.CompareTo(y.RelatePath);
                    }
                }
            });
        }
        private static void SetResourcesIndex(List<Resource> resourceList)
        {
            for (int i = 0; i < resourceList.Count; i++)
            {
                Resource res = resourceList[i];
                if (res != null)
                {
                    res.SetIndex(i + 1);
                }
            }
        }
        private static void LoadAllResourceLists()
        {
            resourceDicForCombine.Clear();

            string[] resourceLists = Directory.GetFiles(cacheData.resourceListsFolder, "*.txt", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < resourceLists.Length; i++)
            {
                string path = resourceLists[i];
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int)fileStream.Length);

                    string strData = Encoding.UTF8.GetString(bytes);
                    InitResourceList(strData);
                }
            }
        }
        private static void InitResourceList(string strData)
        {
            string[] strArr = strData.Split('\n');

            for (int i = 0; i < strArr.Length; i++)
            {
                string res = strArr[i];
                if (res.Contains("manifest"))
                    continue;

                if (!string.IsNullOrEmpty(res))
                {
                    Resource resource = new Resource(res);

                    if (!resourceDicForCombine.ContainsKey(resource.RelatePath))
                    {
                        resourceDicForCombine.Add(resource.RelatePath, resource);
                    }
                }
            }
        }
        private static void ExportResourceList(List<Resource> resourceList, string targetPath, string fileName, bool isFull = false, bool isMd5 = false)
        {
            targetPath += fileName;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < resourceList.Count; i++)
            {
                if (isFull)
                {
                    string strTemp;
                    if (isMd5)
                    {
                        strTemp = resourceList[i].CombineMd5();
                    }
                    else
                    {
                        strTemp = resourceList[i].Combine();
                    }
                    sb.Append(strTemp);
                }
                else
                {
                    if (isMd5)
                    {
                        sb.Append(resourceList[i].CombineSubMD5());
                    }
                    else
                    {
                        string relatePath = resourceList[i].RelatePath;
                        sb.Append(relatePath).Append("\r\n");
                    }
                }
            }

            if (File.Exists(targetPath))
                File.Delete(targetPath);
            using (FileStream stream = new FileStream(targetPath, FileMode.Create))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
        }

        public static ResourceType GetResourceType(string relatePath)
        {
            if (string.IsNullOrEmpty(relatePath))
                return ResourceType.None;

            if (IndependentFilter == null)
                IndependentFilter = new ResourceFilter(cacheData.independentResFilter);

            if (IndependentFilter.IsPassed(relatePath))
                return ResourceType.Independent;
            else
                return ResourceType.Common;
        }
        public static ResourceOrderType GetResourceOrderType(string relatePath)
        {
            if (string.IsNullOrEmpty(relatePath))
                return ResourceOrderType.None;

            if (preFilter == null)
                preFilter = new ResourceFilter(cacheData.preDownloadFilter);

            if (preFilter.IsPassed(relatePath))
                return ResourceOrderType.Important;
            else
                return ResourceOrderType.Ordinary;
        }

        private static Dictionary<string, string> LoadMD5Resource(string targetPath)
        {
            Dictionary<string, string> resourcemd5 = new Dictionary<string, string>();

            using (FileStream fileStream = new FileStream(targetPath, FileMode.Open))
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, (int)fileStream.Length);
                string strData = Encoding.UTF8.GetString(bytes);
                string[] strArr = strData.Split('\n');
                for (int i = 0; i < strArr.Length; i++)
                {
                    string str = strArr[i];
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str.Replace("\r", "");
                        string[] strArrOne = str.Split('#');
                        if (strArr.Length <= 0) continue;
                        if (!resourcemd5.ContainsKey(strArrOne[0]))
                        {
                            resourcemd5.Add(strArrOne[0], strArrOne[strArrOne.Length - 1]);
                        }
                    }
                }
            }
            return resourcemd5;
        }
        private static void ExportDiffMd5ResourceList(Dictionary<string, string> resourceOldList, Dictionary<string, string> resourceNewList, string targetPath, string fileName)
        {
            targetPath += fileName;

            var resourcesDic = resourceNewList.GetEnumerator();
            List<string> diffList = new List<string>();
            while (resourcesDic.MoveNext())
            {
                if (resourceOldList.ContainsKey(resourcesDic.Current.Key))
                {
                    if (resourceOldList[resourcesDic.Current.Key] != resourcesDic.Current.Value)
                    {
                        diffList.Add(resourcesDic.Current.Key);
                    }
                }
                else
                {
                    diffList.Add(resourcesDic.Current.Key);
                }
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < diffList.Count; i++)
            {
                for (int j = 0; j < fullResourceList.Count; j++)
                {
                    if (diffList[i].Equals(fullResourceList[j].RelatePath))
                    {
                        fullResourceList[j].SetIndex(i + 1);
                        sb.Append(fullResourceList[j].Combine());
                        break;
                    }
                }
            }
            if (File.Exists(targetPath))
                File.Delete(targetPath);
            using (FileStream stream = new FileStream(targetPath, FileMode.Create))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
        }

        private static void OnExportNoApkResourceList(string filter, string targetPath, string fileName)
        {
            targetPath += fileName;

            string[] strArr = File.ReadAllLines(filter);
            StringBuilder sb = new StringBuilder();
            int index = 1;
            for (int i = 0; i < strArr.Length; i++)
            {
                string str = strArr[i];
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Replace("\r", "");
                    string[] strArrOne = str.Split('#');
                    bool isHave = false;
                    for (int j = 0; j < subResourceList.Count; j++)
                    {
                        if (strArrOne[0].Equals(subResourceList[j].RelatePath))
                        {
                            isHave = true;
                            break;
                        }
                    }
                    if (!isHave)
                    {
                        sb.Append(strArrOne[0]).Append("#").Append(strArrOne[1]).Append("#").Append(index).Append("#").Append(strArrOne[3]).
                            Append("#").Append(strArrOne[4]).Append("\r\n");
                        index++;
                    }
                }
            }
            if (File.Exists(targetPath))
                File.Delete(targetPath);
            using (FileStream stream = new FileStream(targetPath, FileMode.Create))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
        }

        public static void ExportDifflist(string targetPath, string[] oldlist, string[] newlist)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < newlist.Length; i++)
            {
                string[] newName = newlist[i].Split('#');
                bool ishave = false;
                for (int j = 0; j < oldlist.Length; j++)
                {
                    string[] oldname = oldlist[j].Split('#');
                    if (newName[0].Equals(oldname[0]))
                    {
                        ishave = true;
                        break;
                    }
                }
                if (!ishave)
                {
                    sb.Append(newlist[i]).Append("\r\n");
                }
            }
            targetPath = targetPath + "\\diffList.txt";
            if (File.Exists(targetPath))
                File.Delete(targetPath);
            using (FileStream stream = new FileStream(targetPath, FileMode.Create))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
        }
    }
}
