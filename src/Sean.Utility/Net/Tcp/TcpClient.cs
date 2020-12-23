using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Sean.Utility.Net.Tcp
{
    /// <summary>
    /// Tcp客户端
    /// </summary>
    public class TcpClient : TcpBase
    {
        #region 公有字段
        /// <summary>
        /// 服务端IP地址。默认："127.0.0.1"
        /// </summary>
        public string ServerIp = "127.0.0.1";
        /// <summary>
        /// 服务端端口
        /// </summary>
        public int ServerPort = -1;
        /// <summary>
        /// 文件下载保存目录。默认：@".\DownLoad"
        /// </summary>
        public string FileDownLoadSaveDir = @".\DownLoad";
        #endregion

        #region 私有字段
        /// <summary>
        /// TCP客户端信息
        /// </summary>
        private TcpClientInfo _tcpClientInfo = new TcpClientInfo();
        #endregion

        #region 属性
        /// <summary>
        /// 是否已成功连接到服务端
        /// </summary>
        public bool Connected
        {
            get { return _tcpClientInfo.TcpClient != null && _tcpClientInfo.TcpClient.Connected; }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpClient()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverIp">服务端IP地址</param>
        /// <param name="serverPort">服务端端口</param>
        public TcpClient(string serverIp, int serverPort)
        {
            ServerIp = serverIp;
            ServerPort = serverPort;
        }

        /// <summary>
        /// 连接服务端
        /// </summary>
        public void Connect()
        {
            if (Connected) return;

            if (!string.IsNullOrWhiteSpace(ServerIp) && ServerPort > 0)
            {
                _tcpClientInfo.TcpClient = new System.Net.Sockets.TcpClient(ServerIp, ServerPort);
                if (Connected)
                {
                    //连接服务端成功
                    NetworkStream networkStream = _tcpClientInfo.TcpClient.GetStream();
                    _tcpClientInfo.Writer = new BinaryWriter(networkStream);
                    _tcpClientInfo.Reader = new BinaryReader(networkStream);
                    _tcpClientInfo.ThreadReceiveCommand = new Thread(ReceiveCommand) { IsBackground = true };
                    _tcpClientInfo.ThreadReceiveCommand.Start();
                }
            }
        }
        /// <summary>
        /// 断开与服务器的连接（释放资源）
        /// </summary>
        public void Disconnect()
        {
            try
            {
                SendMessage(MsgClientDisconnect, true);
            }
            catch
            {
            }
            // 释放资源
            ReleaseTcpClientInfo(_tcpClientInfo);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="customHeader">是否自定义消息头，默认false</param>
        public void SendMessage(string message, bool customHeader = false)
        {
            base.SendMessage(customHeader ? message : MsgClientMessage + message, _tcpClientInfo);
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="filePath">文件路径，多文件使用分号隔开</param>
        public void SendFile(string filePath)
        {
            base.SendFile(filePath, _tcpClientInfo);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 接收服务端数据
        /// </summary>
        private void ReceiveCommand()
        {
            while (true)
            {
                try
                {
                    string receiveString = _tcpClientInfo.Reader.ReadString();//阻塞等待服务端发送数据
                    if (receiveString.StartsWith(MsgServerMessage))
                    {
                        //服务端发来文本信息
                        OnReceiveServerMessage(receiveString.Substring(MsgServerMessage.Length));
                    }
                    else if (receiveString.StartsWith(MsgFileTransferReady))
                    {
                        //准备文件传输（接收文件）
                        ParseFileTransferInfo(receiveString, out _tcpClientInfo.ReceiveFileInfo);
                        SendMessage(MsgFileTransferNow, true);
                        _tcpClientInfo.ThreadReceiveFile = new Thread(ReceiveFile) { IsBackground = true };
                        _tcpClientInfo.ThreadReceiveFile.Start();
                        _tcpClientInfo.ThreadReceiveFile.Join();//阻塞等待线程结束
                    }
                    else if (receiveString.StartsWith(MsgFileTransferNow))
                    {
                        //开始文件传输（发送文件）
                        _tcpClientInfo.ThreadSendFile = new Thread(SendFile) { IsBackground = true };
                        _tcpClientInfo.ThreadSendFile.Start();
                        _tcpClientInfo.ThreadSendFile.Join();//阻塞等待线程结束
                    }
                    else if (receiveString.StartsWith(MsgServerStoped))
                    {
                        //服务端关闭服务
                        ReleaseTcpClientInfoWithoutCommand(_tcpClientInfo);
                        OnServerStoped();
                        break;
                    }
                    else
                    {
                        //接收到未知消息
                        OnReceiveUnknownMessage(receiveString);
                    }
                }
                catch (Exception error)
                {
                    //释放资源
                    ReleaseTcpClientInfoWithoutCommand(_tcpClientInfo);

                    SocketException socketException = (SocketException)(error.InnerException);
                    if (socketException != null)
                    {
                        SocketError socketError = socketException.SocketErrorCode;
                        if ((int)socketError == 10054)
                        {
                            //服务端强制断开连接（如：强制关闭服务端）
                            OnServerForceDisconnect();
                            break;
                        }
                    }

                    OnServerConnectError(error.Message);
                    break;
                }
            }
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        private void SendFile()
        {
            bool bRet = false;
            string strError = string.Empty;
            try
            {
                base.SendFile(_tcpClientInfo);
                bRet = true;
            }
            catch (Exception e)
            {
                bRet = false;
                strError = e.Message;
                _tcpClientInfo.SendFileStream.Close();
            }
            if (bRet)
                OnSendFileSuccess(_tcpClientInfo.SendFileStream.Name);
            else
                OnSendFileFail(strError);
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        private void ReceiveFile()
        {
            bool bRet = false;
            string strError = string.Empty;
            try
            {
                base.ReceiveFile(FileDownLoadSaveDir, _tcpClientInfo);
                bRet = true;
            }
            catch (Exception e)
            {
                bRet = false;
                strError = e.Message;
            }
            if (bRet)
                OnReceiveFileSuccess(Path.GetFullPath(Path.Combine(FileDownLoadSaveDir, _tcpClientInfo.ReceiveFileInfo.FileName.Trim())));
            else
                OnReceiveFileFail(strError);
        }
        #endregion

        #region 提供给子类重写的事件
        /// <summary>
        /// 服务端发来文本信息
        /// </summary>
        /// <param name="message">消息</param>
        protected virtual void OnReceiveServerMessage(string message) { }
        /// <summary>
        /// 接收到未知消息
        /// </summary>
        /// <param name="message">消息</param>
        protected virtual void OnReceiveUnknownMessage(string message) { }
        /// <summary>
        /// 文件发送成功
        /// </summary>
        /// <param name="filePath">文件路径</param>
        protected virtual void OnSendFileSuccess(string filePath) { }
        /// <summary>
        /// 文件发送失败
        /// </summary>
        /// <param name="error">错误信息</param>
        protected virtual void OnSendFileFail(string error) { }
        /// <summary>
        /// 文件接收成功
        /// </summary>
        /// <param name="filePath">文件路径</param>
        protected virtual void OnReceiveFileSuccess(string filePath) { }
        /// <summary>
        /// 文件接收失败
        /// </summary>
        /// <param name="error">错误信息</param>
        protected virtual void OnReceiveFileFail(string error) { }
        /// <summary>
        /// 服务端关闭服务
        /// </summary>
        protected virtual void OnServerStoped() { }
        /// <summary>
        /// 服务端强制断开连接（如：强制关闭服务端）
        /// </summary>
        protected virtual void OnServerForceDisconnect() { }
        /// <summary>
        /// 与服务端的连接出错
        /// </summary>
        /// <param name="error">错误信息</param>
        protected virtual void OnServerConnectError(string error) { }
        #endregion
    }
}
