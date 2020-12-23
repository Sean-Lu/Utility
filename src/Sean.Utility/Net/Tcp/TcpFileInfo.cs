namespace Sean.Utility.Net.Tcp
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public struct TcpFileInfo
    {
        /// <summary>
        /// 文件路径。示例："E:\1.txt"
        /// </summary>
        public string FilePath;
        /// <summary>
        /// 文件名称。示例："1.txt"
        /// </summary>
        public string FileName;
        /// <summary>
        /// 文件扩展名（后缀名）。示例："txt"
        /// </summary>
        public string FileExtension;
        /// <summary>
        /// 文件总长度（大小）
        /// </summary>
        public long FileLength;
    }
}