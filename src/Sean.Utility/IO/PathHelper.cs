using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Sean.Utility.Enums;

namespace Sean.Utility.IO
{
    /// <summary>
    /// 路径类
    /// </summary>
    public class PathHelper
    {
        private PathHelper() { }

        /// <summary>
        /// 获取备份文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetBackupFilePath(string filePath, out int index)
        {
            index = 1;
            while (true)
            {
                var backupFilePath = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}({index}){Path.GetExtension(filePath)}");
                if (!File.Exists(backupFilePath))
                {
                    return backupFilePath;
                }

                index++;
            }
        }

        /// <summary>
        /// 获取合法路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetValidPath(string path)
        {
            StringBuilder builder = new StringBuilder(path);
            foreach (char invalidChar in Path.GetInvalidPathChars())
                builder.Replace(invalidChar.ToString(), string.Empty);
            return builder.ToString();
        }
        /// <summary>
        /// 获取合法文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetValidFileName(string fileName)
        {
            StringBuilder builder = new StringBuilder(fileName);
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                builder.Replace(invalidChar.ToString(), string.Empty);
            return builder.ToString();
        }

#if !NETSTANDARD
        /// <summary>
        /// 判断文件是否为图片
        /// </summary>
        /// <param name="filePath">文件的完整路径</param>
        /// <returns></returns>
        public static bool IsImage(string filePath)
        {
            try
            {
                using (var img = Image.FromFile(filePath))
                {
                    // 释放占用图片的资源
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
#endif

        #region 获取所有文件夹、文件（递归遍历）
        /// <summary>
        /// 获取指定目录下的所有文件夹和文件
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="level">搜索级别（遍历）</param>
        /// <returns></returns>
        public static List<List<string>> GetAllDirAndFile(string dir, PathSearchLevel level)
        {
            List<List<string>> listRet = new List<List<string>>();
            List<string> listFileName = GetAllFile(dir);
            List<string> listDir = GetAllDir(dir);

            if (level == PathSearchLevel.All)
            {
                List<string> listDirTmp = new List<string>();
                //递归遍历全部文件夹和文件
                foreach (string strDirTmp in listDir)
                {
                    List<List<string>> listListTmp = GetAllDirAndFile(strDirTmp, level);
                    listDirTmp.AddRange(listListTmp[0]);
                    listFileName.AddRange(listListTmp[1]);
                }
                listDir.AddRange(listDirTmp);
            }

            listRet.Add(listDir);
            listRet.Add(listFileName);
            return listRet;
        }
        /// <summary>
        /// 获取指定目录下的所有文件夹（遍历1层）
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns>所有文件夹的完整路径</returns>
        public static List<string> GetAllDir(string dir)
        {
            return GetAllDir(new DirectoryInfo(dir));
        }
        /// <summary>
        /// 获取指定目录下的所有文件（遍历1层）
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns>所有文件的完整路径</returns>
        public static List<string> GetAllFile(string dir)
        {
            return GetAllFile(new DirectoryInfo(dir));
        }
        #endregion

        #region 扩展名相关
        /// <summary>
        /// 获取文件扩展名(包括“.”)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>包含指定路径的扩展名（包括“.”）的 System.String、null 或 System.string.Empty。如果 path 为 null，则GetExtension 返回 null。如果 path 不具有扩展名信息，则 GetExtension 返回 Empty。</returns>
        public static string GetExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }
        /// <summary>
        /// 更改路径字符串的扩展名
        /// </summary>
        /// <param name="path">文件路径。该路径不能包含在 System.IO.Path.GetInvalidPathChars() 中定义的任何字符。</param>
        /// <param name="extension">新的扩展名（有或没有前导句点）。指定 null 以从 path 移除现有扩展名。</param>
        /// <returns>返回修改后的文件路径</returns>
        public static string ChangeExtension(string path, string extension)
        {
            return Path.ChangeExtension(path, extension);
        }
        /// <summary>
        /// 修改文件扩展名
        /// </summary>
        /// <param name="filePath">文件路径。该路径不能包含在 System.IO.Path.GetInvalidPathChars() 中定义的任何字符。</param>
        /// <param name="extension">新的扩展名（有或没有前导句点）。指定 null 以从 path 移除现有扩展名。</param>
        /// <param name="delete">true：删除旧文件；false：保留旧文件。</param>
        public static void ChangeFileExtension(string filePath, string extension, bool delete)
        {
            string newFilePath = ChangeExtension(filePath, extension);
            if (delete)
                File.Move(filePath, newFilePath);
            else
                File.Copy(filePath, newFilePath);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 获取指定目录下的所有文件夹（遍历1层）
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns>所有文件夹的完整路径</returns>
        private static List<string> GetAllDir(DirectoryInfo dirInfo)
        {
            DirectoryInfo[] dirInfoArray = dirInfo.GetDirectories();
            return dirInfoArray.Select(directoryInfo => Path.Combine(dirInfo.FullName, directoryInfo.Name)).ToList();
        }
        /// <summary>
        /// 获取指定目录下的所有文件（遍历1层）
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns>所有文件的完整路径</returns>
        private static List<string> GetAllFile(DirectoryInfo dirInfo)
        {
            FileInfo[] fileInfoArray = dirInfo.GetFiles();
            return fileInfoArray.Select(fileInfo => Path.Combine(dirInfo.FullName, fileInfo.Name)).ToList();
        }
        #endregion
    }
}
