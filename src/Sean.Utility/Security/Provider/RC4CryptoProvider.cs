using Sean.Utility.Extensions;

namespace Sean.Utility.Security.Provider
{
    /// <summary>
    /// Use RC4 algorithm to encrypt or decrypt data.
    /// </summary>
    public class RC4CryptoProvider : CryptoBase
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">待加密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public string Encrypt(string data, string key)
        {
            int[] s = new int[256];
            for (int i = 0; i < 256; i++)
            {
                s[i] = i;
            }

            //密钥转数组
            char[] charKeys = DefaultKeyEncodeMode.DecodeToString(key, DefaultEncoding).ToCharArray();
            int[] intKeys = new int[charKeys.Length];
            for (int i = 0; i < charKeys.Length; i++)
            {
                intKeys[i] = charKeys[i];
            }

            //明文转数组
            char[] datas = data.ToCharArray();
            int[] mingwen = new int[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                mingwen[i] = datas[i];
            }

            //通过循环得到256位的数组(密钥)
            int j = 0;
            int k = 0;
            int length = intKeys.Length;
            int a;
            for (int i = 0; i < 256; i++)
            {
                a = s[i];
                j = j + a + intKeys[k];
                if (j >= 256)
                {
                    j = j % 256;
                }
                s[i] = s[j];
                s[j] = a;
                if (++k >= length)
                {
                    k = 0;
                }
            }

            //根据上面的256的密钥数组 和 明文得到密文数组
            int x = 0, y = 0, a2, b, c;
            int length2 = mingwen.Length;
            int[] miwen = new int[length2];
            for (int i = 0; i < length2; i++)
            {
                x = x + 1;
                x = x % 256;
                a2 = s[x];
                y = y + a2;
                y = y % 256;
                s[x] = b = s[y];
                s[y] = a2;
                c = a2 + b;
                c = c % 256;
                miwen[i] = mingwen[i] ^ s[c];
            }

            //密文数组转密文字符
            char[] mi = new char[miwen.Length];
            for (int i = 0; i < miwen.Length; i++)
            {
                mi[i] = (char)miwen[i];
            }

            return DefaultEncryptionEncodeMode.EncodeToString(new string(mi), DefaultEncoding);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">待解密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public string Decrypt(string data, string key)
        {
            int[] s = new int[256];
            for (int i = 0; i < 256; i++)
            {
                s[i] = i;
            }

            //密钥转数组
            char[] charKeys = DefaultKeyEncodeMode.DecodeToString(key, DefaultEncoding).ToCharArray();
            int[] intKeys = new int[charKeys.Length];
            for (int i = 0; i < charKeys.Length; i++)
            {
                intKeys[i] = charKeys[i];
            }

            //密文转数组
            char[] datas = DefaultEncryptionEncodeMode.DecodeToString(data, DefaultEncoding).ToCharArray();
            int[] miwen = new int[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                miwen[i] = datas[i];
            }

            //通过循环得到256位的数组(密钥)
            int j = 0;
            int k = 0;
            int length = intKeys.Length;
            int a;
            for (int i = 0; i < 256; i++)
            {
                a = s[i];
                j = j + a + intKeys[k];
                if (j >= 256)
                {
                    j = j % 256;
                }
                s[i] = s[j];
                s[j] = a;
                if (++k >= length)
                {
                    k = 0;
                }
            }

            //根据上面的256的密钥数组 和 密文得到明文数组
            int x = 0, y = 0, a2, b, c;
            int length2 = miwen.Length;
            int[] mingwen = new int[length2];
            for (int i = 0; i < length2; i++)
            {
                x = x + 1;
                x = x % 256;
                a2 = s[x];
                y = y + a2;
                y = y % 256;
                s[x] = b = s[y];
                s[y] = a2;
                c = a2 + b;
                c = c % 256;
                mingwen[i] = miwen[i] ^ s[c];
            }

            //明文数组转明文字符
            char[] ming = new char[mingwen.Length];
            for (int i = 0; i < mingwen.Length; i++)
            {
                ming[i] = (char)mingwen[i];
            }

            return new string(ming);
        }
    }
}
