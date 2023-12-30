using System.Security.Cryptography;
using Sean.Utility.Extensions;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Hash algorithm.
    /// </summary>
    public class HashCryptoProvider : EncodingBase
    {
        private readonly EncodeMode _encodeMode = EncodeMode.Hex;

        #region MD5(Message-Digest Algorithm 5)：消息摘要算法第五版
        /// <summary>
        /// MD5（32位）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string MD5(string data)
        {
            using (var hashAlgorithm = new MD5CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// MD5（32位）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string MD5(byte[] data)
        {
            using (var hashAlgorithm = new MD5CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        #endregion

        #region SHA(Secure Hash Algorithm)：安全哈希算法
        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA1(string data)
        {
            using (var hashAlgorithm = new SHA1CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA1(byte[] data)
        {
            using (var hashAlgorithm = new SHA1CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA256(string data)
        {
            using (var hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA256(byte[] data)
        {
            using (var hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// SHA384
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA384(string data)
        {
            using (var hashAlgorithm = new SHA384CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// SHA384
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA384(byte[] data)
        {
            using (var hashAlgorithm = new SHA384CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// SHA512
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA512(string data)
        {
            using (var hashAlgorithm = new SHA512CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        /// <summary>
        /// SHA512
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SHA512(byte[] data)
        {
            using (var hashAlgorithm = new SHA512CryptoServiceProvider())
            {
                return ComputeHash(data, hashAlgorithm);
            }
        }
        #endregion

        /// <summary>
        /// 使用指定的哈希算法来计算哈希值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashAlgorithm"></param>
        /// <returns></returns>
        public string ComputeHash(string data, HashAlgorithm hashAlgorithm)
        {
            return ComputeHash(DefaultEncoding.GetBytes(data), hashAlgorithm);
        }

        /// <summary>
        /// 使用指定的哈希算法来计算哈希值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashAlgorithm"></param>
        /// <returns></returns>
        public string ComputeHash(byte[] data, HashAlgorithm hashAlgorithm)
        {
            var hash = _encodeMode.EncodeToString(hashAlgorithm.ComputeHash(data), DefaultEncoding);
            //hashAlgorithm.Clear();//等同于Dispose()
            return hash;
        }
    }
}
