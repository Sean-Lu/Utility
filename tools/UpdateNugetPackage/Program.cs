using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sean.Utility.Compress.SharpZipLib;
using Sean.Utility.IO;

namespace UpdateNugetPackage
{
    class Program
    {
        private const string NugetPackagePath = "-path";
        private const string NugetPackageDir = "-dir";
        private const string NugetPackageId = "-id";
        private const string NugetPackageVersion = "-version";

        private static string _xmlFilePath;
        private static string _xpathBase;
        private static string _baseDir;

        private static List<string> _nugetPackageFilePathList = new List<string>();
        private static List<string> _successList = new List<string>();
        private static List<string> _failList = new List<string>();

        static Program()
        {
            _xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"nuget.xml");
            if (string.IsNullOrWhiteSpace(_xmlFilePath) || !File.Exists(_xmlFilePath))
            {
                throw new Exception($"nuget配置文件不存在：{_xmlFilePath}");
            }

            _baseDir = XmlHelper.GetXmlAttributeValue(_xmlFilePath, "/nuget/baseDir", "value");
        }

        static void Main(string[] args)
        {
            try
            {
                #region 外部参数获取
                if (args == null || args.Length < 1)
                {
                    Console.WriteLine($"参数不能为空[{nameof(args)}]");
                    return;
                }

                var nugetPackageDir = string.Empty;//"D:\Sean\Project\Code\Sean.Utility\src\Sean.Utility\bin\Debug";
                var nugetPackageId = string.Empty;//"Sean.Utility";
                var nugetPackageVersion = string.Empty;//"2.0.0-beta2019052701";
                for (var i = 0; i < args.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(nugetPackageDir) && args[i] == NugetPackageDir && i + 1 < args.Length)
                    {
                        nugetPackageDir = args[i + 1];
                    }
                    if (string.IsNullOrWhiteSpace(nugetPackageId) && args[i] == NugetPackageId && i + 1 < args.Length)
                    {
                        nugetPackageId = args[i + 1];
                    }
                    if (string.IsNullOrWhiteSpace(nugetPackageVersion) && args[i] == NugetPackageVersion && i + 1 < args.Length)
                    {
                        nugetPackageVersion = args[i + 1];
                    }
                }
                #endregion

                #region 参数校验
                if (string.IsNullOrWhiteSpace(nugetPackageDir))
                {
                    Console.WriteLine($"参数不能为空：[{nameof(nugetPackageDir)}]");
                    return;
                }
                if (string.IsNullOrWhiteSpace(nugetPackageId))
                {
                    Console.WriteLine($"参数不能为空：[{nameof(nugetPackageId)}]");
                    return;
                }
                #endregion

                _xpathBase = $"/nuget/package[@id='{nugetPackageId}']";
                if (string.IsNullOrWhiteSpace(_baseDir))
                {
                    _baseDir = Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}..\..\..\..\src");
                }

                if (!string.IsNullOrWhiteSpace(nugetPackageVersion))
                {
                    // 指定nugetPackageId + nugetPackageVersion
                    var filepath = Path.Combine(nugetPackageDir, $"{nugetPackageId}.{nugetPackageVersion}.nupkg");
                    _nugetPackageFilePathList.Add(filepath);
                }
                else
                {
                    // 指定nugetPackageId
                    var filePaths = Directory.GetFiles(nugetPackageDir, $"{nugetPackageId}*.nupkg", SearchOption.TopDirectoryOnly);
                    foreach (var filePath in filePaths)
                    {
                        //Console.WriteLine(filePath);
                        _nugetPackageFilePathList.Add(filePath);
                    }
                }

                foreach (var nugetPackageFilePath in _nugetPackageFilePathList)
                {
                    var success = Do(Path.GetFullPath(nugetPackageFilePath));
                    if (success)
                    {
                        _successList.Add(nugetPackageFilePath);
                    }
                    else
                    {
                        _failList.Add(nugetPackageFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"出错：{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            finally
            {
                Console.WriteLine("===================================================");
                Console.ForegroundColor = _successList.Count > 0 && _failList.Count < 1 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"执行结束：成功[{_successList.Count}],失败[{_failList.Count}].");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static bool Do(string nugetPackageFilePath)
        {
            if (string.IsNullOrWhiteSpace(nugetPackageFilePath))
            {
                return false;
            }
            if (!File.Exists(nugetPackageFilePath))
            {
                Console.WriteLine($"未找到nuget包：{nugetPackageFilePath}");
                return false;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(nugetPackageFilePath);
            Console.ForegroundColor = ConsoleColor.White;

            var addFileList = new List<ContentToZip>();
            var xmlNodeList = XmlHelper.GetXmlNodeList(_xmlFilePath, $"{_xpathBase}/addFiles/file");
            if (xmlNodeList != null)
            {
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    if (xmlNode.Attributes != null)
                    {
                        var path = Path.Combine(_baseDir, xmlNode.Attributes["path"]?.Value ?? string.Empty);
                        var target = xmlNode.Attributes["target"]?.Value;
                        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                        {
                            Console.WriteLine($"文件不存在：{path}");
                            return false;
                        }
                        Console.WriteLine($"{path} => {target}");
                        addFileList.Add(new ContentToZip
                        {
                            Path = path,
                            RelativeFolder = target
                        });
                    }
                }
            }

            if (addFileList.Count < 1)
            {
                Console.WriteLine("没有文件需要添加");
                return false;
            }

            // nuget包添加文件
            return ZipHelper.AddFilesToZip(nugetPackageFilePath, addFileList);
        }
    }
}
