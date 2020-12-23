using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Sean.Utility.Net.Ftp
{
    /// <summary>
    /// FTP客户端
    /// </summary>
    public class FtpClient
    {
        #region Fields
        /// <summary>
        /// FTP请求对象
        /// </summary>
        private FtpWebRequest _request;
        /// <summary>
        /// FTP响应对象
        /// </summary>
        private FtpWebResponse _response;
        #endregion

        #region Properties
        /// <summary>
        /// FTP服务器地址
        /// </summary>
        public string FtpUri { get; private set; }
        /// <summary>
        /// FTP服务器IP地址
        /// </summary>
        public string FtpServerIp { get; private set; }
        /// <summary>
        /// FTP服务器目录
        /// </summary>
        public string FtpRemotePath { get; private set; }
        /// <summary>
        /// FTP服务器用户名
        /// </summary>
        public string FtpUserId { get; private set; }
        /// <summary>
        /// FTP服务器密码
        /// </summary>
        public string FtpPassword { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ftpServerIp">FTP服务器IP地址</param>
        /// <param name="ftpRemotePath">指定FTP连接成功后的当前目录。如果为空则为根目录。</param>
        /// <param name="ftpUserId">FTP服务器用户名。如果为空则使用匿名身份验证(直接访问)，否则使用基本身份验证(登录访问)。</param>
        /// <param name="ftpPassword">FTP服务器密码</param>
        public FtpClient(string ftpServerIp, string ftpRemotePath, string ftpUserId, string ftpPassword)
        {
            FtpServerIp = ftpServerIp;
            FtpRemotePath = ftpRemotePath;
            FtpUserId = ftpUserId;
            FtpPassword = ftpPassword;

            UpdateFtpUri();
        }
        /// <summary>
        /// 析构函数（释放资源）
        /// </summary>
        ~FtpClient()
        {
            if (_response != null)
            {
                _response.Close();
                _response = null;
            }
            if (_request != null)
            {
                _request.Abort();
                _request = null;
            }
        }

        /// <summary>
        /// 获取当前目录下的所有文件(不包含子文件)。
        /// </summary>
        /// <returns></returns>
        public List<FtpFileInfo> GetFiles()
        {
            return GetFiles("", false);
        }
        /// <summary>
        /// 获取指定目录下的所有文件(不包含子文件)。
        /// </summary>
        /// <param name="remoteDir">目录，如果为空则使用当前目录。</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns></returns>
        public List<FtpFileInfo> GetFiles(string remoteDir, bool absolute)
        {
            var list = GetFilesAndDirectories(remoteDir, absolute);
            return list.Where(ftpFileInfo => ftpFileInfo.IsDirectory == false).ToList();
        }
        /// <summary>
        /// 获取当前目录下的所有文件夹(不包含子文件夹)。
        /// </summary>
        /// <returns></returns>
        public List<FtpFileInfo> GetDirectories()
        {
            return GetDirectories("", false);
        }
        /// <summary>
        /// 获取指定目录下的所有文件夹(不包含子文件夹)。
        /// </summary>
        /// <param name="remoteDir">目录，如果为空则使用当前目录。</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns></returns>
        public List<FtpFileInfo> GetDirectories(string remoteDir, bool absolute)
        {
            var list = GetFilesAndDirectories(remoteDir, absolute);
            return list.Where(ftpFileInfo => ftpFileInfo.IsDirectory).ToList();
        }
        /// <summary>
        /// 获取当前目录下的所有文件和文件夹(不包含子文件和子文件夹)。
        /// </summary>
        /// <returns></returns>
        public List<FtpFileInfo> GetFilesAndDirectories()
        {
            return GetFilesAndDirectories("", false);
        }
        /// <summary>
        /// 获取指定目录下的所有文件和文件夹(不包含子文件和子文件夹)。
        /// </summary>
        /// <param name="remoteDir">目录，如果为空则使用当前目录。</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns></returns>
        public List<FtpFileInfo> GetFilesAndDirectories(string remoteDir, bool absolute)
        {
            var fileList = new List<FtpFileInfo>();
            string uri = !string.IsNullOrWhiteSpace(remoteDir) ? GetFtpUri(remoteDir, absolute, false)
                                                          : FtpUri;
            _response = Open(new Uri(uri), WebRequestMethods.Ftp.ListDirectoryDetails);
            using (var stream = _response.GetResponseStream())
            {
                if (stream != null)
                    using (var sr = new StreamReader(stream))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            //line的格式如下：
                            //07-28-17  05:03PM       <DIR>          Demo
                            //09-23-17  09:44AM                  324 test.txt
                            string[] split = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            var ftpFileInfo = new FtpFileInfo();
                            ftpFileInfo.IsDirectory = split[2].ToUpper().Equals("<DIR>");
                            ftpFileInfo.Name = split[split.Length - 1];
                            ftpFileInfo.Path = $"{(absolute ? remoteDir : $"{FtpRemotePath}/{remoteDir}")}/{ftpFileInfo.Name}";
                            ftpFileInfo.FullPath = $"{uri}{ftpFileInfo.Name}";
                            ftpFileInfo.FileLength = !ftpFileInfo.IsDirectory ? Convert.ToInt64(split[2]) : 0;
                            ftpFileInfo.ModifyTime = Convert.ToDateTime($"{DateTime.ParseExact(split[0], "MM-dd-yy", null):yyyy-MM-dd} {split[1]}");

                            fileList.Add(ftpFileInfo);
                        }
                    }
            }
            return fileList;
        }
        /// <summary>
        /// 获取当前目录下的所有文件和文件夹(包含子文件和子文件夹)。
        /// </summary>
        /// <returns></returns>
        public List<FtpFileInfo> GetAllFilesAndDirectories()
        {
            return GetAllFilesAndDirectories("", false);
        }
        /// <summary>
        /// 获取指定目录下的所有文件和文件夹(包含子文件和子文件夹)。
        /// </summary>
        /// <param name="remoteDir">目录，如果为空则使用当前目录。</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns></returns>
        public List<FtpFileInfo> GetAllFilesAndDirectories(string remoteDir, bool absolute)
        {
            List<FtpFileInfo> fileList = GetFilesAndDirectories(remoteDir, absolute);
            foreach (FtpFileInfo ftpFileInfo in fileList)
            {
                if (ftpFileInfo.IsDirectory && !string.IsNullOrWhiteSpace(ftpFileInfo.Path))
                {
                    ftpFileInfo.SubFile = GetAllFilesAndDirectories(ftpFileInfo.Path, true);//递归遍历
                }
            }
            return fileList;
        }

        /// <summary>
        /// 切换当前目录到根目录
        /// </summary>
        public void GotoRootDirectory()
        {
            GotoDirectory("", true);
        }
        /// <summary>
        /// 切换当前目录到指定目录
        /// </summary>
        /// <param name="toDir">目标目录</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns>是否执行成功</returns>
        public void GotoDirectory(string toDir, bool absolute)
        {
            FtpRemotePath = absolute || string.IsNullOrWhiteSpace(FtpRemotePath) ? toDir : $"{FtpRemotePath}/{toDir}";
            UpdateFtpUri();
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="remoteDir">目录</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns>是否执行成功</returns>
        public bool CreateDirectory(string remoteDir, bool absolute)
        {
            _response = Open(new Uri(GetFtpUri(remoteDir, absolute, false)), WebRequestMethods.Ftp.MakeDirectory);
            return true;
        }
        /// <summary>
        /// 删除目录(包括下面所有子目录和子文件)
        /// </summary>
        /// <param name="remoteDir">目录</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        public bool RemoveDirectory(string remoteDir, bool absolute)
        {
            List<FtpFileInfo> ftpFileInfos = GetFilesAndDirectories(remoteDir, absolute);
            foreach (FtpFileInfo ftpFileInfo in ftpFileInfos)
            {
                if (ftpFileInfo.IsDirectory)
                    RemoveDirectory(ftpFileInfo.Path, true);
                else
                    DeleteFile(ftpFileInfo.Path, true);
            }
            _response = Open(new Uri(GetFtpUri(remoteDir, absolute, false)), WebRequestMethods.Ftp.RemoveDirectory);
            return true;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="remoteFileName">要删除的文件名</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        public void DeleteFile(string remoteFileName, bool absolute)
        {
            _response = Open(new Uri(GetFtpUri(remoteFileName, absolute, true)), WebRequestMethods.Ftp.DeleteFile);
        }
        /// <summary>
        /// 重命名文件或文件夹名称
        /// </summary>
        /// <param name="curName">当前的文件或文件夹名称</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <param name="newName">新的文件或文件夹名称</param>
        public void ReName(string curName, bool absolute, string newName)
        {
            _request = OpenRequest(new Uri(GetFtpUri(curName, absolute, true)), WebRequestMethods.Ftp.Rename);
            _request.RenameTo = newName;
            _response = (FtpWebResponse)_request.GetResponse();
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <param name="newFileName">新的文件名称</param>
        public void MoveFile(string fileName, bool absolute, string newFileName)
        {
            ReName(fileName, absolute, newFileName);
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="remoteFileName">文件名称</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns>文件大小</returns>
        public long GetFileSize(string remoteFileName, bool absolute)
        {
            _request = OpenRequest(new Uri(GetFtpUri(remoteFileName, absolute, true)), WebRequestMethods.Ftp.GetFileSize);
            return _request.GetResponse().ContentLength;
        }

        /// <summary>
        /// 检查当前目录下的指定文件是否存在
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>存在返回true，不存在返回false。</returns>
        public bool IsFileExist(string fileName)
        {
            return IsFileExist(fileName, "", false);
        }
        /// <summary>
        /// 检查指定目录下的指定文件是否存在
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="remoteDir">目录，如果为空则使用当前目录。</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns>存在返回true，不存在返回false。</returns>
        public bool IsFileExist(string fileName, string remoteDir, bool absolute)
        {
            return GetFiles(remoteDir, absolute).Count(ftpFileInfo => ftpFileInfo.Name == fileName) > 0;
        }
        /// <summary>
        /// 检查当前目录下的指定文件夹是否存在
        /// </summary>
        /// <param name="directory">文件夹</param>
        /// <returns>存在返回true，不存在返回false。</returns>
        public bool IsDirectoryExist(string directory)
        {
            return IsDirectoryExist(directory, "", false);
        }
        /// <summary>
        /// 检查指定目录下的指定文件夹是否存在
        /// </summary>
        /// <param name="directory">文件夹</param>
        /// <param name="remoteDir">目录，如果为空则使用当前目录。</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <returns>存在返回true，不存在返回false。</returns>
        public bool IsDirectoryExist(string directory, string remoteDir, bool absolute)
        {
            return GetDirectories(remoteDir, absolute).Count(ftpFileInfo => ftpFileInfo.Name == directory) > 0;
        }

        /// <summary>
        /// 上传文件到当前目录
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        public void UploadFile(string localFilePath)
        {
            UploadFile(localFilePath, "", false);
        }
        /// <summary>
        /// 上传文件到指定目录
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        /// <param name="remoteDir">目录，如果为空则使用当前目录。</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        public void UploadFile(string localFilePath, string remoteDir, bool absolute)
        {
            FileInfo fileInfo = new FileInfo(localFilePath);
            _request = OpenRequest(new Uri(!string.IsNullOrWhiteSpace(remoteDir) ? GetFtpUri(remoteDir, absolute, false) + fileInfo.Name
                                                                            : GetFtpUri(fileInfo.Name, false, true)), WebRequestMethods.Ftp.UploadFile);
            _request.ContentLength = fileInfo.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            using (var fs = fileInfo.OpenRead())
            using (var stream = _request.GetRequestStream())
            {
                var contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen > 0)
                {
                    stream.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
            }
        }
        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="remoteFilePath">文件名称</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <param name="saveDir">本地文件保存目录</param>
        public bool DownloadFile(string remoteFilePath, bool absolute, string saveDir)
        {
            string fileName = Path.GetFileName(remoteFilePath);
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            using (FileStream fs = new FileStream(Path.Combine(saveDir, fileName), FileMode.Create))
            {
                _response = Open(new Uri(GetFtpUri(remoteFilePath, absolute, true)), WebRequestMethods.Ftp.DownloadFile);
                using (Stream stream = _response.GetResponseStream())
                {
                    int bufferSize = 2048;
                    byte[] buffer = new byte[bufferSize];
                    if (stream != null)
                    {
                        var readCount = stream.Read(buffer, 0, bufferSize);
                        while (readCount > 0)
                        {
                            fs.Write(buffer, 0, readCount);
                            readCount = stream.Read(buffer, 0, bufferSize);
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 建立FTP链接,返回响应对象
        /// </summary>
        /// <param name="uri">FTP地址</param>
        /// <param name="ftpMethod">操作命令</param>
        /// <returns></returns>
        private FtpWebResponse Open(Uri uri, string ftpMethod)
        {
            return (FtpWebResponse)OpenRequest(uri, ftpMethod).GetResponse();
        }
        /// <summary>
        /// 建立FTP链接,返回请求对象
        /// </summary>
        /// <param name="uri">FTP地址</param>
        /// <param name="ftpMethod">操作命令</param>
        private FtpWebRequest OpenRequest(Uri uri, string ftpMethod)
        {
            _request = (FtpWebRequest)WebRequest.Create(uri);
            _request.Method = ftpMethod;
            _request.UseBinary = true;
            _request.KeepAlive = false;
            if (!string.IsNullOrWhiteSpace(FtpUserId))
                _request.Credentials = new NetworkCredential(FtpUserId, FtpPassword);
            return _request;
        }

        /// <summary>
        /// 更新FTP服务器地址
        /// </summary>
        private void UpdateFtpUri()
        {
            FtpUri = string.Format("ftp://{0}/", FtpServerIp);
            if (!string.IsNullOrWhiteSpace(FtpRemotePath))
                FtpUri += FtpRemotePath + "/";
        }

        /// <summary>
        /// 获取FTP服务器地址
        /// </summary>
        /// <param name="remotePath">文件或文件夹路径</param>
        /// <param name="absolute">true：绝对路径；false：相对路径。</param>
        /// <param name="file">true：文件；false：文件夹。</param>
        /// <returns></returns>
        private string GetFtpUri(string remotePath, bool absolute, bool file)
        {
            if (string.IsNullOrWhiteSpace(remotePath)) return "";

            string ftpUri = absolute ? string.Format("ftp://{0}/{1}/", FtpServerIp, remotePath)
                                     : string.Format("{0}{1}/", FtpUri, remotePath);

            return file ? ftpUri.TrimEnd('/') : ftpUri;
        }
        #endregion
    }
}
