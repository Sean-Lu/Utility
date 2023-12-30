using System;
using System.Security.Cryptography;
using System.Text;
using Sean.Utility.Contracts;

namespace Demo.NetCore.Impls.Test.Security
{
    public class HashTest : ISimpleDo
    {
        public void Execute()
        {
            var data = Encoding.UTF8.GetBytes("测试test123...");

            var md5 = CryptoProvider.Hash.MD5(data);
            Console.WriteLine($"MD5值：{md5}，长度：{md5.Length}");

            var md5_2 = MD5(data);
            Console.WriteLine($"MD5值：{md5_2}，长度：{md5_2.Length}");

            Console.WriteLine(md5 == md5_2);
        }

        /// <summary>
        /// 计算MD5哈希值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string MD5(byte[] data)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var hashBytes = md5.ComputeHash(data);
                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.AppendFormat("{0:X2}", hashByte);// 字节转换为2位大写的16进制
                    //sb.AppendFormat("{0:x2}", hashByte);// 字节转换为2位小写的16进制
                }
                return sb.ToString();
            }
        }
    }
}
