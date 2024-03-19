using System.Security.Cryptography;

namespace Sean.Utility.Security
{
    /// <summary>
    /// 编码模式
    /// </summary>
    public enum EncodeMode
    {
        /// <summary>
        /// 不做编码处理
        /// </summary>
        None,
        /// <summary>
        /// Base64编码
        /// </summary>
        Base64,
        /// <summary>
        /// 16进制编码
        /// </summary>
        Hex,
        /// <summary>
        /// 先Base64编码，再进行16进制编码
        /// </summary>
        Base64ToHex,
    }

    /// <summary>
    /// RSA密钥类型
    /// </summary>
    public enum RsaKeyType
    {
        /// <summary>
        /// 通过 <see cref="RSA.ToXmlString"/> 生成的密钥
        /// </summary>
        FromXmlString,
        /// <summary>
        /// 通过 OpenSSL 生成的密钥
        /// </summary>
        OpenSsl
    }
}
