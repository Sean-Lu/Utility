using System.Text;
using Sean.Utility.Extensions;

namespace Sean.Utility.Security
{
    public class StringKey : KeyBase
    {
        private readonly string _key;
        private readonly EncodeMode _keyEncodeMode;

        public StringKey(string key, EncodeMode keyEncodeMode = EncodeMode.None)
        {
            _key = key;
            _keyEncodeMode = keyEncodeMode;
        }

        public override string GetStringKey()
        {
            return _keyEncodeMode.DecodeToString(_key, DefaultEncoding);
        }

        public override byte[] GetBytesKey()
        {
            return _keyEncodeMode.DecodeToBytes(_key, DefaultEncoding);
        }
    }
}
