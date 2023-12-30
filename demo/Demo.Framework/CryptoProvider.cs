using Sean.Utility.Security.Provider;

namespace Demo.Framework
{
    public static class CryptoProvider
    {
        #region 编码\解码
        public static Base64CryptoProvider Base64 { get; } = new Base64CryptoProvider();
        public static HexCryptoProvider Hex { get; } = new HexCryptoProvider();
        #endregion

        #region 单向散列算法（哈希）
        public static HashCryptoProvider Hash { get; } = new HashCryptoProvider();
        #endregion

        #region 对称加密算法：加密密钥和解密密钥相同
        public static AESCryptoProvider AES { get; } = new AESCryptoProvider();
        public static DESCryptoProvider DES { get; } = new DESCryptoProvider();
        public static RC2CryptoProvider RC2 { get; } = new RC2CryptoProvider();
        public static RC4CryptoProvider RC4 { get; } = new RC4CryptoProvider();
        #endregion

        #region 非对称加密算法：加密密钥和解密密钥不同
        public static RSACryptoProvider RSA { get; } = new RSACryptoProvider();
        #endregion
    }
}
