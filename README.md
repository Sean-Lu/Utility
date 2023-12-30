[TOC]

## 简介

> Ctrl + C, Ctrl + V ??? 没必要重复搬砖，做点更加有意义的事情吧！！！

## Packages

| Package                                                                              | NuGet Stable                                                                                                                                         | NuGet Pre-release                                                                                                                                       | Downloads                                                                                                                                             |
| ------------------------------------------------------------------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Sean.Utility](https://www.nuget.org/packages/Sean.Utility/)                         | [![Sean.Utility](https://img.shields.io/nuget/v/Sean.Utility.svg)](https://www.nuget.org/packages/Sean.Utility/)                                     | [![Sean.Utility](https://img.shields.io/nuget/vpre/Sean.Utility.svg)](https://www.nuget.org/packages/Sean.Utility/)                                     | [![Sean.Utility](https://img.shields.io/nuget/dt/Sean.Utility.svg)](https://www.nuget.org/packages/Sean.Utility/)                                     |
| [Sean.Utility.Office.NPOI](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) | [![Sean.Utility.Office.NPOI](https://img.shields.io/nuget/v/Sean.Utility.Office.NPOI.svg)](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) | [![Sean.Utility.Office.NPOI](https://img.shields.io/nuget/vpre/Sean.Utility.Office.NPOI.svg)](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) | [![Sean.Utility.Office.NPOI](https://img.shields.io/nuget/dt/Sean.Utility.Office.NPOI.svg)](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) |

## Sean.Utility

> Library of utility methods.

| Class                                         | 说明                                                                                                                                                               |
| --------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Sean.Utility.Impls.Log.SimpleLocalLogger<T>` | **`日志`**，支持输出格式：<br>1. Console：控制台输出<br>2. 本地文件：`SimpleLocalLoggerBase.LogFilePath`<br>3. 自定义输出：`SimpleLocalLoggerBase.CustomOutputLog` |
| `Sean.Utility.Impls.Queue.SimpleQueue<T>`     | **`队列`**，仅支持本地队列，无中间件                                                                                                                               |
| `Sean.Utility.Impls.Queue.SimpleLocalMQ<T>`   | 线程安全的 **`本地消息队列`**：<br>1. 支持多线程下的生产者\消费者模式<br>2. 支持多生产者、多消费者                                                                 |
| `Sean.Utility.Serialize.JsonSerializer`       | json序列化\反序列化                                                                                                                                                |
| `Sean.Utility.Serialize.XmlSerializer`        | XML序列化\反序列化                                                                                                                                                 |
| `Sean.Utility.Serialize.BinarySerializer`     | 二进制序列化\反序列化                                                                                                                                              |
| `Sean.Utility.IO.XmlHelper`                   | XML文件操作                                                                                                                                                        |
| `Sean.Utility.CmdHelper`                      | 1. 执行指定的cmd命令<br>2. 应用程序分配或附加控制台                                                                                                                |
| `Sean.Utility.Net.FileShareHelper`            | 访问网络共享文件夹（基于磁盘映射）                                                                                                                                 |

- Nuget包从 `2.1.0` 版本开始，去除了对第三方Nuget包 `Newtonsoft.Json` 的依赖：

```
内置 `JsonSerializer` （基于 `DataContractJsonSerializer` 实现）。
引用第三方Nuget包后，可自定义实现 `IJsonSerializer` 接口。

使用示例1：依赖注入
services.AddTransient<IJsonSerializer, JsonSerializer>();
services.AddTransient<INewJsonSerializer, NewJsonSerializer>();

使用示例2：JsonHelper
JsonHelper.Serializer = NewJsonSerializer.Instance;
JsonHelper.Serialize(obj);
```

- [Json.NET vs .NET Serializers](https://www.newtonsoft.com/json/help/html/jsonnetvsdotnetserializers.htm)
  
  - `Json.NET`：`Newtonsoft.Json`
  
  - `DataContractJsonSerializer`：`System.Runtime.Serialization.Json`
  
  - `JavaScriptSerializer`：`System.Web.Script.Serialization`

## Sean.Utility.Security

> 支持常见的加解密：Base64、Hex、Hash、AES、DES、TripleDES、RC2、RC4、RSA

| Class                     | 描述                                                       |
| ------------------------- | -------------------------------------------------------- |
| `Base64CryptoProvider`    | 编码\解码：`Base64`                                           |
| `HexCryptoProvider`       | 编码\解码：`16进制`                                             |
| `HashCryptoProvider`      | 单向散列算法（**`哈希`**）：`MD5`、`SHA1`、`SHA256`、`SHA384`、`SHA512` |
| `AESCryptoProvider`       | 对称加密算法：`AES`                                             |
| `DESCryptoProvider`       | 对称加密算法：`DES`                                             |
| `TripleDESCryptoProvider` | 对称加密算法：`TripleDES`（3DES）                                 |
| `RC2CryptoProvider`       | 对称加密算法：`RC2`                                             |
| `RC4CryptoProvider`       | 对称加密算法：`RC4`                                             |
| `RSACryptoProvider`       | 非对称加密算法：`RSA`                                            |

- `RSACryptoProvider`：

> 支持2种密钥类型：`Xml`、`OpenSSL`
> 
> 支持跨平台：`Windows`, `Linux`, `Mac`, ...，***但需要注意***：

1. 如果使用`.NET Framework`的版本小于4.6，默认使用 `System.Security.Cryptography.RSACryptoServiceProvider`，这个类并不支持跨平台，在`Windows`上运行正常，但是在`Linux`下运行就会报错（编译不会报错）。
2. 如果在非`Windows`平台下使用`RSACryptoProvider`，建议升级`.NET Framework`的版本（>=4.6），或使用`.NET Core`。
3. 在`Windows`平台下没有以上问题。

### 术语解释

- Base64 是一种基于 64 个可打印字符来表示二进制数据的编码方法。
- AES（Advanced Encryption Standard， 高级加密标准），是目前对称密钥加密中比较通用的一种加密方式。速度快，安全级别高，支持 128、192、256、512 位密钥的加密。
- RSA 签名算法是一种非对称算法，RSA 密钥包括公钥和私钥两部分，公钥是公开信息，私钥是保密信息。私钥用于签名，公钥用于验签。

## Sean.Utility.Compress.SharpZipLib

> Zip compression tool based on SharpZipLib.

## Sean.Utility.Net.Http

> Http client

| Class                                     | 说明                                    |
| ----------------------------------------- | --------------------------------------- |
| `Sean.Utility.Net.Http.HttpClientWrapper` | RESTful接口调用：Get、Post、Put、Delete |

## Sean.Utility.Office.NPOI

> Office file operation (based on NPOI): Excel (supported formats: xls, xlsx).

| Class                                  | 说明                               |
| -------------------------------------- | ---------------------------------- |
| `Sean.Utility.Office.NPOI.ExcelHelper` | Excel文件操作，支持格式：xls、xlsx |

## Sean.Utility.Web

> Web

## Sean.Utility.WindowsForms

> WindowsForms
