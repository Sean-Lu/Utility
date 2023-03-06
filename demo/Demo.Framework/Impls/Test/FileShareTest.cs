using System;
using System.IO;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;
using Sean.Utility.Net;

namespace Demo.Framework.Impls.Test
{
    public class FileShareTest : ISimpleDo
    {
        public void Execute()
        {
            // 局域网读取共享文件的2种方式：
            // 1. FileShareHelper：通过WNetAddConnection2A API将共享目录映射为本地磁盘，之后即可按本地文件形式访问文件，最后断开连接。
            // 2. 使用net use命令：通过cmd运行“net use \\path /User:user password /PERSISTENT:YES​”命令，获取共享目录的权限，即可访问共享目录下的文件了。
        }

        #region FileShareHelper
        /// <summary>
        /// 示例1：localPath（本地目录）不为null
        /// </summary>
        private void Test1()
        {
            string strMsg = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}]：{{0}}";
            string localpath = "Z:";
            string remotePath = @"\\10.xx.xx.xx\Share";
            string username = @"\test";
            string password = "12345!a";
            // 连接
            int status = FileShareHelper.Connect(remotePath, localpath, username, password);
            if (status == (int)ERROR_ID.NO_ERROR)
            {
                // 连接成功
                Console.WriteLine(string.Format(strMsg, "Connect Success."));
                FileStream fs = new FileStream(localpath + @"\\log2.txt", FileMode.OpenOrCreate);
                fs.Seek(0, SeekOrigin.End);
                using (StreamWriter stream = new StreamWriter(fs))
                {
                    stream.WriteLine(strMsg, "Connect Success.");
                    stream.Flush();
                    stream.Close();
                }
                fs.Close();

                // 断开连接
                status = FileShareHelper.Disconnect(localpath);
                Console.WriteLine(status == (int)ERROR_ID.NO_ERROR
                    ? string.Format(strMsg, "Disconnect Success.")
                    : string.Format(strMsg, $"Disconnect Failed: {(Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())}"));
            }
            else
            {
                // 连接失败
                Console.WriteLine(string.Format(strMsg, $"Connect Failed: {(Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())}"));
            }
        }

        /// <summary>
        /// 示例2：localPath（本地目录）为null
        /// </summary>
        private void Test2()
        {
            string strMsg = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}]：" + "{0}";
            string remotePath = @"\\10.xx.xx.xx\Share";
            string username = @"\test";
            string password = "12345!a";
            // 连接
            int status = FileShareHelper.Connect(remotePath, null, username, password);
            if (status == (int)ERROR_ID.NO_ERROR)
            {
                // 连接成功
                Console.WriteLine(string.Format(strMsg, "Connect Success."));
                FileStream fs = new FileStream(remotePath + @"\\log1.txt", FileMode.OpenOrCreate);
                fs.Seek(0, SeekOrigin.End);
                using (StreamWriter stream = new StreamWriter(fs))
                {
                    stream.WriteLine(strMsg, "Connect Success.");
                    stream.Flush();
                    stream.Close();
                }
                fs.Close();

                // 断开连接
                status = FileShareHelper.Disconnect(remotePath);
                Console.WriteLine(status == (int)ERROR_ID.NO_ERROR
                    ? string.Format(strMsg, "Disconnect Success.")
                    : string.Format(strMsg, $"Disconnect Failed: {(Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())}"));
            }
            else
            {
                // 连接失败
                Console.WriteLine(string.Format(strMsg, $"Connect Failed: {(Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())}"));
            }
        }
        #endregion
    }
}
