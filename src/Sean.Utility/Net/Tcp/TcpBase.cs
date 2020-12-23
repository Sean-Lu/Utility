using System;
using System.IO;
using System.Threading;

namespace Sean.Utility.Net.Tcp
{
    /// <summary>
    /// TCP客户端、服务端基类
    /// </summary>
    public abstract class TcpBase
    {
        #region 常量
        #region 消息头
        /// <summary>
        /// 服务端发来文本信息
        /// </summary>
        public const string MsgServerMessage = "[ServerMessage]";
        /// <summary>
        /// 客户端发来文本信息
        /// </summary>
        public const string MsgClientMessage = "[ClientMessage]";
        /// <summary>
        /// 客户端主动断开连接
        /// </summary>
        public const string MsgClientDisconnect = "[ClientDisconnect]";
        /// <summary>
        /// 服务端关闭服务
        /// </summary>
        public const string MsgServerStoped = "[ServerStoped]";
        /// <summary>
        /// 客户端连接数量已达上限
        /// </summary>
        public const string MsgMaxClientCount = "[MaxClientCount]";
        /// <summary>
        /// 准备文件传输
        /// </summary>
        public const string MsgFileTransferReady = "[FileTransferReady]";
        /// <summary>
        /// 开始文件传输
        /// </summary>
        public const string MsgFileTransferNow = "[FileTransferNow]";
        /// <summary>
        /// 文件接收成功
        /// </summary>
        public const string MsgFileReceiveSuccess = "[FileReceiveSuccess]";
        /// <summary>
        /// 文件接收失败
        /// </summary>
        public const string MsgFileReceiveFail = "[FileReceiveFail]";
        #endregion

        #region 中间消息
        /// <summary>
        /// 文件名称
        /// </summary>
        public const string MsgFileName = "[<FileName:<##>>]";
        /// <summary>
        /// 文件长度，单位为字节（B）
        /// </summary>
        public const string MsgFileLength = "[<FileLength/B:<##>>]";
        #endregion
        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tcpClientInfo">客户端信息</param>
        public void SendMessage(string message, TcpClientInfo tcpClientInfo)
        {
            if (tcpClientInfo != null && tcpClientInfo.Writer != null)
            {
                tcpClientInfo.Writer.Write(message);
                tcpClientInfo.Writer.Flush();
            }
        }
        /// <summary>
        /// 发送文件（暂时不支持多文件）
        /// </summary>
        /// <param name="filePath">文件路径，多文件使用分号隔开</param>
        /// <param name="tcpClientInfo">TCP客户端信息</param>
        public void SendFile(string filePath, TcpClientInfo tcpClientInfo)
        {
            if (string.IsNullOrWhiteSpace(filePath) || tcpClientInfo == null) return;

            tcpClientInfo.SendFileStream = new FileStream(filePath, FileMode.Open);
            string fileName = Path.GetFileName(filePath);
            string fileLength = tcpClientInfo.SendFileStream.Length.ToString();//单位为字节
            SendMessage(MsgFileTransferReady + MsgFileName.Replace("##", fileName) + MsgFileLength.Replace("##", fileLength), tcpClientInfo);
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="tcpClientInfo">客户端信息</param>
        protected void SendFile(TcpClientInfo tcpClientInfo)
        {
            if (tcpClientInfo == null) throw new ArgumentNullException("tcpClientInfo");

            int size = 0;//初始化读取的流量为0   
            long len = 0;//初始化已经读取的流量  
            byte[] buffer = new byte[tcpClientInfo.FileTransferBufferSize];
            while (len < tcpClientInfo.SendFileStream.Length)
            {
                size = tcpClientInfo.SendFileStream.Read(buffer, 0, buffer.Length);
                tcpClientInfo.Writer.Write(buffer, 0, size);
                len += size;
            }
            tcpClientInfo.SendFileStream.Flush();
            tcpClientInfo.SendFileStream.Close();
            tcpClientInfo.Writer.Flush();
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        /// <param name="fileDownLoadSaveDir">文件下载目录</param>
        /// <param name="tcpClientInfo">客户端信息</param>
        protected void ReceiveFile(string fileDownLoadSaveDir, TcpClientInfo tcpClientInfo)
        {
            if (tcpClientInfo == null) throw new ArgumentNullException("tcpClientInfo");

            int size = 0;
            long len = 0;
            string fileSavePath = Path.Combine(fileDownLoadSaveDir, tcpClientInfo.ReceiveFileInfo.FileName.Trim());
            if (!Directory.Exists(fileDownLoadSaveDir))
                Directory.CreateDirectory(fileDownLoadSaveDir);
            FileStream fs = new FileStream(fileSavePath, FileMode.OpenOrCreate, FileAccess.Write) { Position = 0 };
            byte[] buffer = new byte[tcpClientInfo.FileTransferBufferSize];
            while (len < tcpClientInfo.ReceiveFileInfo.FileLength)
            {
                size = tcpClientInfo.Reader.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, size);
                len += size;
            }
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 释放指定TCP客户端信息的资源
        /// </summary>
        /// <param name="tcpClientInfo">TCP客户端信息</param>
        protected void ReleaseTcpClientInfo(TcpClientInfo tcpClientInfo)
        {
            ReleaseTcpClientInfoWithoutCommand(tcpClientInfo);

            if (tcpClientInfo.ThreadReceiveCommand != null && tcpClientInfo.ThreadReceiveCommand.ThreadState != ThreadState.Aborted)
            {
                tcpClientInfo.ThreadReceiveCommand.Abort();
                tcpClientInfo.ThreadReceiveCommand = null;
            }
        }
        /// <summary>
        /// 释放指定TCP客户端信息的资源（不包括ThreadReceiveCommand）
        /// </summary>
        /// <param name="tcpClientInfo">TCP客户端信息</param>
        protected void ReleaseTcpClientInfoWithoutCommand(TcpClientInfo tcpClientInfo)
        {
            if (tcpClientInfo.ThreadSendFile != null && tcpClientInfo.ThreadSendFile.ThreadState != ThreadState.Aborted)
            {
                tcpClientInfo.ThreadSendFile.Abort();
                tcpClientInfo.ThreadSendFile = null;
            }
            if (tcpClientInfo.ThreadReceiveFile != null && tcpClientInfo.ThreadReceiveFile.ThreadState != ThreadState.Aborted)
            {
                tcpClientInfo.ThreadReceiveFile.Abort();
                tcpClientInfo.ThreadReceiveFile = null;
            }
            if (tcpClientInfo.Reader != null)
            {
                tcpClientInfo.Reader.Close();
                tcpClientInfo.Reader = null;
            }
            if (tcpClientInfo.Writer != null)
            {
                tcpClientInfo.Writer.Close();
                tcpClientInfo.Writer = null;
            }
            if (tcpClientInfo.TcpClient != null && tcpClientInfo.TcpClient.Connected)
            {
                tcpClientInfo.TcpClient.Client.Close();
                tcpClientInfo.TcpClient.Close();
                tcpClientInfo.TcpClient = null;
            }
            if (tcpClientInfo.SendFileStream != null)
            {
                tcpClientInfo.SendFileStream.Close();
                tcpClientInfo.SendFileStream = null;
            }
        }

        /// <summary>
        /// 解析文件传输信息（包含消息头）
        /// </summary>
        /// <param name="fileTransferInfo">待解析字符串（包含消息头）</param>
        /// <param name="fileInfo">文件信息</param>
        protected void ParseFileTransferInfo(string fileTransferInfo, out TcpFileInfo fileInfo)
        {
            if (fileTransferInfo != null && fileTransferInfo.StartsWith(MsgFileTransferReady))
            {
                //去除消息头的字符串
                string strDropHeader = fileTransferInfo.Substring(MsgFileTransferReady.Length);

                ParseFileTransferInfoWithoutHeader(strDropHeader, out fileInfo);
            }
            else
            {
                //设置默认值
                fileInfo = new TcpFileInfo
                {
                    FileName = string.Empty,
                    FileLength = -1
                };
            }
        }
        /// <summary>
        /// 解析文件传输信息（不包含消息头）
        /// </summary>
        /// <param name="fileTransferInfo">待解析字符串（不包含消息头）</param>
        /// <param name="fileInfo">文件信息</param>
        protected void ParseFileTransferInfoWithoutHeader(string fileTransferInfo, out TcpFileInfo fileInfo)
        {
            //设置默认值
            fileInfo = new TcpFileInfo
            {
                FileName = string.Empty,
                FileLength = -1
            };

            if (!string.IsNullOrWhiteSpace(fileTransferInfo))
            {
                int nIndex = -1;

                //解析文件名称
                nIndex = fileTransferInfo.IndexOf(MsgFileName.Substring(0, MsgFileName.IndexOf(":")));
                if (nIndex >= 0)
                {
                    fileInfo.FileName = GetMsg(fileTransferInfo, nIndex);
                }

                //解析文件长度
                nIndex = fileTransferInfo.IndexOf(MsgFileLength.Substring(0, MsgFileLength.IndexOf(":")));
                if (nIndex >= 0)
                {
                    Int64.TryParse(GetMsg(fileTransferInfo, nIndex), out fileInfo.FileLength);
                }
            }
        }

        /// <summary>
        /// 获取消息内容
        /// </summary>
        /// <param name="str">消息</param>
        /// <param name="index">索引位置</param>
        /// <returns></returns>
        private string GetMsg(string str, int index)
        {
            string strRet = string.Empty;
            string strTmp = string.Empty;
            string strStart = "[<";
            string strSplit = ":<";
            string strEnd = ">>]";

            strTmp = str.Substring(index);
            strTmp = strTmp.Substring(0, strTmp.IndexOf(strEnd) + strEnd.Length);
            strTmp = strTmp.Remove(0, strStart.Length);
            strTmp = strTmp.Remove(strTmp.Length - strEnd.Length, strEnd.Length);
            string[] arrayFileName = strTmp.Split(new[] { strSplit }, StringSplitOptions.None);
            if (arrayFileName.Length > 1)
                strRet = arrayFileName[1];
            return strRet;
        }
    }
}
