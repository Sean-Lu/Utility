using System;
using System.Text;
using Sean.Utility.Contracts;
using Sean.Utility.Security;
using Sean.Utility.Security.Provider;

namespace Demo.NetCore.Impls.Test.Security
{
    public class RC4Test : ISimpleDo
    {
        public void Execute()
        {
            var rc4 = new RC4CryptoProvider
            {
                DefaultEncryptionEncodeMode = EncodeMode.Base64,
                DefaultEncoding = Encoding.UTF8
            };
            //rc4.BeforeSetSymmetricAlgorithm += (sender, args) =>
            //{
            //    var algorithm = args.Data;
            //    // toDo something...
            //};
            //rc4.AfterSetSymmetricAlgorithm += (sender, args) =>
            //{
            //    var algorithm = args.Data;
            //    // toDo something...
            //    Console.WriteLine($"KeySize: {algorithm.KeySize}, BlockSize: {algorithm.BlockSize}");
            //};
            var data = "测试test123...";
            var key = "12345";
            Console.WriteLine($"原始数据：{data}");
            var encrypt = rc4.Encrypt(data, key);
            Console.WriteLine($"加密后：{encrypt}");
            var decrypt = rc4.Decrypt(encrypt, key);
            Console.WriteLine($"解密后：{decrypt}");
        }
    }
}
