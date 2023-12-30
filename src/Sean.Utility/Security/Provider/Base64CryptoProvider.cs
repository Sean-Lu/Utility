using System;
using System.Text;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Base64编码\解码
    /// </summary>
    public class Base64CryptoProvider : EncodingBase
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
            return EncryptBytes(encoding.GetBytes(data));
        }
        public static string EncryptBytes(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static string Decrypt(string data, Encoding encoding)
        {
            return encoding.GetString(DecryptBytes(data));
        }
        public static byte[] DecryptBytes(string data)
        {
            return Convert.FromBase64String(data);
        }
    }
}
