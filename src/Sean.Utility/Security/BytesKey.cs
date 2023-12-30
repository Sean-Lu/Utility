using Sean.Utility.Extensions;

namespace Sean.Utility.Security
{
    public class BytesKey : KeyBase
    {
        private readonly byte[] _key;
        private readonly EncodeMode _keyEncodeMode;

        public BytesKey(byte[] key, EncodeMode keyEncodeMode = EncodeMode.None)
        {
            _key = key;
            _keyEncodeMode = keyEncodeMode;
        }

        public override string GetStringKey()
        {
            return _keyEncodeMode.EncodeToString(_key, DefaultEncoding);
        }

        public override byte[] GetBytesKey()
        {
            return _key;
        }
    }
}