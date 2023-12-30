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
    /// RSA密钥长度类型
    /// </summary>
    public enum RsaKeySizeType
    {
        /// <summary>
        /// SHA1WithRSA，对RSA密钥的长度不限制，推荐使用2048位以上
        /// </summary>
        RSA,
        /// <summary>
        /// SHA256WithRSA，强制要求RSA密钥的长度至少为2048
        /// </summary>
        RSA2
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
