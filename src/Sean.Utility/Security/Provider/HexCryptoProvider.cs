using System;
using System.Text;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// 16进制编码\解码
    /// </summary>
    public class HexCryptoProvider : EncodingBase
    {
        public string Encrypt(string data)
        {
            return Encrypt(data, DefaultEncoding);
        }

        public string Decrypt(string data)
        {
            return Decrypt(data, DefaultEncoding);
        }

        public static string Encrypt(string data, Encoding encoding)
        {
            var bytes = encoding.GetBytes(data);
            return EncryptBytes(bytes);
        }
        public static string EncryptBytes(byte[] data)
        {
            var result = BitConverter.ToString(data);
            return result.Replace("-", string.Empty);
        }

        public static string Decrypt(string data, Encoding encoding)
        {
            return encoding.GetString(DecryptBytes(data));
        }
        public static byte[] DecryptBytes(string data)
        {
            //var separator = string.Empty;
            //if (!string.IsNullOrEmpty(separator))
            //{
            //    data = data.Replace(separator, string.Empty);
            //}
            if (data.Length % 2 != 0)
                return null;

            var bytes = new byte[data.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
    }
}
