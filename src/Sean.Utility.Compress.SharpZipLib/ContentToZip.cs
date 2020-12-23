namespace Sean.Utility.Compress.SharpZipLib
{
    /// <summary>
    /// 待压缩内容
    /// </summary>
    public class ContentToZip
    {
        /// <summary>
        /// 待压缩的文件或文件夹路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 压缩包内相对文件夹（可以为空），示例：test/123
        /// </summary>
        public string RelativeFolder { get; set; }
    }
}
