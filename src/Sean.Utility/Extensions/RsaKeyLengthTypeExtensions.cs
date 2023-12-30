using System;
using System.Security.Cryptography;
using Sean.Utility.Security;

namespace Sean.Utility.Extensions
{
    public static class RsaKeyLengthTypeExtensions
    {
#if !NET40 && !NET45
        public static HashAlgorithmName GetHashAlgorithmName(this RsaKeySizeType rsaKeySizeType)
        {
            switch (rsaKeySizeType)
            {
                case RsaKeySizeType.RSA:
                    return HashAlgorithmName.SHA1;
                case RsaKeySizeType.RSA2:
                    return HashAlgorithmName.SHA256;
                default:
                    throw new NotSupportedException($"[{rsaKeySizeType}]Unsupported RSA Type.");
            }
        }
#endif
    }
}
