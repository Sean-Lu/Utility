using System.Collections.Generic;
using System.Windows.Forms;

namespace Sean.Utility.WindowsForms
{
    /// <summary>
    /// 数据绑定
    /// </summary>
    public static class DataBindUtil
    {
        /// <summary>
        /// 绑定Dictionary到ComboBox
        /// </summary>
        /// <typeparam name="T1">Key类型。建议使用string类型</typeparam>
        /// <typeparam name="T2">Value类型。建议使用string类型</typeparam>
        /// <param name="cb">ComboBox</param>
        /// <param name="dic">Dictionary(数据源)</param>
        /// <param name="reverse">Key、Value绑定关系是否颠倒。默认：DisplayMember->Key,ValueMember->Value</param>
        public static void BindDictionary<T1, T2>(ComboBox cb, Dictionary<T1, T2> dic, bool reverse = false)
        {
            BindingSource bs = new BindingSource { DataSource = dic };
            cb.DataSource = bs;
            if (!reverse)
            {
                cb.DisplayMember = "Key";
                cb.ValueMember = "Value";
            }
            else
            {
                cb.DisplayMember = "Value";
                cb.ValueMember = "Key";
            }
        }
    }
}
