using System.IO;
using System.Diagnostics;

namespace Sean.Utility.IO
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper
    {
        private FileHelper() { }

        /// <summary>
        /// 获取指定目录下的所有文件（当前目录和所有子目录）
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns></returns>
        public static string[] GetFiles(string dir)
        {
            return GetFiles(dir, SearchOption.AllDirectories);
        }
        /// <summary>
        /// 获取指定目录下的所有文件
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="searchOption">搜索模式</param>
        /// <returns></returns>
        public static string[] GetFiles(string dir, SearchOption searchOption)
        {
            return Directory.GetFiles(dir, "*.*", searchOption);
        }
        /// <summary>
        /// 返回指定目录中所有文件和子目录的名称
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns></returns>
        public static string[] GetFileSystemEntries(string dir)
        {
            return Directory.GetFileSystemEntries(dir);
        }

        #region 判断文件夹或文件是否存在
        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        /// <param name="create">如果文件夹不存在是否创建</param>
        /// <returns>文件夹路径是否存在</returns>
        public static bool IsDirExist(string dir, bool create = false)
        {
            bool bRet = Directory.Exists(dir);
            if (!bRet && create)
            {
                Directory.CreateDirectory(dir);
            }
            return bRet;
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath">文件</param>
        /// <param name="create">如果文件不存在是否创建</param>
        /// <returns>文件是否存在</returns>
        public static bool IsFileExist(string filePath, bool create = false)
        {
            bool fileExist = File.Exists(filePath);
            if (!fileExist && create)
            {
                File.Create(filePath);
            }
            return fileExist;
        }
        #endregion

        #region 打开文件、文件夹、URL
        /// <summary>
        /// 调用默认程序打开指定文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static Process OpenFile(string filePath)
        {
            return Process.Start(filePath);
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录。默认当前目录"."</param>
        /// <returns></returns>
        private static Process OpenFile(string fileName, string arguments, string workingDirectory = ".")
        {
            ProcessStartInfo proInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = workingDirectory
            };
            Process pro;
            try
            {
                pro = Process.Start(proInfo);
            }
            catch
            {
                return null;
            }
            return pro;
        }
        /// <summary>
        /// 打开指定的系统文件（WorkingDirectory = @"C:\Windows";）
        /// </summary>
        /// <param name="fileName">文件名(记事本:notepad.exe;计算器:calc.exe;资源管理器:explorer.exe;...)</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        public static Process OpenFromSystem(string fileName, string arguments = "")
        {
            //C:\Windows\System：16位的库和应用程序
            //C:\Windows\System32：32位、64位的库和应用程序
            //C:\Windows\SysWOW64：32位的库和应用程序
            return OpenFile(fileName, arguments, @"C:\Windows");
        }
        /// <summary>
        /// 在资源管理器中打开文件或文件夹
        /// </summary>
        /// <param name="path">文件或文件夹路径</param>
        /// <param name="selected">是否定位文件或文件夹。如果为false且为文件，则会调用默认程序打开文件</param>
        /// <returns></returns>
        public static Process OpenInExplorer(string path, bool selected)
        {
            return OpenFromSystem("explorer.exe", (selected ? "/e,/select," : "") + Path.GetFullPath(path));
        }

        /// <summary>
        /// 调用默认浏览器打开指定url
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public static void OpenUrl(string url)
        {
            //方式1：调用系统默认的浏览器
            //Process.Start("explorer.exe", url);

            //方式2：调用系统默认的浏览器
            Process.Start(url);
        }
        /// <summary>
        /// 调用IE浏览器打开指定url
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public static void OpenUrlWithIe(string url)
        {
            //调用IE浏览器 
            Process.Start("iexplore.exe", url);
        }
        #endregion

        #region 删除文件、文件夹
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            File.Delete(filePath);
        }
        /// <summary>
        /// 删除文件夹（包含所有子文件夹和子文件）
        /// </summary>
        /// <param name="dir">待删文件夹</param>
        public static void DeleteFolder(string dir)
        {
            if (!Directory.Exists(dir)) return;

            Directory.Delete(dir, true);//参数2：true表示强制删除(即使有子文件夹或子文件)；false表示仅当文件夹为空时才删除。
        }
        /// <summary>
        /// 删除文件夹（包含所有子文件夹和子文件）
        /// </summary>
        /// <param name="dir">待删文件夹</param>
        public static void DeleteFolder2(string dir)
        {
            if (!Directory.Exists(dir)) return;

            foreach (string path in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(path))
                    File.Delete(path);
                else
                    DeleteFolder(path); //递归删除子文件夹   
            }
            Directory.Delete(dir); //删除已空文件夹   
        }
        #endregion

        #region 复制文件夹
        /// <summary>
        /// 复制文件夹（递归）
        /// </summary>
        /// <param name="fromPath">源文件夹</param>
        /// <param name="toPath">目标文件夹</param>
        public static bool CopyFolder(string fromPath, string toPath)
        {
            if (!Directory.Exists(fromPath)) return false;

            fromPath = fromPath.TrimEnd('\\');//".\\test\\" -> ".\\test"
            toPath = toPath.TrimEnd('\\');//".\\test\\" -> ".\\test"
            //取得要拷贝的文件夹名  
            string strFolderName = fromPath.Substring(fromPath.LastIndexOf("\\") + 1, fromPath.Length - fromPath.LastIndexOf("\\") - 1);
            //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹  
            if (!Directory.Exists(toPath + "\\" + strFolderName))
            {
                Directory.CreateDirectory(toPath + "\\" + strFolderName);
            }
            //创建数组保存源文件夹下的文件名  
            string[] strFiles = Directory.GetFiles(fromPath);
            //循环拷贝文件  
            for (int i = 0; i < strFiles.Length; i++)
            {
                //取得拷贝的文件名，只取文件名，地址截掉。  
                string strFileName = strFiles[i].Substring(strFiles[i].LastIndexOf("\\") + 1, strFiles[i].Length - strFiles[i].LastIndexOf("\\") - 1);
                //开始拷贝文件,true表示覆盖同名文件  
                File.Copy(strFiles[i], toPath + "\\" + strFolderName + "\\" + strFileName, true);
            }

            //创建DirectoryInfo实例  
            DirectoryInfo dirInfo = new DirectoryInfo(fromPath);
            //取得源文件夹下的所有子文件夹名称  
            DirectoryInfo[] ziPath = dirInfo.GetDirectories();
            for (int j = 0; j < ziPath.Length; j++)
            {
                //获取所有子文件夹名  
                string strZiPath = fromPath + "\\" + ziPath[j].ToString();
                //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝  
                CopyFolder(strZiPath, toPath + "\\" + strFolderName);
            }
            return true;
        }
        #endregion

        #region 获取FileVersionInfo
        /// <summary>
        /// 获取指定文件的版本信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static FileVersionInfo GetFileVersionInfo(string filePath)
        {
            return FileVersionInfo.GetVersionInfo(filePath);
        }
        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return GetFileVersionInfo(filePath).FileName;
        }
        /// <summary>
        /// 获取产品名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetProductName(string filePath)
        {
            return GetFileVersionInfo(filePath).ProductName;
        }
        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetCompanyName(string filePath)
        {
            return GetFileVersionInfo(filePath).CompanyName;
        }
        /// <summary>
        /// 获取文件版本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileVersion(string filePath)
        {
            return GetFileVersionInfo(filePath).FileVersion;
        }
        /// <summary>
        /// 获取产品版本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetProductVersion(string filePath)
        {
            return GetFileVersionInfo(filePath).ProductVersion;
        }
        /// <summary>
        /// 获取主版本号
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static int GetProductMajorPart(string filePath)
        {
            return GetFileVersionInfo(filePath).ProductMajorPart;
        }
        /// <summary>
        /// 获取次版本号
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static int GetProductMinorPart(string filePath)
        {
            return GetFileVersionInfo(filePath).ProductMinorPart;
        }
        /// <summary>
        /// 获取内部版本号
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static int GetProductBuildPart(string filePath)
        {
            return GetFileVersionInfo(filePath).ProductBuildPart;
        }
        /// <summary>
        /// 获取修订号（专用部件号）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static int GetProductPrivatePart(string filePath)
        {
            return GetFileVersionInfo(filePath).ProductPrivatePart;
        }
        /// <summary>
        /// 获取版本号：主版本号.次版本号[.内部版本号[.修订号]]
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetVersion(string filePath)
        {
            return string.Format("{0}.{1}.{2}.{3}", GetProductMajorPart(filePath), GetProductMinorPart(filePath), GetProductBuildPart(filePath), GetProductPrivatePart(filePath));
        }
        /// <summary>
        /// 获取文件说明
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileDescription(string filePath)
        {
            return GetFileVersionInfo(filePath).FileDescription;
        }
        /// <summary>
        /// 获取文件语言
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetLanguage(string filePath)
        {
            return GetFileVersionInfo(filePath).Language;
        }
        /// <summary>
        /// 获取原始文件名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetOriginalFilename(string filePath)
        {
            return GetFileVersionInfo(filePath).OriginalFilename;
        }
        /// <summary>
        /// 获取文件版权
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetLegalCopyright(string filePath)
        {
            return GetFileVersionInfo(filePath).LegalCopyright;
        }
        #endregion

        #region 获取FileInfo
        /// <summary>
        /// 获取指定文件的文件信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string filePath)
        {
            return new FileInfo(filePath);
        }
        /// <summary>
        /// 获取文件大小（字节）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static long GetFileLength(string filePath)
        {
            return GetFileInfo(filePath).Length;
        }
        #endregion

        #region 重命名文件夹或文件
        /// <summary>
        /// 重命名文件（文件要有后缀名）。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="newName">新的名称</param>
        public static void RenameFile(string filePath, string newName)
        {
            string strNewFilePath = Path.Combine(Path.GetDirectoryName(filePath), newName);
            File.Move(filePath, strNewFilePath);
        }
        /// <summary>
        /// 重命名文件夹。
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="newName">新的名称</param>
        public static void RenameDirectory(string dir, string newName)
        {
            string strNewDir = Path.Combine(Directory.GetParent(dir).FullName, newName);
            Directory.Move(dir, strNewDir);
        }
        #endregion
    }
}
