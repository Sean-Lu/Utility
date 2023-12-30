namespace Sean.Utility.Security
{
    /// <summary>
    /// Base class for encryption and signature
    /// </summary>
    public abstract class CryptoAndSignatureBase : CryptoBase
    {
        /// <summary>
        /// 默认的签名编码模式，默认值：<see cref="EncodeMode.Base64"/>
        /// </summary>
        public EncodeMode DefaultSignatureEncodeMode { get; set; } = EncodeMode.Base64;
    }
}