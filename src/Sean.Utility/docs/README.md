## 使用示例

### 局域网共享文件夹操作

> 读取共享目录文件的2种方式：

1. 建立磁盘映射方式：`NetworkConnectionHelper`

```
通过WNetAddConnection2A API将共享目录映射为本地磁盘，之后即可按本地文件形式访问文件，最后断开连接。
```

```
// 示例1：localPath（本地目录）不为null
CmdHelper.Create();
string strMsg = string.Format("[{0}]：", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")) + "{0}";
string localpath = "Z:";
string remotePath = @"\\10.xx.xx.xx\Share";
string username = @"\test";
string password = "12345!a";
// 连接
int status = NetworkConnectionHelper.Connect(remotePath, localpath, username, password);
if (status == (int)ERROR_ID.NO_ERROR)
{
    // 连接成功
    CmdHelper.WriteLine(string.Format(strMsg, "Connect Success."));
    FileStream fs = new FileStream(localpath + @"\\log2.txt", FileMode.OpenOrCreate);
    fs.Seek(0, SeekOrigin.End);
    using (StreamWriter stream = new StreamWriter(fs))
    {
        stream.WriteLine(strMsg, "Connect Success.");
        stream.Flush();
        stream.Close();
    }
    fs.Close();

    // 断开连接
    status = NetworkConnectionHelper.Disconnect(localpath);
    CmdHelper.WriteLine(status == (int)ERROR_ID.NO_ERROR
        ? string.Format(strMsg, "Disconnect Success.")
        : string.Format(strMsg, string.Format("Disconnect Failed: {0}", Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())));
}
else
{
    // 连接失败
    CmdHelper.WriteLine(string.Format(strMsg, string.Format("Connect Failed: {0}", Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())));
}
```

```
// 示例2：localPath（本地目录）为null
CmdHelper.Create();
string strMsg = string.Format("[{0}]：", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")) + "{0}";
string remotePath = @"\\10.xx.xx.xx\Share";
string username = @"\test";
string password = "12345!a";
// 连接
int status = NetworkConnectionHelper.Connect(remotePath, null, username, password);
if (status == (int)ERROR_ID.NO_ERROR)
{
    // 连接成功
    CmdHelper.WriteLine(string.Format(strMsg, "Connect Success."));
    FileStream fs = new FileStream(remotePath + @"\\log1.txt", FileMode.OpenOrCreate);
    fs.Seek(0, SeekOrigin.End);
    using (StreamWriter stream = new StreamWriter(fs))
    {
        stream.WriteLine(strMsg, "Connect Success.");
        stream.Flush();
        stream.Close();
    }
    fs.Close();

    // 断开连接
    status = NetworkConnectionHelper.Disconnect(remotePath);
    CmdHelper.WriteLine(status == (int)ERROR_ID.NO_ERROR
        ? string.Format(strMsg, "Disconnect Success.")
        : string.Format(strMsg, string.Format("Disconnect Failed: {0}", Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())));
}
else
{
    // 连接失败
    CmdHelper.WriteLine(string.Format(strMsg, string.Format("Connect Failed: {0}", Enum.IsDefined(typeof(ERROR_ID), status) ? ((ERROR_ID)status).ToString() : status.ToString())));
}
```

2. 使用net use命令：`FileShareHelper`

```
通过cmd运行“net use \\path /User:user password /PERSISTENT:YES​”命令，获取共享目录的权限，即可访问共享目录下的文件了。
```

### xml文件操作：`XmlHelper`

```
<?xml version="1.0" encoding="utf-8"?>
<books>
  <book id="1" ISDN="1001001001">
    <name>我的世界我的梦</name>
    <author>姚明</author>
    <date>2008-09-23</date>
  </book>
  <book id="2" ISDN="2002000230032">
    <name>围城</name>
    <author>钱钟书</author>
    <date>2008-09-23</date>
  </book>
  <book id="3" />
</books>
```

```
Console.WriteLine(XmlHelper.GetXmlAttributeInnerText(@".\Config\Demo.xml", "/books/book[@id='2']", "ISDN"));
Console.WriteLine(XmlHelper.GetXmlNodeInnerText(@".\Config\Demo.xml", "/books/book/author"));
```

```
1.创建XML文档：

　　　　　  //这是XML文档根节点名
            string rootNodeName = "books";
            
            //这是XML文档物理文件名（包含物理路径）
            string xmlFilePath = Application.StartupPath + @"\book.xml";

            XmlHelper.CreateXmlDocument(xmlFilePath, rootNodeName, "1.0", "utf-8", null);
            Console.WriteLine("XML文档创建成功:" + xmlFilePath);

2.向XML文档中添加一个新节点：

            string xmlFilePath = Application.StartupPath + @"\book.xml";
            string xpath = "/books";  //这是新节点的父节点路径
            string nodename = "book";　//这是新节点名称,在父节点下新增
            string nodetext = "这是新节点中的文本值";

            bool isSuccess = XmlHelper.CreateOrUpdateXmlNode(xmlFilePath, xpath, nodename, nodetext);
            Console.WriteLine("XML节点添加或更新成功:" + isSuccess.ToString());

3.向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点,比如name,author,date节点等：

            string xmlFilePath = Application.StartupPath + @"\book.xml";
            string xpath = "/books/book";  //这是新子节点的父节点路径
            string nodename = "name";　//这是新子节点名称,在父节点下新增
            string nodetext = "我的世界我的梦";

            bool isSuccess = XmlHelper.CreateOrUpdateXmlNode(xmlFilePath, xpath, nodename, nodetext);
            Console.WriteLine("XML节点添加或更新成功:" + isSuccess.ToString());

4. 向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点属性,比如id,ISDN属性等：

            string xmlFilePath = Application.StartupPath + @"\book.xml";
            string xpath = "/books/book"; //要新增属性的节点
            string attributeName = "id";　//新属性名称,ISDN号也是这么新增的
            string attributeValue = "1";　//新属性值

            bool isSuccess = XmlHelper.CreateOrUpdateXmlAttribute(xmlFilePath, xpath, attributeName, attributeValue);
            Console.WriteLine("XML属性添加或更新成功:" + isSuccess.ToString());

5. 删除XML文档中的子节点：

            string xmlFilePath = Application.StartupPath + @"\book.xml";
            string xpath = "/books/book[@id='1']"; //要删除的id为1的book子节点

            bool isSuccess = XmlHelper.DeleteXmlNode(xmlFilePath, xpath);
            Console.WriteLine("XML节点删除成功:" + isSuccess.ToString());

6. 删除XML文档中子节点的属性：

            string xmlFilePath = Application.StartupPath + @"\book.xml";
            //删除id为2的book子节点中的ISDN属性
            string xpath = "/books/book[@id='2']";
            string attributeName = "ISDN";

            bool isSuccess = XmlHelper.DeleteXmlAttribute(xmlFilePath, xpath,attributeName);
            Console.WriteLine("XML属性删除成功:" + isSuccess.ToString());

7.读取XML文档中的所有子节点：

            string xmlFilePath = Application.StartupPath + @"\book.xml";
            //要读的id为1的book子节点
            string xpath = "/books/book[@id='1']";

            XmlNodeList nodeList = XmlHelper.GetXmlNodeList(xmlFilePath, xpath);
            string strAllNode = string.Empty;
            //遍历节点中所有的子节点
            foreach (XmlNode node in nodeList)
            {
                strAllNode += "\n name:" + node.Name + " InnerText:" + node.InnerText;
            }

            Console.WriteLine("XML节点中所有子节点有:" + strAllNode);

8.其它...
```

### Path路径操作

```
string path = "C:\\dir1\\dir2\\foo.txt";
string str = "GetFullPath：" + Path.GetFullPath(path) + "\r\n";//GetFullPath：C:\dir1\dir2\foo.txt
str += "GetDirectoryName：" + Path.GetDirectoryName(path) + "\r\n";//GetDirectoryName：C:\dir1\dir2
str += "GetFileName：" + Path.GetFileName(path) + "\r\n";//GetFileName：foo.txt
str += "GetFileNameWithoutExtension：" + Path.GetFileNameWithoutExtension(path) + "\r\n";//GetFileNameWithoutExtension：foo
str += "GetExtension：" + Path.GetExtension(path) + "\r\n";//GetExtension：.txt
str += "GetPathRoot：" + Path.GetPathRoot(path) + "\r\n";//GetPathRoot：C:\
Console.WriteLine(str);
```

### 目录操作

- Directory.GetFiles()

```
//当前文件夹下的所有文件（不包括子文件夹中的文件）
string[] strFiles1 = Directory.GetFiles("..\\Debug");
string[] strFiles2 = Directory.GetFiles("..\\Debug", "*.*");
//当前文件夹下的所有*.exe文件（不包括子文件夹中的文件）
string[] strFiles3 = Directory.GetFiles("..\\Debug", "*.exe");
string[] strFiles4 = Directory.GetFiles("..\\Debug", "*.exe", SearchOption.TopDirectoryOnly);
//当前文件夹下的所有*.exe文件（包括子文件夹中的文件）
string[] strFiles5 = Directory.GetFiles("..\\Debug", "*.exe", SearchOption.AllDirectories);
//当前文件夹下的所有*.exe或*.txt文件（包括子文件夹中的文件）
var strFiles7 = Directory.GetFiles("..\\Debug", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".exe") || s.EndsWith(".txt")).ToArray<string>();
//当前文件夹下的所有文件（包括子文件夹中的文件）
string[] strFiles8 = Directory.GetFiles("..\\Debug", "*.*", SearchOption.AllDirectories);
```

- Directory.GetDirectories()

```
//当前文件夹下的所有文件夹（不包括子文件夹）
string[] strDir1 = Directory.GetDirectories("..\\Debug");
//当前文件夹下的所有test文件夹（不包括子文件夹），个数为0或1
string[] strDir2 = Directory.GetDirectories("..\\Debug", "test");
string[] strDir3 = Directory.GetDirectories("..\\Debug", "test", SearchOption.TopDirectoryOnly);
//当前文件夹下的所有test文件夹（包括子文件夹）
string[] strDir4 = Directory.GetDirectories("..\\Debug", "test", SearchOption.AllDirectories);*/
```

### 附加控制台：`CmdHelper`

```
CmdHelper.Create();
CmdHelper.WriteLine("测试内容");
CmdHelper.Free();
```

### 条形码

> `Code39`

```
Code39是条形码的一种。由于编制简单、能够对任意长度的数据进行编码、支持设备广泛等特性而被广泛采用。

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
5、code39通常情况下不需要校验码。但是对於精确度要求高的应用，需要在code39条形码後面增加一个校验码。
```

> `Code128`

```
CODE128条形码是广泛应用在企业内部管理、生产流程、物流控制系统方面的条码码制，由于其优良的特性在管理信息系统的设计中被广泛使用，CODE128码是应用最广泛的条码码制之一。
CODE128条形码是1981年引入的一种高密度条码，CODE128码可表示从 ASCII 0 到ASCII 127 共128个字符，故称128码。其中包含了数字、字母和符号字符。

Code128各编码方式的编码范围：
1、Code128A：标准数字和字母，控制符，特殊字符；
2、Code128B：标准数字和字母，小写字母，特殊字符；
3、Code128C/EAN128：[00]-[99]的数字对集合，共100个，即只能表示偶数位长度的数字。
```

```
Code128 code128 = new Code128 { Height = 38, ValueFont = new Font("Arial", 9) };
code128.GetImage("test12345!@", Code128.Code128Type.Code128B).Save(@"D:\\1.jpg");
```

### 扩展方法

> `ModelExtensions`

- `ToDictionary<T>()`

```
var model = new CheckInLog
{
    IP = "ceshi",
    CheckInType = 1
}.ToDictionary(true, new Dictionary<string, Func<object, object>>
{
    { $"{nameof(CheckInLog.IP)}", c => $"{c}_test123"},
    { $"checkintype", c =>
    {
        if (c is int i)
        {
            return i + 2;
        }
        return c;
    }}
});
Console.WriteLine(model["IP"]);
Console.WriteLine(model["CheckInType"]);

=============================================================
ceshi_test123
3
```