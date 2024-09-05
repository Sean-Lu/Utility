using System;
using System.Text;
using Demo.NetCore.Contracts;
using Sean.Utility.Contracts;
using Sean.Utility.Security;
using Sean.Utility.Security.Provider;

namespace Demo.NetCore.Impls.Test.Security
{
    public class AESTest : ISimpleDo
    {
        public void Execute()
        {
            var aes = new AESCryptoProvider
            {
                DefaultEncryptionEncodeMode = EncodeMode.Base64,
                DefaultEncoding = Encoding.UTF8,
                Key = "12345678"
            };
            aes.BeforeSetSymmetricAlgorithm += (sender, args) =>
            {
                var algorithm = args.Data;
                // toDo something...
                algorithm.KeySize = 128;
            };
            aes.AfterSetSymmetricAlgorithm += (sender, args) =>
            {
                var algorithm = args.Data;
                // toDo something...
                Console.WriteLine($"KeySize: {algorithm.KeySize}, BlockSize: {algorithm.BlockSize}");
            };
            var data = "测试test123...";
            Console.WriteLine($"原始数据：{data}");
            var encrypt = aes.Encrypt(data);
            Console.WriteLine($"加密后：{encrypt}");
            var decrypt = aes.Decrypt(encrypt);
            Console.WriteLine($"解密后：{decrypt}");
        }
    }
}
