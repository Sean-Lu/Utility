[TOC]

## 简介

> Ctrl + C, Ctrl + V ??? 没必要重复搬砖，做点更加有意义的事情吧！！！

## Packages

| Package | NuGet Stable | NuGet Pre-release | Downloads |
| ------- | ------------ | ----------------- | --------- |
| [Sean.Utility](https://www.nuget.org/packages/Sean.Utility/) | [![Sean.Utility](https://img.shields.io/nuget/v/Sean.Utility.svg)](https://www.nuget.org/packages/Sean.Utility/) | [![Sean.Utility](https://img.shields.io/nuget/vpre/Sean.Utility.svg)](https://www.nuget.org/packages/Sean.Utility/) | [![Sean.Utility](https://img.shields.io/nuget/dt/Sean.Utility.svg)](https://www.nuget.org/packages/Sean.Utility/) |
| [Sean.Utility.Office.NPOI](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) | [![Sean.Utility.Office.NPOI](https://img.shields.io/nuget/v/Sean.Utility.Office.NPOI.svg)](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) | [![Sean.Utility.Office.NPOI](https://img.shields.io/nuget/vpre/Sean.Utility.Office.NPOI.svg)](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) | [![Sean.Utility.Office.NPOI](https://img.shields.io/nuget/dt/Sean.Utility.Office.NPOI.svg)](https://www.nuget.org/packages/Sean.Utility.Office.NPOI/) |

## Sean.Utility

> Library of utility methods.

| Class                                                | 说明                                                         |
| ---------------------------------------------------- | ------------------------------------------------------------ |
| `Sean.Utility.Impls.Log.SimpleLocalLogger<T>`        | **`日志`**，支持输出格式：<br>1. Console：控制台输出<br>2. 本地文件：`SimpleLocalLoggerBase.LogFilePath`<br>3. 自定义输出：`SimpleLocalLoggerBase.CustomOutputLog` |
| `Sean.Utility.Impls.Queue.SimpleQueue<T>`            | **`队列`**，仅支持本地队列，无中间件                         |
| `Sean.Utility.Format.JsonHelper`                     | json数据转换（基于 `Newtonsoft.Json.JsonConvert` ）          |
| `Sean.Utility.Serialize.BinarySerializer`            | 二进制序列化\反序列化                                        |
| `Sean.Utility.Serialize.XmlSerializer<T>`            | XML序列化\反序列化                                           |
| `Sean.Utility.IO.XmlHelper`                          | XML文件操作                                                  |
| `Sean.Utility.Common.CmdHelper`                      | 1. 执行指定的cmd命令<br>2. 应用程序分配或附加控制台，方便输出调试信息 |
| `Sean.Utility.Net.FileShare.NetworkConnectionHelper` | 访问网络共享文件夹（基于磁盘映射）                           |
| `Sean.Utility.Net.Ftp.FtpClient`                     | FTP客户端                                                    |

## Sean.Utility.Compress.SharpZipLib

> Zip compression tool based on SharpZipLib.

## Sean.Utility.Net.Http

> Http client

| Class                              | 说明                                    |
| ---------------------------------- | --------------------------------------- |
| `Sean.Utility.Net.Http.HttpHelper` | RESTful接口调用：Get、Post、Put、Delete |

## Sean.Utility.Office.NPOI

> Office file operation (based on NPOI): Excel (supported formats: xls, xlsx).

| Class                                  | 说明                               |
| -------------------------------------- | ---------------------------------- |
| `Sean.Utility.Office.NPOI.ExcelHelper` | Excel文件操作，支持格式：xls、xlsx |

## Sean.Utility.Web

> Web

## Sean.Utility.WindowsForms

> WindowsForms
