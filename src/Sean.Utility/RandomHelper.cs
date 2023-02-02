using System;
using System.Text;
using Sean.Utility.Enums;

namespace Sean.Utility
{
    public static class RandomHelper
    {
        public const string RandomStringNumer = "0123456789";
        public const string RandomStringAbcUpper = "ABCDEFGHIJKMLNOPQRSTUVWXYZ";
        public const string RandomStringAbcLower = "abcdefghijkmlnopqrstuvwxyz";

        private static Random _random = new Random();

        public static string NextString(int size, RandomStringType type)
        {
            var sb = new StringBuilder();

            if ((type & RandomStringType.Number) == RandomStringType.Number)
            {
                sb.Append(RandomStringNumer);
            }
            if ((type & RandomStringType.AbcUpper) == RandomStringType.AbcUpper)
            {
                sb.Append(RandomStringAbcUpper);
            }
            if ((type & RandomStringType.AbcLower) == RandomStringType.AbcLower)
            {
                sb.Append(RandomStringAbcLower);
            }

            return NextString(size, sb.ToString());
        }

        public static string NextString(int size, string randomStringSource)
        {
            if (string.IsNullOrWhiteSpace(randomStringSource))
            {
                return null;
            }

            var chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = randomStringSource[_random.Next(randomStringSource.Length)];
            }
            return new string(chars);
        }
    }
}
