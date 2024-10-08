﻿using System;
using System.Security.Cryptography;
using Demo.NetCore.Contracts;
using Sean.Utility.Contracts;
using Sean.Utility.Security;
using Sean.Utility.Security.Provider;

namespace Demo.NetCore.Impls.Test.Security
{
    public class RSATest : ISimpleDo
    {
        public void Execute()
        {
            //System.Security.Cryptography.RSAOpenSsl

            var rsa = new RSACryptoProvider
            {
                DefaultKeyType = RsaKeyType.OpenSsl,
                DefaultKeyEncodeMode = EncodeMode.None,
                DefaultEncryptionEncodeMode = EncodeMode.Hex,
                DefaultHashAlgorithmName = HashAlgorithmName.SHA1,// SHA1WithRSA，对RSA密钥的长度不限制，推荐使用2048位以上
                //DefaultHashAlgorithmName = HashAlgorithmName.SHA256,// SHA256WithRSA，强制要求RSA密钥的长度至少为2048
            };

            #region 密钥
            var publicKey = string.Empty;
            var privateKey = string.Empty;
            switch (rsa.DefaultKeyType)
            {
                case RsaKeyType.FromXmlString:
                    rsa.GenerateXmlKey(out publicKey, out privateKey);
                    break;
                case RsaKeyType.OpenSsl:
                    rsa.DefaultKeyEncodeMode = EncodeMode.Base64;
                    // RsaKeySizeType.RSA: 1024
                    //publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCdFay5po4aHYGwIq0sM/lnu2MkaxCmK/3yAK2AI5KmeAvIlcUwDcsIU8psy0YSOHC4fPLMOrMFDh+U/fCVT/ZDGhgS31YkZLdFLUxgwgpVLfQDZ1wxGv8wfAwWCevkrleAUOMIEd1K4a8LMJUSxi+D2lGUUNUnCng910DhdVFVAwIDAQAB";
                    //privateKey = "MIICWwIBAAKBgQCdFay5po4aHYGwIq0sM/lnu2MkaxCmK/3yAK2AI5KmeAvIlcUwDcsIU8psy0YSOHC4fPLMOrMFDh+U/fCVT/ZDGhgS31YkZLdFLUxgwgpVLfQDZ1wxGv8wfAwWCevkrleAUOMIEd1K4a8LMJUSxi+D2lGUUNUnCng910DhdVFVAwIDAQABAoGARuXAdm4o1rqYjPGprTisCVh277nu4sEaNX5+0EW966pkzxxhykV/bHu88gtwzcpxfaLPRsP7hn2QXYObcRiM8Zi0vdN4WQbeAS/l4Wp732aHoGWGPniL8dC11C699CAlPFKBPIkJyO+OJW/lNn33w06Svg9wACLI15cDsBQY0LkCQQDX0yLDhk+m4OW3oEGQBC4MWjhm5GV3+j8vZRfBsyMoy0n9gwZoHrMKHWXua2ZtsP5uBIgbliI0BWojiIpAVY3NAkEAulNaKZ+VvdHfNr4n3UYapcbFxeU8PMbVKmrD+oAiFREjFp0TB+Wtqp+34hkAMRP+493PlGNCm9jfUWhuIHAeDwJAL9wVOHCfVy1GI6s4/ru+jmSvXznEuo9W1abAVubnpBM4jCwzDoHISDTutqlAZJC8Sx9cI2numcSdndWYet29/QJASLBByWEviqj4eqO+a41w0eF0wFpIoLE08eJK6Evaf/t6g9TtWrRYhureUr9MGtlhI8YhuBLtJl156YljBRaYMwJALN6ww7gJF7YdxO5P3mOExbhXhRhVo2ysCZSQcslTjunIARFc23cWbdAzcESir1S/2Vd8p1GVVAeDL0jEWe35JA==";
                    // RsaKeySizeType.RSA2: 2048
                    publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhth6uEDbXf8bG70ag26mYFzkP486gnLjdU6unM0yp2PwqyHihgHaodSs03HCB054FKX/b37ozL1BelAhqSxn2NzJ98265hzOWGWqkCJYZQD0K0Wzt8xaCvS9r9VMvC48EE45advCPJor346FJwaq/JKEl0iTKS1vuTAcIZP84e+Ex+CxuFdUBoJeZFf8MtsQSpZyWnFA/VUDyws6bt0Lk4cu8j/gkFoo5owbOqYp1IZBgciQG8SQSt6sIbaRByyBPKutihipodzcEGG0jlPrNd8AewCouxmN0x18T6tqB3xWyzE6XV3s2DbsoZT94lPvUxmLADRNqJ9WwBf0d42HPQIDAQAB";
                    privateKey = "MIIEogIBAAKCAQEAhth6uEDbXf8bG70ag26mYFzkP486gnLjdU6unM0yp2PwqyHihgHaodSs03HCB054FKX/b37ozL1BelAhqSxn2NzJ98265hzOWGWqkCJYZQD0K0Wzt8xaCvS9r9VMvC48EE45advCPJor346FJwaq/JKEl0iTKS1vuTAcIZP84e+Ex+CxuFdUBoJeZFf8MtsQSpZyWnFA/VUDyws6bt0Lk4cu8j/gkFoo5owbOqYp1IZBgciQG8SQSt6sIbaRByyBPKutihipodzcEGG0jlPrNd8AewCouxmN0x18T6tqB3xWyzE6XV3s2DbsoZT94lPvUxmLADRNqJ9WwBf0d42HPQIDAQABAoIBAEFvreXZ+1oa4NHOK56aRHzAtzasz6zcCcs0WoHXBfy6/+Bb+iwjnDAqH5AgRRL8nkMO5uz5PgmU0cCEQEFzPhW71vq8yDTAx9WXyHRXZmPlWZfiJDtOHGfyqlY4gRtUqGsVOsFHFnQIMMA0ziLJFpKom1U6r8csuGm6kfwtb2tH9ukvqSOGHnyJIdZCHxEiTQO/zYHCpkSqdDE5FE4QKuFsh6AqYxn2s/XPcCSDqRCaoZobwjyyIxwWftOiL1qez8bkAOaYDHk7awM1UeqJDxnPtS7yO/nA4yXszq1HbO0cRRLiq2INM+/OdEg/mRIZCcc/RUD3Vli/vI9To9nuSVUCgYEAudSd0u1tADUtsk2tq93ALaos52OL6GzfXgfeS4oxkJ8EdLEoD6SX01I1AtqSvDRSVuUUmA79Gp/9CqiOLbHreDs4we2+Bmmp6JJLWfdp0jXSVExuTH1GH+3j3HkM+xtlwFcr1s8UrlbdROOm6+7vbzMF++eWWPCsfvFP6/3SNS8CgYEAucNiys3Z8kzUgROdZlJfjgiPE2p8eeMM3AsMPVvxwNxZw1INp8juJgz95gJ2DgTjmrjuDT+whyxo8chbdFnyEurEXAB7nbYcbTBPUc8BAnZlRiomLteU4IGpFvqn6fzQShoX7tLKiUT7w7dtSGBRevq1wOOIkuRAsBELMGmuB1MCgYAl3M0/UcLYvP0PjEYrc2YZp8e4aY7zD2O/e+juCT0qE0xMHmBkN4v7sY89gp1NHXf9XnWAueUWDKz5A/jcFJk+iQp6cN873UGSMmSJ8yArhW3SYTRD/25PSSAZJszfSOjFdL1o6g5zqx0wURYMkkVUqde5SKlYe8kZoD9BfRjcXwKBgFmj8GNfDIIBEzaqSXUb4OKAPNKouyJyLz1r0180pCdl7ab0TISf0FPqRwccPu2q7E44WR9pkXzZMFckUQWHdeotAc5HqV9Ehtkly9D1bE5M5bgDFOPbwgrIZtkg4bBm//gSOWRUscml0TvzorefSxJmMxi+c6Q2owSTZOvaw1DjAoGAIyaQKn0XnYurRk+wW0Zwd5BLTZP+Y4KAz95jBLlH/wcnYKV4jRgUy421XXz7U3kJc0yf3cf+tDSi8Y56GasnWx8Ci2NotQ2W0iDv56F890tpI3Y5galSi/f9Hsas8HgPfGvk1fqex2HTSYmcvgHZSth6AaRQxz7HoOF9gVnso+k=";
                    break;
            }
            rsa.PublickKey = publicKey;
            rsa.PrivatekKey = privateKey;
            Console.WriteLine("=================================================");
            Console.WriteLine($"公钥：{publicKey}");
            Console.WriteLine("=================================================");
            Console.WriteLine($"密钥：{privateKey}");
            Console.WriteLine("=================================================");
            #endregion

            var data = "测试test123...";
            Console.WriteLine($"原始数据：{data}");
            var encrypt = rsa.Encrypt(data);
            Console.WriteLine($"加密后：{encrypt}");
            var decrypt = rsa.Decrypt(encrypt);
            Console.WriteLine($"解密后：{decrypt}");
        }
    }
}
