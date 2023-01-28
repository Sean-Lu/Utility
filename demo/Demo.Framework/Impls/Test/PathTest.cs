using System;
using System.IO;
using Sean.Utility.Contracts;

namespace Demo.Framework.Impls.Test
{
    public class PathTest : ISimpleDo
    {
        public void Execute()
        {
            string path = "C:\\dir1\\dir2\\foo.txt";
            string str = "GetFullPath：" + Path.GetFullPath(path) + "\r\n";//GetFullPath：C:\dir1\dir2\foo.txt
            str += "GetDirectoryName：" + Path.GetDirectoryName(path) + "\r\n";//GetDirectoryName：C:\dir1\dir2
            str += "GetFileName：" + Path.GetFileName(path) + "\r\n";//GetFileName：foo.txt
            str += "GetFileNameWithoutExtension：" + Path.GetFileNameWithoutExtension(path) + "\r\n";//GetFileNameWithoutExtension：foo
            str += "GetExtension：" + Path.GetExtension(path) + "\r\n";//GetExtension：.txt
            str += "GetPathRoot：" + Path.GetPathRoot(path) + "\r\n";//GetPathRoot：C:\
            Console.WriteLine(str);
        }
    }
}
