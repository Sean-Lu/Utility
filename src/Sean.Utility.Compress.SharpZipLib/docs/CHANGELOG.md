# 更新日志

## [2.0.2] - 2020-05-31

## [2.0.0] - 2019-05-31

> 新增

- 压缩文件夹支持只压缩子文件、子文件夹：`ZipDirectory(..., bool onlySubItems = false)`
- 支持向已存在的压缩文件添加文件或文件夹

> 修改

- 压缩文件不强制后缀名为`.zip`

## [1.0.1] - 2019-05-13

> 新增

- 支持自定义压缩包内相对路径，示例：
```
var zipFilePath = @"D:\123.zip";
var zipResult = ZipHelper.ZipFilesOrDirectories(new Dictionary<string, string>
{
    {@"D:\aaa\bbb\1.txt", string.Empty},// 不设置压缩包内相对路径（默认）
    {@"D:\aaa\bbb\2.txt", "test"},// 设置压缩包内相对路径
    {@"D:\aaa\bbb\3.txt", @"test\123"}// 设置压缩包内相对路径
}, zipFilePath);
```

## [1.0.0] - 2019-05-10

> 发布第1个版本