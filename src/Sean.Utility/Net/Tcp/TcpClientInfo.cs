using System.IO;
using System.Threading;

namespace Sean.Utility.Net.Tcp
{
    /// <summary>
    /// TCP客户端信息
    /// </summary>
    public class TcpClientInfo
    {
        /// <summary>
        /// TCP客户端
        /// </summary>
        public System.Net.Sockets.TcpClient TcpClient;
        /// <summary>
        /// 二进制读取器
        /// </summary>
        public BinaryReader Reader;
        /// <summary>
        /// 二进制写入器
        /// </summary>
        public BinaryWriter Writer;

        /// <summary>
        /// 接收数据的线程
        /// </summary>
        public Thread ThreadReceiveCommand;
        /// <summary>
        /// 发送文件的线程
        /// </summary>
        public Thread ThreadSendFile;
        /// <summary>
        /// 接收文件的线程
        /// </summary>
        public Thread ThreadReceiveFile;

        /// <summary>
        /// 文件流（待发送文件）
        /// </summary>
        public FileStream SendFileStream;
        /// <summary>
        /// 接收文件的信息
        /// </summary>
        public TcpFileInfo ReceiveFileInfo;
        /// <summary>
        /// 文件传输的缓存大小
        /// </summary>
        public int FileTransferBufferSize = 512;
    }
}