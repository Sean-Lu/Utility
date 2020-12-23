using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sean.Utility.Format
{
    /// <summary>
    /// 格式转换
    /// </summary>
    public class ConvertHelper
    {
        private ConvertHelper() { }

        #region DataTable转List(泛型)
        /// <summary>
        /// DataTable转List（将数据表转换为字典）
        /// </summary>
        /// <param name="table">DataTable对象</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToListDic(DataTable table)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    foreach (DataColumn dc in table.Columns)
                    {
                        dic[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    list.Add(dic);
                }
            }
            return list;
        }
        #endregion

        #region DataTable与Hashtable互相转换
        /// <summary>
        /// Hashtable转DataTable（哈希表转数据表）。
        /// 数据表包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。
        /// </summary>
        /// <param name="hashTable">哈希表(Hashtable)对象</param>
        /// <param name="tableName">数据表(DataTable)名称。如果为空，则使用默认名称。</param>
        /// <returns></returns>
        public static DataTable ToDataTable(Hashtable hashTable, string tableName)
        {
            if (hashTable == null) return null;

            string strKey = "name";
            string strValue = "value";
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(new DataColumn(strKey));
            dt.Columns.Add(new DataColumn(strValue));
            foreach (string key in hashTable.Keys)
            {
                DataRow dr = dt.NewRow();
                dr[strKey] = key;
                dr[strValue] = hashTable[key];
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// DataTable转Hashtable（数据表转哈希表）。
        /// </summary>
        /// <param name="table">数据表对象，数据表中必须包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。</param>
        /// <returns>哈希表对象。</returns>
        public static Hashtable ToHashtable(DataTable table)
        {
            string strKey = "name";
            string strValue = "value";
            if (table == null || !table.Columns.Contains(strKey) || !table.Columns.Contains(strValue)) return null;

            Hashtable hashtable = new Hashtable();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string key = table.Rows[i][strKey].ToString();
                object value = table.Rows[i][strValue];
                if (!hashtable.ContainsKey(key))
                    hashtable.Add(key, value);
                else
                    hashtable[key] = value;
            }
            return hashtable;
        }

        /// <summary>
        /// DataRow转Hashtable（数据行转哈希表）。
        /// </summary>
        /// <param name="dataRow">待转换的数据行。</param>
        /// <returns></returns>
        public static Hashtable ToHashtable(DataRow dataRow)
        {
            if (dataRow == null) return null;

            Hashtable hashTable = new Hashtable();
            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                string key = dataRow.Table.Columns[i].ColumnName;
                string val = Convert.ToString(dataRow[i]);
                if (!hashTable.ContainsKey(key))
                    hashTable.Add(key, val);
                else
                    hashTable[key] = val;
            }
            return hashTable;
        }
        #endregion

        #region DataTable与DataGridView转换
        /*/// <summary>
        /// DataGridView转DataTable
        /// </summary>
        /// <param name="dgv">DatGridView对象</param>
        /// <returns></returns>
        public static DataTable ToDataTable(DataGridView dgv)
        {
            //return dgv.DataSource as DataTable;

            DataTable dt = new DataTable();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                DataColumn dc = new DataColumn { ColumnName = dgv.Columns[i].HeaderText };
                dt.Columns.Add(dc);
            }
            for (int j = 0; j < dgv.Rows.Count; j++)
            {
                DataRow dr = dt.NewRow();
                for (int x = 0; x < dgv.Columns.Count; x++)
                {
                    dr[x] = dgv.Rows[j].Cells[x].Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }*/
        #endregion

        #region List(泛型)与数组互相转换
        /// <summary>
        /// List转数组
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] ToArray<T>(IList<T> list)
        {
            return list?.ToArray();
        }
        /// <summary>
        /// 数组转List
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(T[] array)
        {
            return array != null ? new List<T>(array) : null;
        }
        #endregion

        #region 字符串和字符数组互相转换
        /// <summary>
        /// string转char[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static char[] ToCharArray(string str)
        {
            return str.ToCharArray();
        }
        /// <summary>
        /// char[]转string
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToString(char[] c)
        {
            return new string(c);
        }
        #endregion

        #region 字符串和字节数组互相转换
        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static byte[] ToBytes(string str, Encoding encode)
        {
            return encode.GetBytes(str);
        }
        /// <summary>
        /// byte[]转string
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ToString(byte[] bytes, Encoding encode)
        {
            return encode.GetString(bytes);
        }
        #endregion

        #region 数字和字节数组互相转换
        /// <summary>
        /// int转byte[]
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(int num)
        {
            return BitConverter.GetBytes(num);
        }
        /// <summary>
        /// byte[]转int
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>数字</returns>
        public static int ToNum(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }
        #endregion

        #region 16进制字符串和字符串互相转换
        /// <summary>
        /// 字符串转16进制字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ToHexString(string str, Encoding encode)
        {
            #region 方式1
            /*byte[] b = encode.GetBytes(str);
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++) //逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;*/
            #endregion

            #region 方式2
            return ToHexString(encode.GetBytes(str), string.Empty);
            #endregion
        }
        /// <summary>
        /// 16进制字符串转字符串
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string FromHexString(string hexString, Encoding encode)
        {
            return encode.GetString(ToBytes(hexString, string.Empty));
        }
        #endregion

        #region 16进制字符串和字节互相转换
        /// <summary>
        /// 16进制字符串转字节
        /// </summary>
        /// <param name="hex">16进制字符串。长度为2</param>
        /// <returns>字节</returns>
        public static byte ToByte(string hex)
        {
            return byte.Parse(hex, NumberStyles.HexNumber);
        }
        /// <summary>
        /// 字节转16进制字符串
        /// </summary>
        /// <param name="b">字节</param>
        /// <returns>16进制字符串。长度为2</returns>
        public static string ToHexString(byte b)
        {
            return ToHexString(new[] { b });
        }
        #endregion

        #region 16进制字符串和字节数组互相转换
        /// <summary> 
        /// 字节数组转16进制字符串。
        /// </summary> 
        /// <param name="bytes">字节数组</param>
        /// <param name="separator">16进制字符串中的分隔符。默认格式：XX-XX-XX</param>
        /// <returns>16进制字符串</returns> 
        public static string ToHexString(byte[] bytes, string separator = "-")
        {
            var result = BitConverter.ToString(bytes);
            return separator != "-" ? result.Replace("-", separator) : result;
        }
        /// <summary> 
        /// 16进制字符串转字节数组。
        /// </summary> 
        /// <param name="hexString">16进制字符串</param> 
        /// <param name="separator">16进制字符串中的分隔符。示例：XX-XX-XX</param>
        /// <returns>字节数组</returns> 
        public static byte[] ToBytes(string hexString, string separator = "-")
        {

            if (string.IsNullOrWhiteSpace(separator))
            {
                if (hexString.Length % 2 != 0)
                    return null;

                var bytes = new byte[hexString.Length / 2];
                for (int i = 0; i < hexString.Length / 2; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
                return bytes;
            }
            else
            {
                string[] split = hexString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                var bytes = new byte[split.Length];
                for (int i = 0; i < split.Length; i++)
                {
                    bytes[i] = ToByte(split[i]);
                }
                return bytes;
            }
        }
        #endregion

        #region 进制转换（十进制转X进制，X进制转十进制）
        /// <summary>
        /// 十进制转二进制
        /// </summary>
        /// <param name="dec">十进制</param>
        /// <returns>二进制</returns>
        public static string DecToBin(int dec)
        {
            return Convert.ToString(dec, 2);
        }
        /// <summary>
        /// 十进制转八进制
        /// </summary>
        /// <param name="dec">十进制</param>
        /// <returns>八进制</returns>
        public static string DecToOct(int dec)
        {
            return Convert.ToString(dec, 8);
        }
        /// <summary>
        /// 十进制转十六进制
        /// </summary>
        /// <param name="dec">十进制</param>
        /// <returns>十六进制</returns>
        public static string DecToHex(int dec)
        {
            return Convert.ToString(dec, 16);
        }

        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="bin">二进制</param>
        /// <returns>十进制</returns>
        public static int BinToDec(string bin)
        {
            return Convert.ToInt32(bin, 2);
        }
        /// <summary>
        /// 八进制转十进制
        /// </summary>
        /// <param name="oct">八进制</param>
        /// <returns>十进制</returns>
        public static int OctToDec(string oct)
        {
            return Convert.ToInt32(oct, 8);
        }
        /// <summary>
        /// 十六进制转十进制
        /// </summary>
        /// <param name="hex">十六进制</param>
        /// <returns>十进制</returns>
        public static int HexToDec(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }
        #endregion

        #region 人民币转换
        /// <summary>
        /// 数字转中文（人民币转换）
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns>中文</returns>
        public static string ToChinese(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
        #endregion
    }
}
