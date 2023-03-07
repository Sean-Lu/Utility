using System;

namespace Demo.Framework.Helper
{
    public static class ConvertHelper
    {
        #region 进制转换（十进制转X进制，X进制转十进制）
        /// <summary>
        /// 十进制转二进制
        /// </summary>
        /// <param name="dec">十进制</param>
        /// <returns>二进制</returns>
        public static string DecToBin(int dec)
        {
            return Convert.ToString(dec, 2);
        }
        /// <summary>
        /// 十进制转八进制
        /// </summary>
        /// <param name="dec">十进制</param>
        /// <returns>八进制</returns>
        public static string DecToOct(int dec)
        {
            return Convert.ToString(dec, 8);
        }
        /// <summary>
        /// 十进制转十六进制
        /// </summary>
        /// <param name="dec">十进制</param>
        /// <returns>十六进制</returns>
        public static string DecToHex(int dec)
        {
            return Convert.ToString(dec, 16);
        }

        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="bin">二进制</param>
        /// <returns>十进制</returns>
        public static int BinToDec(string bin)
        {
            return Convert.ToInt32(bin, 2);
        }
        /// <summary>
        /// 八进制转十进制
        /// </summary>
        /// <param name="oct">八进制</param>
        /// <returns>十进制</returns>
        public static int OctToDec(string oct)
        {
            return Convert.ToInt32(oct, 8);
        }
        /// <summary>
        /// 十六进制转十进制
        /// </summary>
        /// <param name="hex">十六进制</param>
        /// <returns>十进制</returns>
        public static int HexToDec(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }
        #endregion
    }
}
