using System.Collections.Generic;
using System.Drawing;

namespace Sean.Utility.Barcode
{
    /// <summary>
    /// Code39条形码
    /// </summary>
    public class Code39
    {
        private readonly Dictionary<char, string> _validChar;

        public Code39()
        {
            _validChar = new Dictionary<char, string>()
            {
                {'0',"000110100"},
                {'1',"100100001"},
                {'2',"001100001"},
                {'3',"101100000"},
                {'4',"000110001"},
                {'5',"100110000"},
                {'6',"001110000"},
                {'7',"000100101"},
                {'8',"100100100"},
                {'9',"001100100"},
                {'A',"100001001"},
                {'B',"001001001"},
                {'C',"101001000"},
                {'D',"000011001"},
                {'E',"100011000"},
                {'F',"001011000"},
                {'G',"000001101"},
                {'H',"100001100"},
                {'I',"001001100"},
                {'J',"000011100"},
                {'K',"100000011"},
                {'L',"001000011"},
                {'M',"101000010"},
                {'N',"000010011"},
                {'O',"100010010"},
                {'P',"001010010"},
                {'Q',"000000111"},
                {'R',"100000110"},
                {'S',"001000110"},
                {'T',"000010110"},
                {'U',"110000001"},
                {'V',"011000001"},
                {'W',"111000000"},
                {'X',"010010001"},
                {'Y',"110010000"},
                {'Z',"011010000"},
                {'-',"010000101"},
                {'.',"110000100"},
                {' ',"011000100"},
                {'$',"010101000"},
                {'/',"010100010"},
                {'+',"010001010"},
                {'%',"000101010"},
                {'*',"010010100"}
            };
        }

        /// <summary>
        /// 获取Code39条形码图片（基于GDI+）
        /// </summary>
        /// <param name="content">条码内容</param>
        /// <param name="height">条码高度</param>
        /// <param name="showText">是否显示条码内容</param>
        /// <returns>Code39条形码</returns>
        public Bitmap GetImage(string content, int height, bool showText, Font textFont)
        {
            int topMargin = 0;
            int leftMargin = 5;
            int thickLength = 2;
            int narrowLength = 1;
            string strEncode = _validChar['*']; //添加起始码

            content = content.ToUpper();

            Bitmap image = new Bitmap(((thickLength * 3 + narrowLength * 7) * (content.Length + 2)) + (leftMargin * 2), height + (topMargin * 2));
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);

                for (int i = 0; i < content.Length; i++)
                {
                    //非法字符校验
                    if (content[i] == '*' || !_validChar.ContainsKey(content[i]))
                    {
                        graphics.DrawString("Invalid Bar Code", SystemFonts.DefaultFont, Brushes.Red, leftMargin, topMargin);
                        return image;
                    }
                    //编码
                    strEncode = $"{strEncode}0{_validChar[content[i]]}";
                }
                strEncode = $"{strEncode}0{_validChar['*']}"; //添加结束码

                int barWidth;
                for (int i = 0; i < strEncode.Length; i++)
                {
                    barWidth = strEncode[i] == '1' ? thickLength : narrowLength;
                    graphics.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, leftMargin, topMargin, barWidth, height);
                    leftMargin += barWidth;
                }

                if (showText)
                {
                    SizeF sizeF = graphics.MeasureString(content, textFont);
                    float x = (image.Width - sizeF.Width) / 2;
                    float y = image.Height - sizeF.Height;
                    graphics.FillRectangle(Brushes.White, x, y, sizeF.Width, sizeF.Height);
                    graphics.DrawString(content, textFont, Brushes.Black, x, y);
                }
            }

            return image;
        }
    }
}
