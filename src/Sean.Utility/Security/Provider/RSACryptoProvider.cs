using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Sean.Utility.Extensions;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Use RSA algorithm to encrypt or decrypt or sign or verify data.
    /// </summary>
    /// <remarks>
    /// <para>√ 目前支持以下2种密钥类型（<see cref="DefaultKeyType"/>）：</para>
    /// <para>1. 通过 <see cref="RSA.ToXmlString"/> 生成的密钥</para>
    /// <para>2. 通过 OpenSSL 生成的密钥（请注意修改 <see cref="CryptoBase.DefaultKeyEncodeMode"/> 的值，通常为：<see cref="EncodeMode.Base64"/>）</para>
    /// <para>√ 支持跨平台：Windows, Linux, Mac, ...</para>
    /// <para>注：如果使用.NET Framework的版本小于4.6，默认使用 <see cref="RSACryptoServiceProvider"/>，这个类并不支持跨平台，在Windows上运行正常，但是在Linux下运行就会报错（编译不会报错）。</para>
    /// <para>如果在非Windows平台下使用<see cref="RSACryptoProvider"/>，建议升级.NET Framework的版本（>=4.6），或使用.NET Core。</para>
    /// </remarks>
    public class RSACryptoProvider : CryptoAndSignatureBase
    {
        /// <summary>
        /// 默认的密钥类型，默认值：<see cref="RsaKeyType.FromXmlString"/>
        /// </summary>
        /// <remarks>
        /// The default key type, default value: <see cref="RsaKeyType.FromXmlString"/>
        /// </remarks>
        public RsaKeyType DefaultKeyType { get; set; } = RsaKeyType.FromXmlString;
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublickKey { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivatekKey { get; set; }

#if !NET40 && !NET45
        /// <summary>
        /// 默认值：<see cref="RSAEncryptionPadding.Pkcs1"/>
        /// </summary>
        /// <remarks>
        /// Default value: <see cref="RSAEncryptionPadding.Pkcs1"/>
        /// </remarks>
        public RSAEncryptionPadding DefaultEncryptionPadding { get; set; } = RSAEncryptionPadding.Pkcs1;
        /// <summary>
        /// 默认值：<see cref="RSAEncryptionPadding.Pkcs1"/>
        /// </summary>
        /// <remarks>
        /// Default value: <see cref="RSAEncryptionPadding.Pkcs1"/>
        /// </remarks>
        public RSASignaturePadding DefaultSignaturePadding { get; set; } = RSASignaturePadding.Pkcs1;

        /// <summary>
        /// 默认值：<see cref="HashAlgorithmName.SHA1"/>
        /// </summary>
        /// <remarks>
        /// Default value: <see cref="HashAlgorithmName.SHA1"/>
        /// </remarks>
        public HashAlgorithmName DefaultHashAlgorithmName { get; set; } = HashAlgorithmName.SHA1;
#else
        /// <summary>
        /// 用于创建哈希值的哈希算法，默认值：<see cref="SHA1CryptoServiceProvider"/>
        /// </summary>
        /// <remarks>
        /// The hash algorithm used to create the hash value, default value: <see cref="SHA1CryptoServiceProvider"/>
        /// </remarks>
        public object DefaultHashAlgorithmInstance { get; set; } = new SHA1CryptoServiceProvider();
#endif

        #region 公钥加密
        /// <summary>
        /// Encrypt with public key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            return Encrypt(data, PublickKey);
        }
        /// <summary>
        /// Encrypt with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="publicKey">Public key</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data, string publicKey)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(publicKey));

            using (var rsa = CreateRSAProviderFromPublicKey(publicKey))
            {
                //return rsa.Encrypt(data, rsaEncryptionPadding);

                // 使用非对称密钥加密数据时，一次加密的数据长度是：密钥长度/8-11，超过这个大小会报错：The message exceeds the maximum allowable length for the chosen options (117).
                // 如果key的长度为1024位，1024/8 - 11 = 117，一次加密内容不能超过117bytes。
                // 如果key的长度为2048位，2048/8 - 11 = 245，一次加密内容不能超过245bytes。
                var bufferSize = rsa.KeySize / 8 - 11;//单块最大长度
                //Console.WriteLine($"#########密钥长度：{rsa.KeySize}");
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(data), outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        // 分段加密
                        var readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
#if !NET40 && !NET45
                        var encryptedBytes = rsa.Encrypt(temp, DefaultEncryptionPadding);
#else
                        var encryptedBytes = rsa.Encrypt(temp, false);
#endif
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return outputStream.ToArray();
                }
            }
        }
        /// <summary>
        /// Encrypt with public key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string data)
        {
            return Encrypt(data, DefaultEncryptionEncodeMode);
        }
        /// <summary>
        /// Encrypt with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encryptionEncodeMode"><see cref="CryptoBase.DefaultEncryptionEncodeMode"/></param>
        /// <returns></returns>
        public string Encrypt(string data, EncodeMode encryptionEncodeMode)
        {
            return Encrypt(data, encryptionEncodeMode, PublickKey);
        }
        /// <summary>
        /// Encrypt with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encryptionEncodeMode"><see cref="CryptoBase.DefaultEncryptionEncodeMode"/></param>
        /// <param name="publicKey">Public key</param>
        /// <returns></returns>
        public string Encrypt(string data, EncodeMode encryptionEncodeMode, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(publicKey));

            return encryptionEncodeMode.EncodeToString(Encrypt(DefaultEncoding.GetBytes(data), publicKey), DefaultEncoding);
        }
        #endregion

        #region 私钥解密
        /// <summary>
        /// Decrypt with private key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            return Decrypt(data, PrivatekKey);
        }
        /// <summary>
        /// Decrypt with private key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey">Private key</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data, string privateKey)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(privateKey));

            using (var rsa = CreateRSAProviderFromPrivateKey(privateKey))
            {
                //return rsa.Decrypt(data, rsaEncryptionPadding);

                var bufferSize = rsa.KeySize / 8;
                //Console.WriteLine($"#########密钥长度：{rsa.KeySize}");
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(data), outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        // 分段解密
                        var readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
#if !NET40 && !NET45
                        var rawBytes = rsa.Decrypt(temp, DefaultEncryptionPadding);
#else
                        var rawBytes = rsa.Decrypt(temp, false);
#endif
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }
                    return outputStream.ToArray();
                }
            }
        }
        /// <summary>
        /// Decrypt with private key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(string data)
        {
            return Decrypt(data, DefaultEncryptionEncodeMode);
        }
        /// <summary>
        /// Decrypt with private key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encryptionEncodeMode"><see cref="CryptoBase.DefaultEncryptionEncodeMode"/></param>
        /// <returns></returns>
        public string Decrypt(string data, EncodeMode encryptionEncodeMode)
        {
            return Decrypt(data, encryptionEncodeMode, PrivatekKey);
        }
        /// <summary>
        /// Decrypt with private key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encryptionEncodeMode"><see cref="CryptoBase.DefaultEncryptionEncodeMode"/></param>
        /// <param name="privateKey">Private key</param>
        /// <returns></returns>
        public string Decrypt(string data, EncodeMode encryptionEncodeMode, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(privateKey));

            return DefaultEncoding.GetString(Decrypt(encryptionEncodeMode.DecodeToBytes(data, DefaultEncoding), privateKey));
        }
        #endregion

        #region 私钥签名
        /// <summary>
        /// Sign with private key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] SignData(byte[] data)
        {
            return SignData(data, PrivatekKey);
        }
        /// <summary>
        /// Sign with private key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey">Private key</param>
        /// <returns></returns>
        public byte[] SignData(byte[] data, string privateKey)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(privateKey));

            using (var rsa = CreateRSAProviderFromPrivateKey(privateKey))
            {
#if !NET40 && !NET45
                return rsa.SignData(data, DefaultHashAlgorithmName, DefaultSignaturePadding);
#else
                return rsa.SignData(data, DefaultHashAlgorithmInstance);
#endif
            }
        }
        /// <summary>
        /// Sign with private key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SignData(string data)
        {
            return SignData(data, DefaultSignatureEncodeMode);
        }
        /// <summary>
        /// Sign with private key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signatureEncodeMode"><see cref="CryptoAndSignatureBase.DefaultSignatureEncodeMode"/></param>
        /// <returns></returns>
        public string SignData(string data, EncodeMode signatureEncodeMode)
        {
            return SignData(data, signatureEncodeMode, PrivatekKey);
        }
        /// <summary>
        /// Sign with private key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signatureEncodeMode"><see cref="CryptoAndSignatureBase.DefaultSignatureEncodeMode"/></param>
        /// <param name="privateKey">Private key</param>
        /// <returns></returns>
        public string SignData(string data, EncodeMode signatureEncodeMode, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(privateKey));

            return signatureEncodeMode.EncodeToString(SignData(DefaultEncoding.GetBytes(data), privateKey), DefaultEncoding);
        }
        #endregion

        #region 公钥验签
        /// <summary>
        /// Verification with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool VerifyData(byte[] data, byte[] signature)
        {
            return VerifyData(data, signature, PublickKey);
        }
        /// <summary>
        /// Verification with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey">Public key</param>
        /// <returns></returns>
        public bool VerifyData(byte[] data, byte[] signature, string publicKey)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (signature == null) throw new ArgumentNullException(nameof(signature));
            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(publicKey));

            using (var rsa = CreateRSAProviderFromPublicKey(publicKey))
            {
#if !NET40 && !NET45
                return rsa.VerifyData(data, signature, DefaultHashAlgorithmName, DefaultSignaturePadding);
#else
                return rsa.VerifyData(data, DefaultHashAlgorithmInstance, signature);
#endif
            }
        }
        /// <summary>
        /// Verification with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool VerifyData(string data, string signature)
        {
            return VerifyData(data, signature, DefaultSignatureEncodeMode);
        }
        /// <summary>
        /// Verification with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="signatureEncodeMode"><see cref="CryptoAndSignatureBase.DefaultSignatureEncodeMode"/></param>
        /// <returns></returns>
        public bool VerifyData(string data, string signature, EncodeMode signatureEncodeMode)
        {
            return VerifyData(data, signature, signatureEncodeMode, PublickKey);
        }
        /// <summary>
        /// Verification with public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="signatureEncodeMode"><see cref="CryptoAndSignatureBase.DefaultSignatureEncodeMode"/></param>
        /// <param name="publicKey">Public key</param>
        /// <returns></returns>
        public bool VerifyData(string data, string signature, EncodeMode signatureEncodeMode, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(signature))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(signature));
            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(publicKey));

            return VerifyData(DefaultEncoding.GetBytes(data), signatureEncodeMode.DecodeToBytes(signature, DefaultEncoding), publicKey);
        }
        #endregion

        /// <summary>
        /// Generate RSA public and private key by <see cref="RSA.ToXmlString"/>
        /// </summary>
        /// <param name="xmlPublicKey">Public key</param>
        /// <param name="xmlPrivateKey">Private key</param>
        public void GenerateXmlKey(out string xmlPublicKey, out string xmlPrivateKey)
        {
            GenerateXmlKey(out xmlPublicKey, out xmlPrivateKey, DefaultKeyEncodeMode, DefaultEncoding);
        }

#if !NET40 && !NET45
        public static RSA CreateRSAProvider()
        {
            return RSA.Create();
        }
#else
        public static RSACryptoServiceProvider CreateRSAProvider()
        {
            return new RSACryptoServiceProvider();
        }
#endif

        /// <summary>
        /// Use the private key to create an RSA instance
        /// </summary>
        /// <param name="privateKey">Private key</param>
        /// <returns></returns>
        public
#if !NET40 && !NET45
            RSA
#else
            RSACryptoServiceProvider
#endif
            CreateRSAProviderFromPrivateKey(string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(privateKey));

            var rsa = CreateRSAProvider();
            switch (DefaultKeyType)
            {
                case RsaKeyType.FromXmlString:
                    rsa.FromXmlString(DefaultKeyEncodeMode.DecodeToString(privateKey, DefaultEncoding));
                    break;
                case RsaKeyType.OpenSsl:
                    var rsaParameters = new RSAParameters();
                    var privateKeyBits = DefaultKeyEncodeMode.DecodeToBytes(privateKey, DefaultEncoding);
                    using (var mem = new MemoryStream(privateKeyBits))
                    {
                        using (var binaryReader = new BinaryReader(mem))
                        {
                            var twobytes = binaryReader.ReadUInt16();
                            if (twobytes == 0x8130)
                                binaryReader.ReadByte();
                            else if (twobytes == 0x8230)
                                binaryReader.ReadInt16();
                            else
                                throw new Exception("Unexpected value read binr.ReadUInt16()");

                            twobytes = binaryReader.ReadUInt16();
                            if (twobytes != 0x0102)
                                throw new Exception("Unexpected version");

                            var bt = binaryReader.ReadByte();
                            if (bt != 0x00)
                                throw new Exception("Unexpected value read binr.ReadByte()");

                            rsaParameters.Modulus = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                            rsaParameters.Exponent = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                            rsaParameters.D = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                            rsaParameters.P = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                            rsaParameters.Q = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                            rsaParameters.DP = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                            rsaParameters.DQ = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                            rsaParameters.InverseQ = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                        }
                    }
                    rsa.ImportParameters(rsaParameters);
                    break;
            }
            return rsa;
        }

        /// <summary>
        /// Use the public key to create an RSA instance
        /// </summary>
        /// <param name="publicKey">Public key</param>
        /// <returns></returns>
        public
#if !NET40 && !NET45
            RSA
#else
            RSACryptoServiceProvider
#endif
            CreateRSAProviderFromPublicKey(string publicKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(publicKey));

            // ------- create RSACryptoServiceProvider instance and initialize with public key -----
            // System.Security.Cryptography.RSA.Create() 工厂方法，使用它之后，在 Windows 上创建的是 System.Security.Cryptography.RSACng 的实例，在 Mac 与 Linux 上创建的是 System.Security.Cryptography.RSAOpenSsl 的实例，它们都继承自 System.Security.Cryptography.RSA 抽象类。
            var rsa = CreateRSAProvider();
            switch (DefaultKeyType)
            {
                case RsaKeyType.FromXmlString:
                    rsa.FromXmlString(DefaultKeyEncodeMode.DecodeToString(publicKey, DefaultEncoding));
                    break;
                case RsaKeyType.OpenSsl:
                    var rsaParameters = new RSAParameters();
                    // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
                    var x509Key = DefaultKeyEncodeMode.DecodeToBytes(publicKey, DefaultEncoding);
                    using (var mem = new MemoryStream(x509Key))
                    {
                        using (var binaryReader = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                        {
                            var twobytes = binaryReader.ReadUInt16();
                            if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                                binaryReader.ReadByte();    //advance 1 byte
                            else if (twobytes == 0x8230)
                                binaryReader.ReadInt16();   //advance 2 bytes
                            else
                                return null;

                            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
                            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
                            byte[] seq = new byte[15];

                            seq = binaryReader.ReadBytes(15);       //read the Sequence OID
                            if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
                                return null;

                            twobytes = binaryReader.ReadUInt16();
                            if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                                binaryReader.ReadByte();    //advance 1 byte
                            else if (twobytes == 0x8203)
                                binaryReader.ReadInt16();   //advance 2 bytes
                            else
                                return null;

                            var bt = binaryReader.ReadByte();
                            if (bt != 0x00)     //expect null byte next
                                return null;

                            twobytes = binaryReader.ReadUInt16();
                            if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                                binaryReader.ReadByte();    //advance 1 byte
                            else if (twobytes == 0x8230)
                                binaryReader.ReadInt16();   //advance 2 bytes
                            else
                                return null;

                            twobytes = binaryReader.ReadUInt16();
                            byte lowbyte = 0x00;
                            byte highbyte = 0x00;

                            if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                                lowbyte = binaryReader.ReadByte();  // read next bytes which is bytes in modulus
                            else if (twobytes == 0x8202)
                            {
                                highbyte = binaryReader.ReadByte(); //advance 2 bytes
                                lowbyte = binaryReader.ReadByte();
                            }
                            else
                                return null;
                            byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                            int modsize = BitConverter.ToInt32(modint, 0);

                            int firstbyte = binaryReader.PeekChar();
                            if (firstbyte == 0x00)
                            {   //if first byte (highest order) of modulus is zero, don't include it
                                binaryReader.ReadByte();    //skip this null byte
                                modsize -= 1;   //reduce modulus buffer size by 1
                            }

                            byte[] modulus = binaryReader.ReadBytes(modsize);   //read the modulus bytes

                            if (binaryReader.ReadByte() != 0x02)            //expect an Integer for the exponent data
                                return null;
                            int expbytes = (int)binaryReader.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                            byte[] exponent = binaryReader.ReadBytes(expbytes);

                            rsaParameters.Modulus = modulus;
                            rsaParameters.Exponent = exponent;
                        }
                    }
                    rsa.ImportParameters(rsaParameters);
                    break;
            }
            return rsa;
        }

        /// <summary>
        /// Generate RSA public and private key by <see cref="RSA.ToXmlString"/>
        /// </summary>
        /// <param name="xmlPublicKey">Public key</param>
        /// <param name="xmlPrivateKey">Private key</param>
        public static void GenerateXmlKey(out string xmlPublicKey, out string xmlPrivateKey, EncodeMode keyEncodeMode, Encoding encoding)
        {
            using (var rsa = CreateRSAProvider())
            {
                xmlPublicKey = keyEncodeMode.EncodeToString(rsa.ToXmlString(false), encoding);
                xmlPrivateKey = keyEncodeMode.EncodeToString(rsa.ToXmlString(true), encoding);
            }
        }

        #region Private Methods
        private int GetIntegerSize(BinaryReader binr)
        {
            int count;
            var bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        #endregion
    }
}
