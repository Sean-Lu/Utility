using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Sean.Utility.Format
{
    public class ConvertHelper
    {
        #region 16进制字符串和字符串互相转换
        /// <summary>
        /// 字符串转16进制字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ToHexString(string str, Encoding encode)
        {
            #region 方式1
            /*byte[] b = encode.GetBytes(str);
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++) //逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;*/
            #endregion

            #region 方式2
            return ToHexString(encode.GetBytes(str), string.Empty);
            #endregion
        }
        /// <summary>
        /// 16进制字符串转字符串
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string FromHexString(string hexString, Encoding encode)
        {
            return encode.GetString(ToBytes(hexString, string.Empty));
        }
        #endregion

        #region 16进制字符串和字节互相转换
        /// <summary>
        /// 16进制字符串转字节
        /// </summary>
        /// <param name="hex">16进制字符串。长度为2</param>
        /// <returns>字节</returns>
        public static byte ToByte(string hex)
        {
            return byte.Parse(hex, NumberStyles.HexNumber);
        }
        /// <summary>
        /// 字节转16进制字符串
        /// </summary>
        /// <param name="b">字节</param>
        /// <returns>16进制字符串。长度为2</returns>
        public static string ToHexString(byte b)
        {
            return ToHexString(new[] { b });
        }
        #endregion

        #region 16进制字符串和字节数组互相转换
        /// <summary> 
        /// 字节数组转16进制字符串。
        /// </summary> 
        /// <param name="bytes">字节数组</param>
        /// <param name="separator">16进制字符串中的分隔符。默认格式：XX-XX-XX</param>
        /// <returns>16进制字符串</returns> 
        public static string ToHexString(byte[] bytes, string separator = "-")
        {
            var result = BitConverter.ToString(bytes);
            return separator != "-" ? result.Replace("-", separator) : result;
        }
        /// <summary> 
        /// 16进制字符串转字节数组。
        /// </summary> 
        /// <param name="hexString">16进制字符串</param> 
        /// <param name="separator">16进制字符串中的分隔符。示例：XX-XX-XX</param>
        /// <returns>字节数组</returns> 
        public static byte[] ToBytes(string hexString, string separator = "-")
        {
            if (!string.IsNullOrEmpty(separator))
            {
                hexString = hexString.Replace(separator, string.Empty);
            }
            if (hexString.Length % 2 != 0)
                return null;

            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
        #endregion

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

        #region 人民币转换
        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns>中文</returns>
        public static string ToChinese(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
        #endregion
    }
}
