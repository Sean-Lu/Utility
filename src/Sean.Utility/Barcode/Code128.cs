#if !NETSTANDARD
using Sean.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace Sean.Utility.Barcode
{
    /// <summary>
    /// Code128条形码
    /// </summary>
    public class Code128
    {
        private readonly DataTable _code128 = new DataTable();

        /// <summary>
        /// 条码高度
        /// </summary>
        public uint Height { get; set; } = 40;
        /// <summary>
        /// 条码内容的字体，值为null时表示不显示条码内容。
        /// </summary>
        public Font ValueFont { get; set; } = null;
        /// <summary>
        /// 放大倍数
        /// </summary>
        public byte Magnify { get; set; } = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Code128()
        {
            #region Code128编码表
            _code128.Columns.Add("ID");
            _code128.Columns.Add("Code128A");
            _code128.Columns.Add("Code128B");
            _code128.Columns.Add("Code128C");
            _code128.Columns.Add("BandCode");
            _code128.CaseSensitive = true;
            _code128.Rows.Add("0", " ", " ", "00", "212222");
            _code128.Rows.Add("1", "!", "!", "01", "222122");
            _code128.Rows.Add("2", "\"", "\"", "02", "222221");
            _code128.Rows.Add("3", "#", "#", "03", "121223");
            _code128.Rows.Add("4", "$", "$", "04", "121322");
            _code128.Rows.Add("5", "%", "%", "05", "131222");
            _code128.Rows.Add("6", "&", "&", "06", "122213");
            _code128.Rows.Add("7", "'", "'", "07", "122312");
            _code128.Rows.Add("8", "(", "(", "08", "132212");
            _code128.Rows.Add("9", ")", ")", "09", "221213");
            _code128.Rows.Add("10", "*", "*", "10", "221312");
            _code128.Rows.Add("11", "+", "+", "11", "231212");
            _code128.Rows.Add("12", ",", ",", "12", "112232");
            _code128.Rows.Add("13", "-", "-", "13", "122132");
            _code128.Rows.Add("14", ".", ".", "14", "122231");
            _code128.Rows.Add("15", "/", "/", "15", "113222");
            _code128.Rows.Add("16", "0", "0", "16", "123122");
            _code128.Rows.Add("17", "1", "1", "17", "123221");
            _code128.Rows.Add("18", "2", "2", "18", "223211");
            _code128.Rows.Add("19", "3", "3", "19", "221132");
            _code128.Rows.Add("20", "4", "4", "20", "221231");
            _code128.Rows.Add("21", "5", "5", "21", "213212");
            _code128.Rows.Add("22", "6", "6", "22", "223112");
            _code128.Rows.Add("23", "7", "7", "23", "312131");
            _code128.Rows.Add("24", "8", "8", "24", "311222");
            _code128.Rows.Add("25", "9", "9", "25", "321122");
            _code128.Rows.Add("26", ":", ":", "26", "321221");
            _code128.Rows.Add("27", ";", ";", "27", "312212");
            _code128.Rows.Add("28", "<", "<", "28", "322112");
            _code128.Rows.Add("29", "=", "=", "29", "322211");
            _code128.Rows.Add("30", ">", ">", "30", "212123");
            _code128.Rows.Add("31", "?", "?", "31", "212321");
            _code128.Rows.Add("32", "@", "@", "32", "232121");
            _code128.Rows.Add("33", "A", "A", "33", "111323");
            _code128.Rows.Add("34", "B", "B", "34", "131123");
            _code128.Rows.Add("35", "C", "C", "35", "131321");
            _code128.Rows.Add("36", "D", "D", "36", "112313");
            _code128.Rows.Add("37", "E", "E", "37", "132113");
            _code128.Rows.Add("38", "F", "F", "38", "132311");
            _code128.Rows.Add("39", "G", "G", "39", "211313");
            _code128.Rows.Add("40", "H", "H", "40", "231113");
            _code128.Rows.Add("41", "I", "I", "41", "231311");
            _code128.Rows.Add("42", "J", "J", "42", "112133");
            _code128.Rows.Add("43", "K", "K", "43", "112331");
            _code128.Rows.Add("44", "L", "L", "44", "132131");
            _code128.Rows.Add("45", "M", "M", "45", "113123");
            _code128.Rows.Add("46", "N", "N", "46", "113321");
            _code128.Rows.Add("47", "O", "O", "47", "133121");
            _code128.Rows.Add("48", "P", "P", "48", "313121");
            _code128.Rows.Add("49", "Q", "Q", "49", "211331");
            _code128.Rows.Add("50", "R", "R", "50", "231131");
            _code128.Rows.Add("51", "S", "S", "51", "213113");
            _code128.Rows.Add("52", "T", "T", "52", "213311");
            _code128.Rows.Add("53", "U", "U", "53", "213131");
            _code128.Rows.Add("54", "V", "V", "54", "311123");
            _code128.Rows.Add("55", "W", "W", "55", "311321");
            _code128.Rows.Add("56", "X", "X", "56", "331121");
            _code128.Rows.Add("57", "Y", "Y", "57", "312113");
            _code128.Rows.Add("58", "Z", "Z", "58", "312311");
            _code128.Rows.Add("59", "[", "[", "59", "332111");
            _code128.Rows.Add("60", "\\", "\\", "60", "314111");
            _code128.Rows.Add("61", "]", "]", "61", "221411");
            _code128.Rows.Add("62", "^", "^", "62", "431111");
            _code128.Rows.Add("63", "_", "_", "63", "111224");
            _code128.Rows.Add("64", "NUL", "`", "64", "111422");
            _code128.Rows.Add("65", "SOH", "a", "65", "121124");
            _code128.Rows.Add("66", "STX", "b", "66", "121421");
            _code128.Rows.Add("67", "ETX", "c", "67", "141122");
            _code128.Rows.Add("68", "EOT", "d", "68", "141221");
            _code128.Rows.Add("69", "ENQ", "e", "69", "112214");
            _code128.Rows.Add("70", "ACK", "f", "70", "112412");
            _code128.Rows.Add("71", "BEL", "g", "71", "122114");
            _code128.Rows.Add("72", "BS", "h", "72", "122411");
            _code128.Rows.Add("73", "HT", "i", "73", "142112");
            _code128.Rows.Add("74", "LF", "j", "74", "142211");
            _code128.Rows.Add("75", "VT", "k", "75", "241211");
            _code128.Rows.Add("76", "FF", "I", "76", "221114");
            _code128.Rows.Add("77", "CR", "m", "77", "413111");
            _code128.Rows.Add("78", "SO", "n", "78", "241112");
            _code128.Rows.Add("79", "SI", "o", "79", "134111");
            _code128.Rows.Add("80", "DLE", "p", "80", "111242");
            _code128.Rows.Add("81", "DC1", "q", "81", "121142");
            _code128.Rows.Add("82", "DC2", "r", "82", "121241");
            _code128.Rows.Add("83", "DC3", "s", "83", "114212");
            _code128.Rows.Add("84", "DC4", "t", "84", "124112");
            _code128.Rows.Add("85", "NAK", "u", "85", "124211");
            _code128.Rows.Add("86", "SYN", "v", "86", "411212");
            _code128.Rows.Add("87", "ETB", "w", "87", "421112");
            _code128.Rows.Add("88", "CAN", "x", "88", "421211");
            _code128.Rows.Add("89", "EM", "y", "89", "212141");
            _code128.Rows.Add("90", "SUB", "z", "90", "214121");
            _code128.Rows.Add("91", "ESC", "{", "91", "412121");
            _code128.Rows.Add("92", "FS", "|", "92", "111143");
            _code128.Rows.Add("93", "GS", "}", "93", "111341");
            _code128.Rows.Add("94", "RS", "~", "94", "131141");
            _code128.Rows.Add("95", "US", "DEL", "95", "114113");
            _code128.Rows.Add("96", "FNC3", "FNC3", "96", "114311");
            _code128.Rows.Add("97", "FNC2", "FNC2", "97", "411113");
            _code128.Rows.Add("98", "SHIFT", "SHIFT", "98", "411311");
            _code128.Rows.Add("99", "CODEC", "CODEC", "99", "113141");
            _code128.Rows.Add("100", "CODEB", "FNC4", "CODEB", "114131");
            _code128.Rows.Add("101", "FNC4", "CODEA", "CODEA", "311141");
            _code128.Rows.Add("102", "FNC1", "FNC1", "FNC1", "411131");
            _code128.Rows.Add("103", "StartA", "StartA", "StartA", "211412");
            _code128.Rows.Add("104", "StartB", "StartB", "StartB", "211214");
            _code128.Rows.Add("105", "StartC", "StartC", "StartC", "211232");
            _code128.Rows.Add("106", "Stop", "Stop", "Stop", "2331112");
            #endregion
        }

        /// <summary>
        /// 获取Code128条形码图片
        /// </summary>
        /// <param name="content">条码内容</param>
        /// <param name="type">条码类型</param>   
        /// <returns>Code128条形码</returns>
        public Bitmap GetImage(string content, Code128Type type)
        {
            string viewText = content;
            string text = "";
            IList<int> textNumb = new List<int>();
            int examine = 0;//首位
            switch (type)
            {
                case Code128Type.Code128C:
                    examine = 105;
                    if ((content.Length & 1) != 0) throw new Exception("128C长度必须是偶数");
                    while (content.Length != 0)
                    {
                        int temp = 0;
                        if (!long.TryParse(content.Substring(0, 2), out _))
                        {
                            throw new Exception("128C必须是数字！");

                        }
                        text += GetValue(type, content.Substring(0, 2), ref temp);
                        textNumb.Add(temp);
                        content = content.Remove(0, 2);
                    }
                    break;
                case Code128Type.EAN128:
                    examine = 105;
                    if ((content.Length & 1) != 0) throw new Exception("EAN128长度必须是偶数");
                    textNumb.Add(102);
                    text += "411131";
                    while (content.Length != 0)
                    {
                        int temp = 0;
                        if (!long.TryParse(content.Substring(0, 2), out _))
                        {
                            throw new Exception("EAN128必须是数字！");

                        }
                        text += GetValue(Code128Type.Code128C, content.Substring(0, 2), ref temp);
                        textNumb.Add(temp);
                        content = content.Remove(0, 2);
                    }
                    break;
                default:
                    examine = type == Code128Type.Code128A ? 103 : 104;

                    while (content.Length != 0)
                    {
                        int temp = 0;
                        string valueCode = GetValue(type, content.Substring(0, 1), ref temp);
                        if (valueCode.Length == 0) throw new Exception("无效的字符集!" + content.Substring(0, 1));
                        text += valueCode;
                        textNumb.Add(temp);
                        content = content.Remove(0, 1);
                    }
                    break;
            }
            if (textNumb.Count == 0) throw new Exception("错误的编码,无数据");
            text = text.Insert(0, GetValue(examine));//获取开始位

            for (int i = 0; i != textNumb.Count; i++)
            {
                examine += textNumb[i] * (i + 1);
            }
            examine = examine % 103;//获得严效位
            text += GetValue(examine);//获取严效位
            text += "2331112";//结束位
            Bitmap image = GetImage(text);
            GetViewText(image, viewText);
            return image;
        }

        /// <summary>
        /// 获取目标对应的数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="value">数值 A b 30</param>
        /// <param name="setId">返回编号</param>
        /// <returns>编码</returns>
        private string GetValue(Code128Type code, string value, ref int setId)
        {
            if (_code128 == null)
                return "";
            DataRow[] row = _code128.Select(code.ToString() + "='" + value + "'");
            if (row.Length != 1)
                throw new Exception("错误的编码" + value.ToString());
            setId = Int32.Parse(row[0]["ID"].ToString());
            return row[0]["BandCode"].ToString();
        }
        /// <summary>
        /// 根据编号获得条纹
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        private string GetValue(int codeId)
        {
            DataRow[] row = _code128.Select("ID='" + codeId.ToString() + "'");
            if (row.Length != 1)
                throw new Exception("验效位的编码错误" + codeId.ToString());
            return row[0]["BandCode"].ToString();
        }
        /// <summary>
        /// 获得条码图形
        /// </summary>
        /// <param name="content">文字</param>
        /// <returns>图形</returns>
        private Bitmap GetImage(string content)
        {
            char[] value = content.ToCharArray();
            int width = 0;
            for (int i = 0; i != value.Length; i++)
            {
                width += int.Parse(value[i].ToString()) * (Magnify + 1);
            }
            Bitmap codeImage = new Bitmap(width, (int)Height);
            Graphics garphics = Graphics.FromImage(codeImage);
            int lenEx = 0;
            for (int i = 0; i != value.Length; i++)
            {
                int valueNumb = Int32.Parse(value[i].ToString()) * (Magnify + 1); //获取宽和放大系数
                garphics.FillRectangle((i & 1) != 0 ? Brushes.White : Brushes.Black, new Rectangle(lenEx, 0, valueNumb, (int)Height));
                lenEx += valueNumb;
            }
            garphics.Dispose();
            return codeImage;
        }
        /// <summary>
        /// 显示可见条码文字 如果小于40 不显示文字
        /// </summary>
        /// <param name="bitmap">图形</param>
        /// <param name="viewText"></param>      
        private void GetViewText(Bitmap bitmap, string viewText)
        {
            if (ValueFont == null) return;

            Graphics graphics = Graphics.FromImage(bitmap);
            SizeF drawSize = graphics.MeasureString(viewText, ValueFont);
            if (drawSize.Height > bitmap.Height - 10 || drawSize.Width > bitmap.Width)
            {
                graphics.Dispose();
                return;
            }

            int starY = bitmap.Height - (int)drawSize.Height;
            graphics.FillRectangle(Brushes.White, new Rectangle(0, starY, bitmap.Width, (int)drawSize.Height));
            graphics.DrawString(viewText, ValueFont, Brushes.Black, 0, starY);
        }
    }
}
#endif