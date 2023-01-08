using System;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Sean.Utility.Enums;

namespace Sean.Utility.Common
{
    public static class ComputerUtil
    {
        #region DllImport

        [DllImport("user32")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        [DllImport("user32")]
        private static extern void LockWorkStation();
        [DllImport("user32")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        #endregion

        /// <summary>
        /// 系统消息
        /// </summary>
        private const int WM_SYSCOMMAND = 0x112;
        /// <summary>
        /// 关闭显示器的系统命令
        /// </summary>
        private const int SC_MONITORPOWER = 0xF170;

        #region 获取计算机信息
        /// <summary> 
        /// 获取计算机名
        /// </summary> 
        /// <returns></returns> 
        public static string GetComputerName()
        {
            return Dns.GetHostName();//Environment.GetEnvironmentVariable("ComputerName");
        }
        #endregion

        #region 计算机操作：关机、重启、注销、锁定等
        /// <summary>
        /// 关机
        /// </summary>
        public static void Shutdown()
        {
            Process.Start(new ProcessStartInfo("shutdown.exe", "-s -t 00"));
        }

        /// <summary>
        /// 重启
        /// </summary>
        public static void Restart()
        {
            Process.Start(new ProcessStartInfo("shutdown.exe", "-r -t 00"));
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void LogOff()
        {
            ExitWindowsEx(0, 0);
        }

        /// <summary>
        /// 锁定计算机
        /// </summary>
        public static void Lock()
        {
            LockWorkStation();
        }

        /// <summary>
        /// 关闭显示器
        /// </summary>
        public static void TurnOffMonitor()
        {
            SetMonitorInState(MonitorState.Off);
        }
        /// <summary>
        /// 关闭显示器
        /// </summary>
        /// <param name="hWnd">句柄，如：this.Handle</param>
        public static void TurnOffMonitor(IntPtr hWnd)
        {
            SetMonitorInState(MonitorState.Off, hWnd);
        }

        /// <summary>
        /// 打开显示器
        /// </summary>
        public static void TurnOnMonitor()
        {
            SetMonitorInState(MonitorState.On);
        }
        /// <summary>
        /// 打开显示器
        /// </summary>
        /// <param name="hWnd">句柄，如：this.Handle</param>
        public static void TurnOnMonitor(IntPtr hWnd)
        {
            SetMonitorInState(MonitorState.On, hWnd);
        }
        #endregion

        #region 获取IP地址
        /// <summary>
        /// 获取本机IP地址（取第1个IPv4地址）
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpv4()
        {
            return GetAllLocalIpv4()?.FirstOrDefault();
        }

        /// <summary>
        /// 获取本机所有IPv4地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllLocalIpv4()
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                return hostEntry.AddressList.Where(c => c.AddressFamily == AddressFamily.InterNetwork).Select(c => c.ToString()).ToList();
            }
            catch
            {
                // ignored
            }

            return null;
        }
        /// <summary>
        /// 获取本机所有IPv4地址
        /// </summary>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string GetAllLocalIpv4(string separator)
        {
            try
            {
                return string.Join(separator, GetAllLocalIpv4());
            }
            catch
            {
                // ignored
            }

            return null;
        }

        /// <summary>
        /// 获取本机所有IPv6地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllLocalIpv6()
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                return hostEntry.AddressList.Where(c => c.AddressFamily == AddressFamily.InterNetworkV6).Select(c => c.ToString()).ToList();
            }
            catch
            {
                // ignored
            }

            return null;
        }
        #endregion

        /// <summary>
        /// 检测本机端口是否被TCP占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsTcpPortUsed(int port)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPointsTcp = ipProperties.GetActiveTcpListeners();
            return ipEndPointsTcp.Any(endPoint => endPoint.Port == port);
        }

        /// <summary>
        /// 检测本机端口是否被UDP占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsUdpPortUsed(int port)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPointsUdp = ipProperties.GetActiveUdpListeners();
            return ipEndPointsUdp.Any(endPoint => endPoint.Port == port);
        }

        /// <summary>
        /// 设置显示器状态
        /// </summary>
        /// <param name="state"></param>
        private static void SetMonitorInState(MonitorState state)
        {
            //hWnd: If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows.
            SendMessage((IntPtr)0xFFFF, WM_SYSCOMMAND, SC_MONITORPOWER, (int)state);
        }
        /// <summary>
        /// 设置显示器状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="hWnd"></param>
        private static void SetMonitorInState(MonitorState state, IntPtr hWnd)
        {
            SendMessage(hWnd, WM_SYSCOMMAND, SC_MONITORPOWER, (int)state);
        }
    }
}