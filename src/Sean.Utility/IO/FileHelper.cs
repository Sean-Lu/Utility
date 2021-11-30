using System;
using System.IO;
using System.Diagnostics;

namespace Sean.Utility.IO
{
    public class FileHelper
    {
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="oldFilePath"></param>
        /// <param name="newName"></param>
        public static void Rename(string oldFilePath, string newName)
        {
            var newFilePath = Path.Combine(Path.GetDirectoryName(oldFilePath), newName);
            File.Move(oldFilePath, newFilePath);
        }

        /// <summary>
        /// 复制文件 <see cref="File.Copy(string,string,bool)"/>
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="overwrite">是否覆盖</param>
        public static void Copy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <summary>
        /// 获取文件大小（字节）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static long GetLength(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

        /// <summary>
        /// 通过资源管理器（explorer.exe）打开文件或文件夹，如果参数 <paramref name="path"/> 是url链接的话，则会调用默认浏览器打开url链接
        /// </summary>
        /// <param name="path">可选项：文件路径、文件夹路径、url链接</param>
        /// <param name="selected">是否定位文件或文件夹。如果为false且为文件，则会调用默认程序打开文件</param>
        /// <returns></returns>
        public static Process OpenByExplorer(string path, bool selected = false)
        {
            using (var process = Process.Start("explorer.exe", $"{(selected ? "/e,/select," : string.Empty)}{path}"))
            {
                return process;
            }
        }
    }
}
