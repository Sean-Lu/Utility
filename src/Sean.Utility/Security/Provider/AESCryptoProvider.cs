using System.Security.Cryptography;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Use AES algorithm to encrypt or decrypt data.
    /// </summary>
    /// <remarks>
    /// AES加密算法(<see cref="Rijndael"/>)采用的密钥长度(<see cref="SymmetricAlgorithm.KeySize"/>)支持：128bit、192bit、256bit
    /// </remarks>
    public class AESCryptoProvider : SymmetricAlgorithmBase
    {
        protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            return Rijndael.Create();//new RijndaelManaged();
        }
    }
}
