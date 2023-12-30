using System;
using System.Text;
using Sean.Utility.Security;
using Sean.Utility.Security.Provider;

namespace Sean.Utility.Extensions
{
    public static class EncodeModeExtensions
    {
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="encodeMode"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncodeToString(this EncodeMode encodeMode, string data, Encoding encoding)
        {
            switch (encodeMode)
            {
                case EncodeMode.None:
                    return data;
                case EncodeMode.Base64:
                    return Base64CryptoProvider.Encrypt(data, encoding);
                case EncodeMode.Hex:
                    return HexCryptoProvider.Encrypt(data, encoding);
                case EncodeMode.Base64ToHex:
                    return HexCryptoProvider.Encrypt(Base64CryptoProvider.Encrypt(data, encoding), encoding);
                default:
                    throw new NotSupportedException($"{string.Format(Constants.NotSupportedEncodeMode2, encodeMode.ToString())}");
            }
        }
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="encodeMode"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncodeToString(this EncodeMode encodeMode, byte[] data, Encoding encoding)
        {
            switch (encodeMode)
            {
                case EncodeMode.None:
                    return encoding.GetString(data);
                case EncodeMode.Base64:
                    return Base64CryptoProvider.EncryptBytes(data);
                case EncodeMode.Hex:
                    return HexCryptoProvider.EncryptBytes(data);
                case EncodeMode.Base64ToHex:
                    return HexCryptoProvider.Encrypt(Base64CryptoProvider.EncryptBytes(data), encoding);
                default:
                    throw new NotSupportedException($"{string.Format(Constants.NotSupportedEncodeMode2, encodeMode.ToString())}");
            }
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="encodeMode"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DecodeToString(this EncodeMode encodeMode, string data, Encoding encoding)
        {
            switch (encodeMode)
            {
                case EncodeMode.None:
                    return data;
                case EncodeMode.Base64:
                    return Base64CryptoProvider.Decrypt(data, encoding);
                case EncodeMode.Hex:
                    return HexCryptoProvider.Decrypt(data, encoding);
                case EncodeMode.Base64ToHex:
                    return Base64CryptoProvider.Decrypt(HexCryptoProvider.Decrypt(data, encoding), encoding);
                default:
                    throw new NotSupportedException($"{string.Format(Constants.NotSupportedEncodeMode2, encodeMode.ToString())}");
            }
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="encodeMode"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] DecodeToBytes(this EncodeMode encodeMode, string data, Encoding encoding)
        {
            switch (encodeMode)
            {
                case EncodeMode.None:
                    return encoding.GetBytes(data);
                case EncodeMode.Base64:
                    return Base64CryptoProvider.DecryptBytes(data);
                case EncodeMode.Hex:
                    return HexCryptoProvider.DecryptBytes(data);
                case EncodeMode.Base64ToHex:
                    return Base64CryptoProvider.DecryptBytes(HexCryptoProvider.Decrypt(data, encoding));
                default:
                    throw new NotSupportedException($"{string.Format(Constants.NotSupportedEncodeMode2, encodeMode.ToString())}");
            }
        }
    }
}
