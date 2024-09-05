using System;
using System.Text;
using Demo.NetCore.Contracts;
using Sean.Utility.Contracts;
using Sean.Utility.Security;
using Sean.Utility.Security.Provider;

namespace Demo.NetCore.Impls.Test.Security
{
    public class RC2Test : ISimpleDo
    {
        public void Execute()
        {
            var rc2 = new RC2CryptoProvider
            {
                DefaultEncryptionEncodeMode = EncodeMode.Base64,
                DefaultEncoding = Encoding.UTF8,
                Key = "12345"
            };
            rc2.BeforeSetSymmetricAlgorithm += (sender, args) =>
            {
                var algorithm = args.Data;
                // toDo something...
            };
            rc2.AfterSetSymmetricAlgorithm += (sender, args) =>
            {
                var algorithm = args.Data;
                // toDo something...
                Console.WriteLine($"KeySize: {algorithm.KeySize}, BlockSize: {algorithm.BlockSize}");
            };
            var data = "测试test123...";
            Console.WriteLine($"原始数据：{data}");
            var encrypt = rc2.Encrypt(data);
            Console.WriteLine($"加密后：{encrypt}");
            var decrypt = rc2.Decrypt(encrypt);
            Console.WriteLine($"解密后：{decrypt}");
        }
    }
}
