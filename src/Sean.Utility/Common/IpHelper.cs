using System.Net;

namespace Sean.Utility
{
    /// <summary>
    /// IPAddress
    /// </summary>
    public class IpHelper
    {
        /// <summary>
        /// 是否是IP地址
        /// </summary>
        /// <param name="ip">IPv4或IPv6地址</param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIpAddress(string ip, out IPAddress ipAddress)
        {
            return IPAddress.TryParse(ip, out ipAddress);
        }

        /// <summary>
        /// 是否是局域网IP地址
        /// </summary>
        /// <param name="ipv4Address">IPv4地址</param>
        /// <returns></returns>
        public static bool IsPrivateNetwork(string ipv4Address)
        {
            if (ipv4Address == "127.0.0.1" || ipv4Address?.ToLower() == "localhost" || ipv4Address == "::1")
            {
                // 本机IP地址
                return true;
            }

            // 局域网IP分类：
            // A类：10.0.0.0/8，即10.0.0.0-10.255.255.255
            // B类：172.16.0.0/12，即172.16.0.0-172.31.255.255
            // C类：192.168.0.0/16，即192.168.0.0-192.168.255.255

            if (IsIpAddress(ipv4Address, out var ipAddress))
            {
                var ipBytes = ipAddress.GetAddressBytes();
                if (ipBytes.Length == 4)
                {
                    // IPv4
                    if (ipBytes[0] == 10) return true;
                    if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31) return true;
                    if (ipBytes[0] == 192 && ipBytes[1] == 168) return true;
                }
                else if (ipBytes.Length == 16)
                {
                    // IPv6

                }
            }

            return false;
        }
    }
}