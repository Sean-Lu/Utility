using System;
using System.IO;

namespace Sean.Utility.IO
{
    public class DirectoryHelper
    {
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="oldDirectory"></param>
        /// <param name="newName"></param>
        public static void Rename(string oldDirectory, string newName)
        {
            var newDir = Path.Combine(Directory.GetParent(oldDirectory).FullName, newName);
            Directory.Move(oldDirectory, newDir);
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDirectoryName">源文件夹路径</param>
        /// <param name="destDirectoryName">目标文件夹路径</param>
        /// <param name="overwrite">是否覆盖</param>
        public static void Copy(string sourceDirectoryName, string destDirectoryName, bool overwrite = false)
        {
            if (!Directory.Exists(destDirectoryName))
            {
                Directory.CreateDirectory(destDirectoryName);
            }

            var files = Directory.GetFiles(sourceDirectoryName);
            foreach (var filePath in files)
            {
                //Console.WriteLine($"文件：{filePath}");
                var fileName = Path.GetFileName(filePath);
                File.Copy(filePath, Path.Combine(destDirectoryName, fileName), overwrite);
            }

            var subDirs = Directory.GetDirectories(sourceDirectoryName);
            foreach (var subDir in subDirs)
            {
                //Console.WriteLine($"文件夹：{subDir}");
                var dir = Path.GetFileName(subDir);
                Copy(subDir, Path.Combine(destDirectoryName, dir), overwrite);// 递归
            }
        }
    }
}
