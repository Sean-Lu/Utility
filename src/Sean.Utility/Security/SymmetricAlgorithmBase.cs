using System;
using System.IO;
using System.Security.Cryptography;
using Sean.Utility;
using Sean.Utility.Extensions;

namespace Sean.Utility.Security
{
    /// <summary>
    /// 对称算法的基类
    /// </summary>
    public abstract class SymmetricAlgorithmBase : CryptoBase
    {
        /// <summary>
        /// 密钥，长度要求取决于 <see cref="SymmetricAlgorithm.KeySize"/> 的值。
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 获取或设置对称算法的初始化向量，长度要求取决于 <see cref="SymmetricAlgorithm.BlockSize"/> 的值。
        /// </summary>
        public virtual byte[] IV { get; set; }// = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// 在设置对称算法参数之前触发的事件
        /// </summary>
        public event EventHandler<EventArgs<SymmetricAlgorithm>> BeforeSetSymmetricAlgorithm;
        /// <summary>
        /// 在设置对称算法参数之后触发的事件
        /// </summary>
        public event EventHandler<EventArgs<SymmetricAlgorithm>> AfterSetSymmetricAlgorithm;

        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual byte[] Encrypt(byte[] data)
        {
            return Encrypt(data, Key);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public virtual byte[] Encrypt(byte[] data, string key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            using (var algorithm = CreateSymmetricAlgorithm(key))
            {
                using (var encryptor = algorithm.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual string Encrypt(string data)
        {
            return Encrypt(data, Key);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public virtual string Encrypt(string data, string key)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            var bytes = DefaultEncoding.GetBytes(data);
            var encrypt = Encrypt(bytes, key);
            return DefaultEncryptionEncodeMode.EncodeToString(encrypt, DefaultEncoding);
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destinationFilePath">目标文件路径</param>
        public virtual void EncryptFile(string sourceFilePath, string destinationFilePath)
        {
            EncryptFile(sourceFilePath, destinationFilePath, Key);
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destinationFilePath">目标文件路径</param>
        /// <param name="key">密钥</param>
        public virtual void EncryptFile(string sourceFilePath, string destinationFilePath, string key)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(sourceFilePath));
            if (string.IsNullOrWhiteSpace(destinationFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(destinationFilePath));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            using (var fin = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var fout = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fout.SetLength(0);
                    //Create variables to help with read and write.
                    byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                    long rdlen = 0; //This is the total number of bytes written.
                    long totlen = fin.Length; //This is the total length of the input file.
                    int len; //This is the number of bytes to be written at a time.

                    using (var algorithm = CreateSymmetricAlgorithm(key))
                    {
                        using (var encryptor = algorithm.CreateEncryptor())
                        using (var cs = new CryptoStream(fout, encryptor, CryptoStreamMode.Write))
                        {
                            while (rdlen < totlen)
                            {
                                len = fin.Read(bin, 0, bin.Length);
                                cs.Write(bin, 0, len);
                                rdlen += len;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 解密
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual byte[] Decrypt(byte[] data)
        {
            return Decrypt(data, Key);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public virtual byte[] Decrypt(byte[] data, string key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            using (var algorithm = CreateSymmetricAlgorithm(key))
            {
                using (var decryptor = algorithm.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual string Decrypt(string data)
        {
            return Decrypt(data, Key);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public virtual string Decrypt(string data, string key)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            var bytes = DefaultEncryptionEncodeMode.DecodeToBytes(data, DefaultEncoding);
            var decrypt = Decrypt(bytes, key);
            return DefaultEncoding.GetString(decrypt);
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destinationFilePath">目标文件路径</param>
        public virtual void DecryptFile(string sourceFilePath, string destinationFilePath)
        {
            DecryptFile(sourceFilePath, destinationFilePath, Key);
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destinationFilePath">目标文件路径</param>
        /// <param name="key">密钥</param>
        public virtual void DecryptFile(string sourceFilePath, string destinationFilePath, string key)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(sourceFilePath));
            if (string.IsNullOrWhiteSpace(destinationFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(destinationFilePath));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            using (var fin = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var fout = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fout.SetLength(0);
                    //Create variables to help with read and write.
                    byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                    long rdlen = 0; //This is the total number of bytes written.
                    long totlen = fin.Length; //This is the total length of the input file.
                    int len; //This is the number of bytes to be written at a time.

                    using (var algorithm = CreateSymmetricAlgorithm(key))
                    {
                        using (var decryptor = algorithm.CreateDecryptor())
                        using (var cs = new CryptoStream(fout, decryptor, CryptoStreamMode.Write))
                        {
                            while (rdlen < totlen)
                            {
                                len = fin.Read(bin, 0, bin.Length);
                                cs.Write(bin, 0, len);
                                rdlen += len;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        protected abstract SymmetricAlgorithm CreateSymmetricAlgorithm();

        protected virtual SymmetricAlgorithm CreateSymmetricAlgorithm(string key)
        {
            SymmetricAlgorithm algorithm = CreateSymmetricAlgorithm();
            BeforeSetSymmetricAlgorithm?.Invoke(this, new EventArgs<SymmetricAlgorithm>(algorithm));

            #region Key
            var bytesFromKey = DefaultEncoding.GetBytes(DefaultKeyEncodeMode.DecodeToString(key, DefaultEncoding));
            if (bytesFromKey.Length >= algorithm.KeySize / 8)
            {
                algorithm.Key = bytesFromKey;
            }
            else
            {
                var keyBytes = new byte[algorithm.KeySize / 8];
                Array.Copy(bytesFromKey, 0, keyBytes, 0, Math.Min(keyBytes.Length, bytesFromKey.Length));
                algorithm.Key = keyBytes;
            }
            #endregion

            #region IV
            // ECB模式不需要设置IV
            if (algorithm.Mode != CipherMode.ECB)
            {
                if (IV != null)
                {
                    algorithm.IV = IV;
                }
                else
                {
                    var ivBytes = new byte[algorithm.BlockSize / 8];
                    Array.Copy(algorithm.Key, 0, ivBytes, 0, Math.Min(ivBytes.Length, algorithm.Key.Length));
                    algorithm.IV = ivBytes;
                }
            }
            #endregion

            AfterSetSymmetricAlgorithm?.Invoke(this, new EventArgs<SymmetricAlgorithm>(algorithm));
            return algorithm;
        }
    }
}
