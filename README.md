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

| Class                                                | 说明                                                                                                                                      |
| ---------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| `Sean.Utility.Impls.Log.SimpleLocalLogger<T>`        | **`日志`**，支持输出格式：<br>1. Console：控制台输出<br>2. 本地文件：`SimpleLocalLoggerBase.LogFilePath`<br>3. 自定义输出：`SimpleLocalLoggerBase.CustomOutputLog` |
| `Sean.Utility.Impls.Queue.SimpleQueue<T>`            | **`队列`**，仅支持本地队列，无中间件                                                                                                                   |
| `Sean.Utility.Serialize.JsonSerializer`              | json序列化\反序列化                                                                                                                            |
| `Sean.Utility.Serialize.XmlSerializer`               | XML序列化\反序列化                                                                                                                             |
| `Sean.Utility.Serialize.BinarySerializer`            | 二进制序列化\反序列化                                                                                                                             |
| `Sean.Utility.IO.XmlHelper`                          | XML文件操作                                                                                                                                 |
| `Sean.Utility.Common.CmdHelper`                      | 1. 执行指定的cmd命令<br>2. 应用程序分配或附加控制台，方便输出调试信息                                                                                               |
| `Sean.Utility.Net.FileShare.NetworkConnectionHelper` | 访问网络共享文件夹（基于磁盘映射）                                                                                                                       |
| `Sean.Utility.Net.Ftp.FtpClient`                     | FTP客户端                                                                                                                                  |

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

## Sean.Utility.Compress.SharpZipLib

> Zip compression tool based on SharpZipLib.

## Sean.Utility.Net.Http

> Http client

| Class                              | 说明                              |
| ---------------------------------- | ------------------------------- |
| `Sean.Utility.Net.Http.HttpHelper` | RESTful接口调用：Get、Post、Put、Delete |

## Sean.Utility.Office.NPOI

> Office file operation (based on NPOI): Excel (supported formats: xls, xlsx).

| Class                                  | 说明                      |
| -------------------------------------- | ----------------------- |
| `Sean.Utility.Office.NPOI.ExcelHelper` | Excel文件操作，支持格式：xls、xlsx |

## Sean.Utility.Web

> Web

## Sean.Utility.WindowsForms

> WindowsForms
