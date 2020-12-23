#if !NET40
using System.IO.Compression;

namespace Sean.Utility.Compress
{
    /// <summary>
    /// Zip压缩\解压
    /// </summary>
    public class ZipFileHelper
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="sourceDirectoryName"></param>
        /// <param name="destinationArchiveFileName"></param>
        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
        {
            ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName);
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="sourceDirectoryName"></param>
        /// <param name="destinationArchiveFileName"></param>
        /// <param name="compressionLevel"></param>
        /// <param name="includeBaseDirectory"></param>
        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, includeBaseDirectory);
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="sourceArchiveFileName"></param>
        /// <param name="destinationDirectoryName"></param>
        public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
        {
            ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
        }
    }
}
#endif