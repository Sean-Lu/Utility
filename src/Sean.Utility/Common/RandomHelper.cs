using System;
using System.Text;
using System.Security.Cryptography;

namespace Sean.Utility.Common
{
    /// <summary>
    /// 随机数（支持伪随机、真随机）
    /// </summary>
    public class RandomHelper
    {
        #region 随机数生成器
        /// <summary>
        /// 伪随机数生成器（依靠随机数种子）。伪随机数生成效率 > 真随机数生成效率
        /// </summary>
        private static Random _fakeRandom = new Random(unchecked((int)DateTime.Now.Ticks));
        /// <summary>
        /// .NET下的真随机数生成器
        /// </summary>
        private static RNGCryptoServiceProvider _relRandom = new RNGCryptoServiceProvider();
        #endregion

        private RandomHelper() { }

        #region 伪随机
        #region 随机数
        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            return _fakeRandom.NextDouble();
        }
        /// <summary>
        /// 返回一个介于指定范围内的随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <returns></returns>
        public static double NextDouble(double max, double min = 0)
        {
            return NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// 返回一个指定范围内的随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <returns></returns>
        public static int NextInt(int max, int min = 0)
        {
            return _fakeRandom.Next(min, max);
        }

        /// <summary>
        /// 生成指定范围的数组
        /// </summary>
        /// <param name="end">最大值</param>
        /// <param name="start">起始值</param>
        /// <param name="random">是否对数组顺序做随机排序操作</param>
        /// <returns></returns>
        public static int[] CreateNumArray(int end, int start = 0, bool random = true)
        {
            int nCount = end - start + 1;
            int[] array = new int[nCount];
            for (int i = 0; i < nCount; i++)
            {
                array[i] = start;
                start++;
            }
            //对数组进行随机排序
            if (random) RandomArray(array);
            return array;
        }
        #endregion

        #region 随机字符串
        /// <summary>
        /// 生成随机字符串（只包含字母）
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string NextStringAbc(int length)
        {
            return NextString($"{Constants.StringAzLower}{Constants.StringAzLower.ToUpper()}", length);
        }
        /// <summary>
        /// 生成随机字符串（只包含字母）
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="lowerCase">字符串是否小写，true表示小写，false表示大写</param>
        /// <returns>随机字符串</returns>
        public static string NextStringAbc(int length, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder(length);
            int startChar = lowerCase ? 97 : 65;//65 = A / 97 = a
            for (int i = 0; i < length; i++)
                builder.Append((char)(26 * _fakeRandom.NextDouble() + startChar));
            return builder.ToString();
        }
        /// <summary>
        /// 生成随机字符串（只包含大小写字母和数字）
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static string NextStringAbcAndNum(int length)
        {
            return NextString($"{Constants.String09}{Constants.StringAzLower}{Constants.StringAzLower.ToUpper()}", length);
        }
        /// <summary>  
        /// 获取指定长度的纯数字随机数字串  
        /// </summary>  
        /// <param name="length">数字串长度</param>  
        /// <returns>纯数字随机数字串</returns>  
        public static string NextStringNum(int length)
        {
            StringBuilder builder = new StringBuilder(string.Empty);
            for (int i = 0; i < length; i++)
            {
                builder.Append(_fakeRandom.Next(10));
            }
            return builder.ToString();
        }
        /// <summary>
        /// 依据指定字符串来生成随机字符串
        /// </summary>
        /// <param name="randomString">指定字符串</param>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string NextString(string randomString, int length)
        {
            string nextString = string.Empty;
            if (!string.IsNullOrWhiteSpace(randomString))
            {
                StringBuilder builder = new StringBuilder(length);
                int maxCount = randomString.Length;
                for (int i = 0; i < length; i++)
                {
                    int number = _fakeRandom.Next(0, maxCount);
                    builder.Append(randomString[number]);
                }
                nextString = builder.ToString();
            }
            return nextString;
        }
        /// <summary>
        /// 依据指定字符串来生成随机字符串
        /// </summary>
        /// <param name="randomString">指定字符串</param>
        /// <param name="length">字符串长度</param>
        /// <param name="lowerCase">字符串是否小写</param>
        /// <returns>随机字符串</returns>
        public static string NextString(string randomString, int length, bool lowerCase)
        {
            string nextString = NextString(randomString, length);
            return lowerCase ? nextString.ToLower() : nextString.ToUpper();
        }
        #endregion

        #region 随机时间
        /// <summary>
        /// 生成随机时间（日期不变，时间随机）
        /// </summary>
        /// <returns>随机时间</returns>
        public static DateTime NextTime()
        {
            int hour = _fakeRandom.Next(0, 23);
            int minute = _fakeRandom.Next(0, 59);
            int second = _fakeRandom.Next(0, 59);
            string dateTimeString = string.Format("{0} {1}:{2}:{3}", DateTime.Now.ToString("yyyy-MM-dd"), hour, minute, second);
            DateTime nextTime = Convert.ToDateTime(dateTimeString);
            return nextTime;
        }
        #endregion

        #region  随机MAC地址
        /// <summary>
        /// 生成随机MAC地址
        /// </summary>
        /// <returns>新的MAC地址</returns>
        public static string NextMacAddress()
        {
            int minValue = 0, maxValue = 16;
            string randomMacAddress = string.Format("{0}{1}:{2}{3}:{4}{5}:{6}{7}:{8}{9}:{10}{11}",
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),//16进制
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x"),
                                                   _fakeRandom.Next(minValue, maxValue).ToString("x")
                                                    ).ToUpper();
            return randomMacAddress;
        }
        #endregion

        /// <summary>
        /// 对一个数组进行随机排序（使用数组长度作为交换次数）
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static void RandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换。
            int count = arr.Length;
            //开始交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = _fakeRandom.Next(count - 1);
                int randomNum2 = _fakeRandom.Next(count - 1);
                //交换两个随机数位置的值
                var temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }
        #endregion

        #region 真随机
        /// <summary>
        /// 获取随机字符串(任意字符。取自ASCII范围：'!' - '~')
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="relRandom">是否使用真随机。true表示真随机；false表示伪随机。</param>
        public static string GetRandomAnyString(int length, bool relRandom)
        {
            if (length < 1) throw new ArgumentException("参数值必须大于 0 ", "length");

            byte[] data = new byte[1];//一个字节=8位（2^8=256）。2^7=128（ASCII码个数）
            int nCharStart = 0x21;//'!'
            int nCharEnd = 0x7e;//'~'
            int nCharTmp = 0;
            string strRet = "";
            do
            {
                if (relRandom)
                    _relRandom.GetBytes(data);//真随机
                else
                    _fakeRandom.NextBytes(data);//伪随机
                nCharTmp = (Convert.ToInt32(data[0]) % (nCharEnd - nCharStart + 1)) + nCharStart;//0x21~0x7e
                strRet += Convert.ToString(Convert.ToChar(nCharTmp));
            }
            while (strRet.Length < length);
            return strRet;
        }
        #endregion
    }
}
