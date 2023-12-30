using System.Text;

namespace Sean.Utility.Security
{
    public abstract class EncodingBase
    {
        /// <summary>
        /// 默认的编码格式，默认值：<see cref="Encoding.UTF8"/>
        /// </summary>
        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;
    }
}