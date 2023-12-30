using System;
using System.Text;
using Sean.Utility.Contracts;
using Sean.Utility.Security;
using Sean.Utility.Security.Provider;

namespace Demo.NetCore.Impls.Test.Security
{
    public class DESTest : ISimpleDo
    {
        public void Execute()
        {
            var des = new DESCryptoProvider
            {
                DefaultEncryptionEncodeMode = EncodeMode.Base64,
                DefaultEncoding = Encoding.UTF8,
                Key = "12345678"
            };
            var data = "测试test123...";
            Console.WriteLine($"原始数据：{data}");
            var encrypt = des.Encrypt(data);
            Console.WriteLine($"加密后：{encrypt}");
            var decrypt = des.Decrypt(encrypt);
            Console.WriteLine($"解密后：{decrypt}");
        }
    }
}
