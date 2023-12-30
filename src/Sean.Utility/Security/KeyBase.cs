namespace Sean.Utility.Security
{
    public abstract class KeyBase : EncodingBase
    {
        public abstract string GetStringKey();
        public abstract byte[] GetBytesKey();
    }
}