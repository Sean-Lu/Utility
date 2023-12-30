using System.Security.Cryptography;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Use 3DES algorithm to encrypt or decrypt data.
    /// </summary>
    /// <remarks>
    /// <see cref="DES"/> 加密算法采用的密钥长度(<see cref="SymmetricAlgorithm.KeySize"/>)是64bit
    /// </remarks>
    public class TripleDESCryptoProvider : SymmetricAlgorithmBase
    {
        protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            return TripleDES.Create();//new TripleDESCryptoServiceProvider();
        }
    }
}
