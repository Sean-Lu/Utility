## 简介

> 基于ICSharpCode.SharpZipLib.dll封装，支持：zip压缩\解压

> 常见压缩方式：

1. 微软自带压缩类`ZipArchive`，需要`NET FrameWork4.5`才可以使用
2. 调用压缩软件命令的方式，需要电脑安装压缩软件，如：RAR、7Zip等
3. 使用第三方提供的压缩类库，如：ICSharpCode.SharpZipLib.dll

> 常见使用场景

1. 压缩单个文件、文件夹
2. 压缩多个文件、文件夹
3. 压缩包加密
4. 解压
5. 向已存在的压缩文件添加文件