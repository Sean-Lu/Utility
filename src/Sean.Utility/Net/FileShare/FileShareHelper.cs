using Sean.Utility.Common;
using System.Text;

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
            var error = new StringBuilder();
            CmdHelper.RunCmd(new[] { $@"net use {remotePath} /User:{username} {password} /PERSISTENT:YES" }, null, (sender, e) =>
           {
               if (e.Data != null)
               {
                   error.AppendLine(e.Data);
               }
           });
            return string.IsNullOrWhiteSpace(error.ToString());
        }

        /// <summary>
        /// 取消连接网络共享资源（所有）
        /// </summary>
        /// <returns></returns>
        public static bool Disconnect()
        {
            var error = new StringBuilder();
            CmdHelper.RunCmd(new[] { @"net use * /del /y" }, null, (sender, e) =>
            {
                if (e.Data != null)
                {
                    error.AppendLine(e.Data);
                }
            });
            return string.IsNullOrWhiteSpace(error.ToString());
        }
    }
}
