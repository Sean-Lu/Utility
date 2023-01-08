using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace Sean.Utility.Compress.SharpZipLib
{
    /// <summary>
    /// Zip压缩\解压（基于ICSharpCode.SharpZipLib）
    /// </summary>
    public static class ZipHelper
    {
        private const int DefaultCompressionLevel = 6;

        #region 压缩
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="filePath">待压缩的文件</param>
        /// <param name="zipFilePath">压缩后的文件路径，为空时默认与文件同一级目录下，跟文件同名</param>
        /// <param name="password">密码</param>
        /// <param name="compressionLevel">压缩等级：[0(无压缩)-9(压缩率最高)]</param>
        /// <returns></returns>   
        public static bool ZipFile(string filePath, string zipFilePath, string password = null, int compressionLevel = DefaultCompressionLevel)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件不存在", filePath);
            }

            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                var zipFileDir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(zipFileDir))
                {
                    zipFilePath = Path.Combine(zipFileDir, $"{Path.GetFileNameWithoutExtension(filePath)}.zip");
                }

                if (string.IsNullOrWhiteSpace(zipFilePath))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(zipFilePath));
            }

            return ZipFilesOrDirectories(new List<string> { filePath }, zipFilePath, password, compressionLevel);
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dir">待压缩的文件夹</param>
        /// <param name="zipFilePath">压缩后的文件路径，为空时默认与文件夹同一级目录下，跟文件夹同名</param>
        /// <param name="password">密码</param>
        /// <param name="compressionLevel">压缩等级：[0(无压缩)-9(压缩率最高)]</param>
        /// <param name="onlySubItems">是否只压缩文件夹内的子文件或子文件夹</param>
        /// <returns></returns>   
        public static bool ZipDirectory(string dir, string zipFilePath, string password = null, int compressionLevel = DefaultCompressionLevel, bool onlySubItems = false)
        {
            if (string.IsNullOrWhiteSpace(dir))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dir));
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException($"文件夹不存在：{dir}");
            }

            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                var dirInfo = new DirectoryInfo(dir);
                zipFilePath = $"{dirInfo.FullName}.zip";
            }

            var filePathOrDirDic = new List<string>();
            if (!onlySubItems)
            {
                filePathOrDirDic.Add(dir);
            }
            else
            {
                var subFiles = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
                if (subFiles.Length > 0)
                {
                    filePathOrDirDic.AddRange(subFiles);
                }

                var subDirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
                if (subDirs.Length > 0)
                {
                    filePathOrDirDic.AddRange(subDirs);
                }
            }

            return ZipFilesOrDirectories(filePathOrDirDic, zipFilePath, password, compressionLevel);
        }

        /// <summary>
        /// 压缩多个文件或文件夹
        /// </summary>
        /// <param name="filePathOrDirList">待压缩的文件或文件夹路径</param>
        /// <param name="zipFilePath">压缩后的文件路径，不能为空</param>
        /// <param name="password">密码</param>
        /// <param name="compressionLevel">压缩等级：[0(无压缩)-9(压缩率最高)]</param>
        /// <returns></returns>
        public static bool ZipFilesOrDirectories(IEnumerable<string> filePathOrDirList, string zipFilePath, string password = null, int compressionLevel = DefaultCompressionLevel)
        {
            if (filePathOrDirList == null) throw new ArgumentNullException(nameof(filePathOrDirList));
            if (string.IsNullOrWhiteSpace(zipFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(zipFilePath));

            var data = filePathOrDirList.Select(c => new ContentToZip
            {
                Path = c
            });
            return ZipFilesOrDirectories(data, zipFilePath, password, compressionLevel);
        }

        /// <summary>
        /// 压缩多个文件或文件夹
        /// </summary>
        /// <param name="data">待压缩内容：文件或文件夹</param>
        /// <param name="zipFilePath">压缩后的文件路径，不能为空</param>
        /// <param name="password">密码</param>
        /// <param name="compressionLevel">压缩等级：[0(无压缩)-9(压缩率最高)]</param>
        /// <returns></returns>
        public static bool ZipFilesOrDirectories(IEnumerable<ContentToZip> data, string zipFilePath, string password = null, int compressionLevel = DefaultCompressionLevel)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(zipFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(zipFilePath));

            if (data.Count(c => !string.IsNullOrWhiteSpace(c.Path)) < 1)
            {
                return false;
            }

            var filePathList = new List<ContentToZip>();
            var dirList = new List<ContentToZip>();
            foreach (var item in data)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.Path))
                {
                    // 忽略空路径
                    continue;
                }

                var fileOrDir = item.Path;
                if (Directory.Exists(fileOrDir))
                {
                    dirList.Add(item);
                }
                else if (File.Exists(fileOrDir))
                {
                    filePathList.Add(item);
                }
                else
                {
                    throw new ArgumentException($"路径错误：{fileOrDir}");
                }
            }

            if (filePathList.Count < 1 && dirList.Count < 1)
            {
                return false;
            }

            bool result = true;

            //if (Path.GetExtension(zipFilePath).ToLower() != ".zip")
            //{
            //    zipFilePath = $"{zipFilePath}.zip";
            //}

            var zipFileDir = Path.GetDirectoryName(zipFilePath);
            if (!string.IsNullOrWhiteSpace(zipFileDir) && !Directory.Exists(zipFileDir))
            {
                Directory.CreateDirectory(zipFileDir);
            }

            using (var zipOutputStream = new ZipOutputStream(File.Create(zipFilePath)))
            {
                zipOutputStream.SetLevel(compressionLevel);
                if (!string.IsNullOrWhiteSpace(password))
                {
                    zipOutputStream.Password = password;
                }

                var errorMsg = "压缩操作失败：{0}";
                foreach (var fileOrDir in filePathList)
                {
                    // 压缩文件
                    if (!ZipFile(fileOrDir?.Path, zipOutputStream, fileOrDir?.RelativeFolder ?? string.Empty))
                    {
                        result = false;
                        errorMsg = string.Format(errorMsg, fileOrDir);
                        break;
                    }
                }

                if (result)
                {
                    foreach (var fileOrDir in dirList)
                    {
                        // 压缩文件夹
                        if (!ZipDirectory(fileOrDir?.Path, zipOutputStream, fileOrDir?.RelativeFolder ?? string.Empty))
                        {
                            result = false;
                            errorMsg = string.Format(errorMsg, fileOrDir);
                            break;
                        }
                    }
                }

                if (!result)
                {
                    if (File.Exists(zipFilePath))
                    {
                        File.Delete(zipFilePath);
                    }

                    throw new Exception(errorMsg);
                }

                zipOutputStream.Finish();
                zipOutputStream.Close();
                return result;
            }
        }
        #endregion

        #region 解压
        /// <summary>  
        /// 解压
        /// </summary>  
        /// <param name="zipFilePath">待解压的压缩文件</param>  
        /// <param name="unZipDir">解压后的文件夹</param>  
        /// <param name="password">密码</param>   
        /// <returns>是否成功</returns>  
        public static void UnZip(string zipFilePath, string unZipDir, string password = null)
        {
            var fastZip = new FastZip
            {
                Password = password
            };
            fastZip.ExtractZip(zipFilePath, unZipDir, FastZip.Overwrite.Always, null, null, null, true);
        }
        #endregion

        /// <summary>
        /// 向已存在的zip压缩文件添加文件
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="fileToZip">待添加的文件</param>
        /// <param name="relativeFolder">压缩包内相对文件夹，如：test/123</param>
        /// <returns></returns>
        public static bool AddFileToZip(string zipFilePath, string fileToZip, string relativeFolder = null)
        {
            //if (string.IsNullOrWhiteSpace(zipFilePath))
            //    throw new ArgumentException("Value cannot be null or whitespace.", nameof(zipFilePath));
            //if (string.IsNullOrWhiteSpace(fileToZip))
            //    throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileToZip));
            //if (!File.Exists(zipFilePath))
            //{
            //    throw new FileNotFoundException("文件不存在", zipFilePath);
            //}
            //if (!File.Exists(fileToZip))
            //{
            //    throw new FileNotFoundException("文件不存在", fileToZip);
            //}

            //using (var zipFile = new ZipFile(zipFilePath))
            //{
            //    zipFile.BeginUpdate();

            //    var fileName = !string.IsNullOrWhiteSpace(relativeFolder) ? Path.Combine(relativeFolder, Path.GetFileName(fileToZip)) : Path.GetFileName(fileToZip);
            //    zipFile.Add(new StaticDiskDataSource(fileToZip), fileName);

            //    zipFile.CommitUpdate();
            //}

            return AddFilesToZip(zipFilePath, new List<ContentToZip>
            {
                new ContentToZip
                {
                    Path = fileToZip,
                    RelativeFolder = relativeFolder
                }
            });
        }
        /// <summary>
        /// 向已存在的zip压缩文件批量添加文件
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="data">待压缩内容：文件</param>
        /// <returns></returns>
        public static bool AddFilesToZip(string zipFilePath, IEnumerable<ContentToZip> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(zipFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(zipFilePath));
            if (data.Count(c => !string.IsNullOrWhiteSpace(c.Path)) < 1)
            {
                return false;
            }
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("文件不存在", zipFilePath);
            }
            var filePathList = data.Where(c => !string.IsNullOrWhiteSpace(c.Path)).Select(c => c.Path);
            foreach (var fileToZip in filePathList)
            {
                if (!File.Exists(fileToZip))
                {
                    throw new FileNotFoundException("文件不存在", fileToZip);
                }
            }

            using (var zipFile = new ZipFile(zipFilePath))
            {
                zipFile.BeginUpdate();

                foreach (var item in data)
                {
                    var fileToZip = item.Path;
                    var relativeFolder = item.RelativeFolder;
                    if (string.IsNullOrWhiteSpace(fileToZip))
                    {
                        // 忽略空路径
                        continue;
                    }

                    var fileName = !string.IsNullOrWhiteSpace(relativeFolder) ? Path.Combine(relativeFolder, Path.GetFileName(fileToZip)) : Path.GetFileName(fileToZip);
                    zipFile.Add(new StaticDiskDataSource(fileToZip), fileName);
                }

                zipFile.CommitUpdate();
            }

            return true;
        }

        #region Private Methods
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">待压缩文件路径</param>
        /// <param name="zipOutputStream"></param>
        /// <param name="parentFolderName">压缩包内相对文件夹</param>
        /// <returns></returns>
        private static bool ZipFile(string fileToZip, ZipOutputStream zipOutputStream, string parentFolderName = "")
        {
            if (zipOutputStream == null) throw new ArgumentNullException(nameof(zipOutputStream));
            if (string.IsNullOrWhiteSpace(fileToZip))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileToZip));
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("文件不存在", fileToZip);
            }

            var result = true;
            try
            {
                var relativeFolder = string.Empty;
                if (!string.IsNullOrWhiteSpace(parentFolderName))
                {
                    // 创建文件夹
                    relativeFolder = Path.Combine("", $"{parentFolderName}/");
                    var zipEntry = new ZipEntry(relativeFolder)
                    {
                        DateTime = GetDirLastModifyTime("")
                    };
                    zipOutputStream.PutNextEntry(zipEntry);
                    zipOutputStream.Flush();
                }

                using (var fileStream = File.OpenRead(fileToZip))
                {
                    var fileName = !string.IsNullOrWhiteSpace(relativeFolder) ? Path.Combine(relativeFolder, Path.GetFileName(fileToZip)) : Path.GetFileName(fileToZip);
                    var zipEntry = new ZipEntry(fileName)
                    {
                        DateTime = GetFileLastModifyTime(fileToZip),
                        Size = fileStream.Length
                    };
                    zipOutputStream.PutNextEntry(zipEntry);

                    int sizeRead;
                    var buffer = new byte[2048];
                    while ((sizeRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        zipOutputStream.Write(buffer, 0, sizeRead);
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 压缩文件夹（递归）
        /// </summary>
        /// <param name="folderToZip">待压缩文件夹路径</param>
        /// <param name="zipOutputStream"></param>
        /// <param name="parentFolderName">压缩包内相对文件夹</param>
        private static bool ZipDirectory(string folderToZip, ZipOutputStream zipOutputStream, string parentFolderName = "")
        {
            if (zipOutputStream == null) throw new ArgumentNullException(nameof(zipOutputStream));
            if (string.IsNullOrWhiteSpace(folderToZip))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(folderToZip));
            if (!Directory.Exists(folderToZip))
            {
                throw new DirectoryNotFoundException($"文件夹不存在：{folderToZip}");
            }

            var result = true;
            try
            {
                //创建当前文件夹
                var relativeFolder = Path.Combine(parentFolderName, $"{Path.GetFileName(folderToZip)}/");
                var zipEntry = new ZipEntry(relativeFolder)
                {
                    DateTime = GetDirLastModifyTime(folderToZip)
                };
                zipOutputStream.PutNextEntry(zipEntry);
                zipOutputStream.Flush();

                //先压缩文件，再递归压缩文件夹
                var filePathArray = Directory.GetFiles(folderToZip);
                foreach (string filePath in filePathArray)
                {
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        zipEntry = new ZipEntry(Path.Combine(relativeFolder, Path.GetFileName(filePath)))
                        {
                            DateTime = GetFileLastModifyTime(filePath),
                            Size = fileStream.Length
                        };
                        zipOutputStream.PutNextEntry(zipEntry);

                        int sizeRead;
                        var buffer = new byte[2048];
                        while ((sizeRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            zipOutputStream.Write(buffer, 0, sizeRead);
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }

            var folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
            {
                if (!ZipDirectory(folder, zipOutputStream, Path.Combine(parentFolderName, Path.GetFileName(folderToZip))))
                {
                    return false;
                }
            }
            return result;
        }

        /// <summary>  
        /// 获取所有文件  
        /// </summary>  
        /// <returns></returns>  
        private static Hashtable GetAllFiles(string dir)
        {
            Hashtable filesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            GetAllFiles(fileDire, filesList);
            GetAllFiles(fileDire.GetDirectories(), filesList);
            return filesList;
        }
        /// <summary>  
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// </summary>  
        /// <param name="dirs"></param>  
        /// <param name="filesList"></param>  
        private static void GetAllFiles(IEnumerable<DirectoryInfo> dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                GetAllFiles(dir, filesList);
                GetAllFiles(dir.GetDirectories(), filesList);
            }
        }
        /// <summary>  
        /// 获取一个文件夹下的文件  
        /// </summary>  
        /// <param name="dir">目录名称</param>
        /// <param name="filesList">文件列表HastTable</param>  
        private static void GetAllFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }

        private static DateTime GetFileLastModifyTime(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return DateTime.Now;
            }

            var fileInfo = new FileInfo(filePath);
            return fileInfo.LastWriteTime;
        }

        private static long GetFileSize(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        private static DateTime GetDirLastModifyTime(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                return DateTime.Now;
            }

            var dirInfo = new DirectoryInfo(dir);
            return dirInfo.LastWriteTime;
        }
        #endregion
    }
}
