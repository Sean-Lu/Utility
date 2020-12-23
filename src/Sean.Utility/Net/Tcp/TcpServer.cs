using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Sean.Utility.Net.Tcp
{
    /// <summary>
    /// Tcp服务端
    /// </summary>
    public class TcpServer : TcpBase
    {
        #region 公有字段
        /// <summary>
        /// 保存所有已连接客户端的数据字典
        /// </summary>
        public Dictionary<string, TcpClientInfo> TcpClientInfos = new Dictionary<string, TcpClientInfo>();
        /// <summary>
        /// 服务端端口
        /// </summary>
        public int ServerPort = -1;
        /// <summary>
        /// 文件下载保存目录。默认：@".\DownLoad"
        /// </summary>
        public string FileDownLoadSaveDir = @".\DownLoad";
        /// <summary>
        /// 最大客户端连接数量（默认为-1）。大于0表示限制连接数量；等于0表示不允许客户端连接；小于0表示不限制连接数量。
        /// </summary>
        public int MaxClientCount = -1;
        #endregion

        #region 私有字段
        /// <summary>
        /// TCP服务端监听器
        /// </summary>
        private TcpListener _tcpListener;
        /// <summary>
        /// 接收客户端连接的线程
        /// </summary>
        private Thread _threadReceiveClient;
        /// <summary>
        /// 标志TCP服务端的服务是否停止
        /// </summary>
        private bool _serverStoped = true;
        #endregion

        #region 公有方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpServer()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverPort">服务端端口</param>
        public TcpServer(int serverPort)
        {
            ServerPort = serverPort;
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        public void StartServer()
        {
            if (!_serverStoped)
                return;

            if (ServerPort > 0)
            {
                _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), ServerPort);
                _tcpListener.Start();
                _serverStoped = false;
                _threadReceiveClient = new Thread(ReceiveClient) { IsBackground = true };
                _threadReceiveClient.Start();
            }
        }
        /// <summary>
        /// 关闭服务
        /// </summary>
        public void StopServer()
        {
            if (_serverStoped)
                return;

            try
            {
                SendMessageToAll(MsgServerStoped, true);
            }
            catch
            {
            }
            _serverStoped = true;

            // 释放资源
            if (_threadReceiveClient != null && _threadReceiveClient.ThreadState != ThreadState.Aborted)
            {
                _threadReceiveClient.Abort();
                _threadReceiveClient = null;
            }

            foreach (KeyValuePair<string, TcpClientInfo> tcpClientInfo in TcpClientInfos)
            {
                DisconnectClient(tcpClientInfo.Value);
            }
            TcpClientInfos.Clear();

            if (_tcpListener != null)
            {
                _tcpListener.Stop();
                _tcpListener = null;
            }
        }
        /// <summary>
        /// 断开指定客户端的连接
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        public void DisconnectClient(string clientName)
        {
            DisconnectClient(GetTcpClientInfo(clientName));
        }
        /// <summary>
        /// 断开指定客户端的连接（释放资源）
        /// </summary>
        /// <param name="tcpClientInfo">TCP客户端信息</param>
        public void DisconnectClient(TcpClientInfo tcpClientInfo)
        {
            if (tcpClientInfo != null)
            {
                string strClientName = tcpClientInfo.TcpClient.Client.RemoteEndPoint.ToString();

                //释放资源
                ReleaseTcpClientInfo(tcpClientInfo);
                if (TcpClientInfos.ContainsKey(strClientName))
                    TcpClientInfos.Remove(strClientName);
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="clientName">客户端名称</param>
        /// <param name="customHeader">是否自定义消息头，默认false</param>
        public void SendMessage(string message, string clientName, bool customHeader = false)
        {
            SendMessage(customHeader ? message : MsgServerMessage + message, GetTcpClientInfo(clientName));
        }
        /// <summary>
        /// 发送消息（群发）
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="customHeader">是否自定义消息头，默认false</param>
        public void SendMessageToAll(string message, bool customHeader = false)
        {
            foreach (KeyValuePair<string, TcpClientInfo> tcpClientInfo in TcpClientInfos)
            {
                SendMessage(customHeader ? message : MsgServerMessage + message, tcpClientInfo.Value);
            }
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="filePath">文件路径，多文件使用分号隔开</param>
        /// <param name="clientName">客户端名称</param>
        public void SendFile(string filePath, string clientName)
        {
            SendFile(filePath, GetTcpClientInfo(clientName));
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 接收客户端连接
        /// </summary>
        private void ReceiveClient()
        {
            string strClientName = string.Empty;//客户端名称

            while (true)
            {
                try
                {
                    //限制客户端连接数量
                    if (MaxClientCount >= 0 && TcpClientInfos.Count >= MaxClientCount)
                    {
                        OnMaxClientCount(MaxClientCount);
                        return;
                    }

                    System.Net.Sockets.TcpClient tcpClient = _tcpListener.AcceptTcpClient(); //阻塞等待客户端连接
                    strClientName = tcpClient.Client.RemoteEndPoint.ToString();
                    NetworkStream networkStream = tcpClient.GetStream();

                    TcpClientInfo clientInfo = new TcpClientInfo();
                    clientInfo.TcpClient = tcpClient;
                    clientInfo.Writer = new BinaryWriter(networkStream);
                    clientInfo.Reader = new BinaryReader(networkStream);
                    clientInfo.ThreadReceiveCommand = new Thread(ReceiveCommand) { IsBackground = true };
                    clientInfo.ThreadReceiveCommand.Start(strClientName); //也可以直接传递TcpClient对象
                    TcpClientInfos[strClientName] = clientInfo;
                    OnClientConnectSuccess(strClientName);
                }
                catch (Exception err)
                {
                    if (!_serverStoped)
                        OnClientConnectFail(strClientName, err.Message);
                }
            }
        }
        /// <summary>
        /// 接收指定客户端数据
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        private void ReceiveCommand(object clientName)
        {
            TcpClientInfo clientInfo = GetTcpClientInfo((string)clientName);
            if (clientInfo == null) return;//结束线程

            while (true)
            {
                try
                {
                    string receiveString = clientInfo.Reader.ReadString();//阻塞等待客户端发送数据
                    if (receiveString.StartsWith(MsgClientMessage))
                    {
                        //客户端发来文本信息
                        OnReceiveClientMessage((string)clientName, receiveString.Substring(MsgClientMessage.Length));
                    }
                    else if (receiveString.StartsWith(MsgFileTransferReady))
                    {
                        //准备文件传输（接收文件）
                        base.ParseFileTransferInfo(receiveString, out clientInfo.ReceiveFileInfo);
                        SendMessage(MsgFileTransferNow, clientInfo);
                        clientInfo.ThreadReceiveFile = new Thread(ReceiveFile) { IsBackground = true };
                        clientInfo.ThreadReceiveFile.Start(clientName);
                        clientInfo.ThreadReceiveFile.Join();//阻塞等待线程结束
                    }
                    else if (receiveString.StartsWith(MsgFileTransferNow))
                    {
                        //开始文件传输（发送文件）
                        clientInfo.ThreadSendFile = new Thread(SendFile) { IsBackground = true };
                        clientInfo.ThreadSendFile.Start(clientName);
                        clientInfo.ThreadSendFile.Join();//阻塞等待线程结束
                    }
                    else if (receiveString.StartsWith(MsgClientDisconnect))
                    {
                        //客户端主动断开连接
                        ReleaseErrorClient(clientInfo);//释放资源（当前客户端）
                        OnClientDisconnect((string)clientName);
                        break;
                    }
                    else
                    {
                        //接收到未知消息
                        OnReceiveUnknownMessage((string)clientName, receiveString);
                    }
                }
                catch (Exception error)
                {
                    ReleaseErrorClient(clientInfo);//释放资源（当前客户端）

                    SocketException socketException = (SocketException)(error.InnerException);
                    if (socketException != null)
                    {
                        SocketError socketError = socketException.SocketErrorCode;
                        if ((int)socketError == 10054)
                        {
                            //客户端强制断开连接（如：强制关闭客户端）
                            OnClientForceDisconnect((string)clientName);
                            break;
                        }
                    }

                    OnClientConnectError((string)clientName, error.Message);
                    break;
                }
            }
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        private void SendFile(object clientName)
        {
            TcpClientInfo clientInfo = GetTcpClientInfo((string)clientName);
            if (clientInfo == null) return;//结束线程

            bool bRet = false;
            string strError = string.Empty;
            try
            {
                base.SendFile(clientInfo);
                bRet = true;
            }
            catch (Exception e)
            {
                bRet = false;
                strError = e.Message;
                clientInfo.SendFileStream.Close();
            }
            if (bRet)
                OnSendFileSuccess((string)clientName, clientInfo.SendFileStream.Name);
            else
                OnSendFileFail((string)clientName, strError);
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        private void ReceiveFile(object clientName)
        {
            TcpClientInfo clientInfo = GetTcpClientInfo((string)clientName);
            if (clientInfo == null) return;//结束线程

            bool bRet = false;
            string strError = string.Empty;
            try
            {
                base.ReceiveFile(FileDownLoadSaveDir, clientInfo);
                bRet = true;
            }
            catch (Exception e)
            {
                bRet = false;
                strError = e.Message;
            }
            if (bRet)
                OnReceiveFileSuccess((string)clientName, Path.GetFullPath(Path.Combine(FileDownLoadSaveDir, clientInfo.ReceiveFileInfo.FileName.Trim())));
            else
                OnReceiveFileFail((string)clientName, strError);
        }
        /// <summary>
        /// 根据客户端名称获取客户端信息
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        private TcpClientInfo GetTcpClientInfo(string clientName)
        {
            return !string.IsNullOrWhiteSpace(clientName) ? (TcpClientInfos.ContainsKey(clientName) ? TcpClientInfos[clientName] : null) : null;
        }
        /// <summary>
        /// 释放出错TCP客户端
        /// </summary>
        /// <param name="tcpClientInfo">TCP客户端</param>
        private void ReleaseErrorClient(TcpClientInfo tcpClientInfo)
        {
            if (tcpClientInfo != null)
            {
                string strClientName = tcpClientInfo.TcpClient.Client.RemoteEndPoint.ToString();

                //释放资源
                base.ReleaseTcpClientInfoWithoutCommand(tcpClientInfo);
                if (TcpClientInfos.ContainsKey(strClientName))
                    TcpClientInfos.Remove(strClientName);
            }
        }
        #endregion

        #region 提供给子类重写的事件
        /// <summary>
        /// 客户端连接成功
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        protected virtual void OnClientConnectSuccess(string clientName) { }
        /// <summary>
        /// 客户端连接失败
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="error">错误信息</param>
        protected virtual void OnClientConnectFail(string clientName, string error) { }
        /// <summary>
        /// 客户端连接数量已达上限
        /// </summary>
        /// <param name="maxClientCount">最大客户端连接数量</param>
        protected virtual void OnMaxClientCount(int maxClientCount) { }

        /// <summary>
        /// 客户端发来文本信息
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="message">消息</param>
        protected virtual void OnReceiveClientMessage(string clientName, string message) { }
        /// <summary>
        /// 接收到未知消息
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="message">消息</param>
        protected virtual void OnReceiveUnknownMessage(string clientName, string message) { }
        /// <summary>
        /// 文件发送成功
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="filePath">文件路径</param>
        protected virtual void OnSendFileSuccess(string clientName, string filePath) { }
        /// <summary>
        /// 文件发送失败
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="error">错误信息</param>
        protected virtual void OnSendFileFail(string clientName, string error) { }
        /// <summary>
        /// 文件接收成功
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="filePath">文件路径</param>
        protected virtual void OnReceiveFileSuccess(string clientName, string filePath) { }
        /// <summary>
        /// 文件接收失败
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="error">错误信息</param>
        protected virtual void OnReceiveFileFail(string clientName, string error) { }
        /// <summary>
        /// 客户端主动断开连接
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        protected virtual void OnClientDisconnect(string clientName) { }
        /// <summary>
        /// 客户端强制断开连接（如：强制关闭客户端）
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        protected virtual void OnClientForceDisconnect(string clientName) { }
        /// <summary>
        /// 与客户端的连接出错
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <param name="error">错误信息</param>
        protected virtual void OnClientConnectError(string clientName, string error) { }
        #endregion
    }
}
