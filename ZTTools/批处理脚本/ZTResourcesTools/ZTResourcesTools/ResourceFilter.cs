using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTResourcesTools
{
    class ResourceFilter
    {
        private string filter;
        private string filterStr;

        List<string> keyWordList;
        Dictionary<string, byte> resourceDic;

        public ResourceFilter(string filter)
        {
            this.filter = filter;
            this.filterStr = string.Empty;

            keyWordList = new List<string>();
            resourceDic = new Dictionary<string, byte>();

            Initialize();
        }

        private void Initialize()
        {
            filterStr = LoadFilter();
            ParseData();
        }

        private string LoadFilter()
        {
            string filterStr = string.Empty;
            using (FileStream stream = new FileStream(filter, FileMode.Open))
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                filterStr = Encoding.UTF8.GetString(bytes);
            }

            return filterStr;
        }

        private void ParseData()
        {
            JsonData filterJson = JsonMapper.ToObject(filterStr);
            JsonData jsonArr = filterJson["KeyWords"];
            foreach (JsonData data in jsonArr)
            {
                string keyWord = (string)data;
                keyWordList.Add(keyWord);
            }
            jsonArr = filterJson["Resources"];
            foreach (JsonData data in jsonArr)
            {
                string resource = (string)data;
                if (!string.IsNullOrEmpty(resource))
                {
                    if (!resourceDic.ContainsKey(resource))
                    {
                        resourceDic.Add(resource, 0);
                    }
                }
            }
        }

        public bool IsPassed(string relateName)
        {
            if (string.IsNullOrEmpty(relateName))
            {
                return false;
            }

            if (resourceDic.ContainsKey(relateName))
            {
                return true;
            }
            else
            {
                for (int i = 0; i < keyWordList.Count; i++)
                {
                    string keyWord = keyWordList[i];
                    if (!string.IsNullOrEmpty(keyWord))
                    {
                        if (relateName.Contains(keyWord))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
