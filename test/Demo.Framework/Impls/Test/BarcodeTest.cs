using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sean.Utility.Barcode;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Demo.Framework.Impls.Test
{
    public class BarcodeTest : ISimpleDo
    {
        public void Execute()
        {
            #region Code39
            /*Code39是条形码的一种。由于编制简单、能够对任意长度的数据进行编码、支持设备广泛等特性而被广泛采用。

            Code39条形码规则：
            1、每五条线表示一个字符；
            2、粗线表示1，细线表示0；
            3、线条间的间隙宽的表示1，窄的表示0；
            4、五条线加上它们之间的四条间隙就是九位二进制编码，而且这九位中必定有三位是1，所以称为39码；
            5、条形码的首尾各一个 * 标识开始和结束。

            Code39只接受如下43个有效输入字符：
            1、26个大写字母（A - Z），
            2、十个数字（0 - 9），
            3、连接号(-),句号（.）,空格,美圆符号($),斜扛(/),加号(+)以及百分号(%)。
            4、其余的输入将被忽略。
            5、code39通常情况下不需要校验码。但是对於精确度要求高的应用，需要在code39条形码後面增加一个校验码。*/
            #endregion

            #region Code128
            /*CODE128条形码是广泛应用在企业内部管理、生产流程、物流控制系统方面的条码码制，由于其优良的特性在管理信息系统的设计中被广泛使用，CODE128码是应用最广泛的条码码制之一。
            CODE128条形码是1981年引入的一种高密度条码，CODE128码可表示从 ASCII 0 到ASCII 127 共128个字符，故称128码。其中包含了数字、字母和符号字符。

            Code128各编码方式的编码范围：
            1、Code128A：标准数字和字母，控制符，特殊字符；
            2、Code128B：标准数字和字母，小写字母，特殊字符；
            3、Code128C/EAN128：[00]-[99]的数字对集合，共100个，即只能表示偶数位长度的数字。*/

            Code128 code128 = new Code128 { Height = 38, ValueFont = new Font("Arial", 9) };
            code128.GetImage("test12345!@", Code128Type.Code128B).Save(@"D:\\1.jpg");
            #endregion
        }
    }
}
