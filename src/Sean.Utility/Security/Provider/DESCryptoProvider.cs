using System;
using System.Security.Cryptography;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Use DES algorithm to encrypt or decrypt data.
    /// </summary>
    /// <remarks>
    /// <see cref="DES"/> 加密算法采用的密钥长度(<see cref="SymmetricAlgorithm.KeySize"/>)是64bit
    /// </remarks>
    public class DESCryptoProvider : SymmetricAlgorithmBase
    {
        #region 3DES
        /// <summary>
        /// 3DES加密(对一块数据用三个不同的密钥进行三次DES加密，强度更高)
        /// </summary>
        /// <param name="data">待加密字符串</param>
        /// <param name="key1">密钥1</param>
        /// <param name="key2">密钥2</param>
        /// <param name="key3">密钥3</param>
        /// <returns></returns>
        public string EncryptTriple(string data, string key1, string key2, string key3)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(key1))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key1));
            if (string.IsNullOrWhiteSpace(key2))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key2));
            if (string.IsNullOrWhiteSpace(key3))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key3));

            var encrypt = Encrypt(data, key3);
            encrypt = Encrypt(encrypt, key2);
            encrypt = Encrypt(encrypt, key1);
            return encrypt;
        }
        /// <summary>
        /// 3DES解密(对一块数据用三个不同的密钥进行三次DES解密，强度更高)
        /// </summary>
        /// <param name="data">待解密字符串</param>
        /// <param name="key1">密钥1</param>
        /// <param name="key2">密钥2</param>
        /// <param name="key3">密钥3</param>
        /// <returns></returns>
        public string DecryptTriple(string data, string key1, string key2, string key3)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(key1))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key1));
            if (string.IsNullOrWhiteSpace(key2))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key2));
            if (string.IsNullOrWhiteSpace(key3))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key3));

            var decrypt = Decrypt(data, key1);
            decrypt = Decrypt(decrypt, key2);
            decrypt = Decrypt(decrypt, key3);
            return decrypt;
        }
        #endregion

        protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            return DES.Create();//new DESCryptoServiceProvider();
        }
    }
}
