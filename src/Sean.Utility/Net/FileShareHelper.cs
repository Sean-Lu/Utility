using System.Runtime.InteropServices;
using Sean.Utility.Enums;

namespace Sean.Utility.Net
{
    /// <summary>
    /// 使用磁盘映射的方式访问网络共享文件夹
    /// </summary> 
    public static class FileShareHelper
    {
        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="lpNetResource">网络资源</param>
        /// <param name="lpPassword">密码</param>
        /// <param name="lpUserName">用户名</param>
        /// <param name="dwFlags">设为零或CONNECT_UPDATE_PROFILE（表示创建永久性连接）</param>
        /// <returns></returns>
        [DllImport("mpr.dll")]
        public static extern int WNetAddConnection2A(NetResource[] lpNetResource, string lpPassword, string lpUserName, int dwFlags);
        /// <summary>
        /// 取消连接
        /// </summary>
        /// <param name="name">已连接资源的远程名称或本地名称（如果此参数指定了重定向本地设备，则该功能仅取消指定的设备重定向。 如果参数指定远程网络资源，则所有没有设备的连接都将被取消）</param>
        /// <param name="dwFlags">设为零或CONNECT_UPDATE_PROFILE。如为零，而且建立的是永久性连接，则在windows下次重新启动时仍会重新连接</param>
        /// <param name="fForce">如为TRUE，表示强制断开连接（即使连接的资源上正有打开的文件或作业）</param>
        /// <returns></returns>
        [DllImport("mpr.dll")]
        public static extern int WNetCancelConnection2A(string name, int dwFlags, int fForce);

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="remotePath">共享目录</param>
        /// <param name="localPath">本地目录（可以为null）</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>ERROR_ID</returns>
        public static int Connect(string remotePath, string localPath, string username, string password)
        {
            NetResource[] netResources = new NetResource[1];
            netResources[0].dwScope = RESOURCE_SCOPE.RESOURCE_GLOBALNET;
            netResources[0].dwType = RESOURCE_TYPE.RESOURCETYPE_DISK;
            netResources[0].dwDisplayType = RESOURCE_DISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE;
            netResources[0].dwUsage = RESOURCE_USAGE.RESOURCEUSAGE_CONNECTABLE;
            netResources[0].lpLocalName = localPath;
            netResources[0].lpRemoteName = remotePath;

            Disconnect(string.IsNullOrWhiteSpace(localPath) ? remotePath : localPath);

            return WNetAddConnection2A(netResources, password, username, 1);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="path">共享目录或本地目录（当本地目录不为null时，选择本地目录；当本地目录为null时，选择共享目录）</param>
        /// <returns>ERROR_ID</returns>
        public static int Disconnect(string path)
        {
            return WNetCancelConnection2A(path, 1, 1);
        }
    }

    /// <summary>
    /// 网络资源
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NetResource
    {
        public RESOURCE_SCOPE dwScope;
        public RESOURCE_TYPE dwType;
        public RESOURCE_DISPLAYTYPE dwDisplayType;
        public RESOURCE_USAGE dwUsage;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpLocalName;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpRemoteName;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpComment;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpProvider;
    }
}
