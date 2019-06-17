using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZTResourcesTools
{
    public enum ResourceType
    {
        None = -1,
        Common,
        Independent,
    }

    public enum ResourceOrderType
    {
        None = -1,
        Ordinary,
        Important,
    }

    class Resource
    {
        private string fullPath;
        private string relatePath;
        public string RelatePath { get { return relatePath; } }
        private string relateFolder;

        private string relatePath_Win;
        private string relateFolder_Win;

        private string relateMD5;

        int byteNum;
        int index;
        public int Index { get { return index; } }
        ResourceOrderType resourceOrder;
        ResourceType resourceType;

        public bool IsImportant { get { return resourceOrder == ResourceOrderType.Important; } }

        public Resource(string relatePath, int index)
        {
            this.relatePath = relatePath;
            this.index = index;
        }

        public Resource(string fullPath, string rootPath)
        {
            this.fullPath = fullPath;
            relatePath = GetRelatePath(rootPath, fullPath);
            relateFolder = GetRelateFolder(relatePath);
            byteNum = GetByteNum();
            index = 0;
            resourceType = GetResourceType();
            resourceOrder = GetResourceOrderType();
        }

        public Resource(string strData)
        {
            if (string.IsNullOrEmpty(strData))
                return;

            string[] strArr = strData.Split('#');

            if (strArr.Length < 3)
                return;

            if (strArr.Length >= 3)
            {
                relatePath = strArr[0];
                byteNum = int.Parse(strArr[1]);
                index = int.Parse(strArr[2]);
                resourceType = GetResourceType();
                resourceOrder = GetResourceOrderType();
            }
        }

        private string GetRelatePath(string rootPath, string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath) || string.IsNullOrEmpty(rootPath))
            {
                return string.Empty;
            }

            relatePath_Win = fullPath.Replace(rootPath, "");
            relatePath = relatePath_Win.Replace("\\", "/");
            relatePath = relatePath.Substring(1);
            return relatePath;
        }

        private string GetRelateFolder(string relatePath)
        {
            if (string.IsNullOrEmpty(relatePath))
            {
                return string.Empty;
            }

            int index = relatePath.LastIndexOf('/');
            if (index >= 0)
            {
                string folder = relatePath.Remove(index);
                relateFolder_Win = folder.Replace("\\", "/");
                return relatePath.Remove(index);
            }
            else
            {
                return string.Empty;
            }
        }

        private int GetByteNum()
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return 0;
            }

            int byteNum = 0;
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                byteNum = (int)stream.Length;
            }

            return byteNum;
        }

        private ResourceType GetResourceType()
        {
            return Tools.GetResourceType(relatePath);
        }

        private ResourceOrderType GetResourceOrderType()
        {
            return Tools.GetResourceOrderType(relatePath);
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public string Combine()
        {
            if (string.IsNullOrEmpty(relatePath))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(relatePath).Append('#').Append(byteNum).Append('#').Append(index).Append('#').Append((int)resourceType).Append('#').Append((int)resourceOrder).Append("\r\n");
            return sb.ToString();
        }

        public string CombineMd5()
        {
            if (string.IsNullOrEmpty(relatePath))
            {
                return string.Empty;
            }
            relateMD5 = GetResMD5();

            StringBuilder sb = new StringBuilder();
            sb.Append(relatePath).Append('#').Append(byteNum).Append('#').Append(index).Append('#').Append((int)resourceType).Append('#').Append((int)resourceOrder).Append('#')
                .Append(relateMD5).Append("\r\n");
            return sb.ToString();
        }

        public string CombineSubMD5()
        {
            if (string.IsNullOrEmpty(RelatePath))
            {
                return string.Empty;
            }
            relateMD5 = GetResMD5();
            StringBuilder sb = new StringBuilder();
            sb.Append(relatePath).Append('#').Append(relateMD5).Append("\r\n");
            return sb.ToString();
        }

        public void CopyTo(string target)
        {
            string targetPath = target + relatePath_Win;
            string targetFolder = target + relateFolder_Win;

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }

            File.Copy(fullPath, targetPath);
        }

        public string GetResMD5()
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return "";
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);
                stream.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
