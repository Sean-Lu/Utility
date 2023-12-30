namespace Sean.Utility.Security
{
    /// <summary>
    /// Base class for encryption
    /// </summary>
    public abstract class CryptoBase : EncodingBase
    {
        /// <summary>
        /// 默认的密钥编码模式，默认值：<see cref="EncodeMode.None"/>
        /// </summary>
        public EncodeMode DefaultKeyEncodeMode { get; set; } = EncodeMode.None;
        /// <summary>
        /// 默认的加密编码模式，默认值：<see cref="EncodeMode.Base64"/>
        /// </summary>
        public EncodeMode DefaultEncryptionEncodeMode { get; set; } = EncodeMode.Base64;
    }
}
