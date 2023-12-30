using System.Security.Cryptography;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Use RC2 algorithm to encrypt or decrypt data.
    /// </summary>
    /// <remarks>
    /// <see cref="RC2"/> 密钥长度(<see cref="SymmetricAlgorithm.KeySize"/>)支持的范围是：[40bit - 128bit]，以8bit递增。
    /// </remarks>
    public class RC2CryptoProvider : SymmetricAlgorithmBase
    {
        protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            return RC2.Create();//new RC2CryptoServiceProvider();
        }
    }
}
