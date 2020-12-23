using Sean.Utility.Common;

namespace Sean.Utility.Net.FileShare
{
    /// <summary>
    /// 访问网络共享文件夹（基于net use命令）
    /// </summary> 
    public class FileShareHelper
    {
        /// <summary>
        /// 连接网络共享资源
        /// </summary>
        /// <param name="remotePath">共享目录</param>
        /// <returns></returns>
        public static bool Connect(string remotePath)
        {
            return Connect(remotePath, "", "");
        }

        /// <summary>
        /// 连接网络共享资源
        /// </summary>
        /// <param name="remotePath">共享目录</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool Connect(string remotePath, string username, string password)
        {
            var cmd = $@"net use {remotePath} /User:{username} {password} /PERSISTENT:YES";
            CmdHelper.RunCmd(cmd, out _, out var error);
            return string.IsNullOrWhiteSpace(error);
        }

        /// <summary>
        /// 取消连接网络共享资源（所有）
        /// </summary>
        /// <returns></returns>
        public static bool Disconnect()
        {
            CmdHelper.RunCmd(@"net use * /del /y", out _, out var error);
            return string.IsNullOrWhiteSpace(error);
        }
    }
}
