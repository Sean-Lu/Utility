using System;
using System.Collections.Generic;

namespace Sean.Utility.Net.Ftp
{
    /// <summary>
    /// FTP文件信息
    /// </summary>
    public class FtpFileInfo
    {
        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool IsDirectory { get; set; }
        /// <summary>
        /// 文件或文件夹名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件或文件夹路径（不包含FTP服务器IP地址）
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 完整的文件或文件夹路径（包含FTP服务器IP地址）
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 文件长度（单位：字节）
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 修改时间（精确到分钟）
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 子文件 或 子文件夹
        /// </summary>
        public List<FtpFileInfo> SubFile { get; set; }
    }
}