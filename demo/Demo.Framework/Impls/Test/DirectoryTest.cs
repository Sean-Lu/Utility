using System.IO;
using System.Linq;
using Sean.Utility.Contracts;

namespace Demo.Framework.Impls.Test
{
    public class DirectoryTest : ISimpleDo
    {
        public void Execute()
        {
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


            //当前文件夹下的所有文件夹（不包括子文件夹）
            string[] strDir1 = Directory.GetDirectories("..\\Debug");
            //当前文件夹下的所有test文件夹（不包括子文件夹），个数为0或1
            string[] strDir2 = Directory.GetDirectories("..\\Debug", "test");
            string[] strDir3 = Directory.GetDirectories("..\\Debug", "test", SearchOption.TopDirectoryOnly);
            //当前文件夹下的所有test文件夹（包括子文件夹）
            string[] strDir4 = Directory.GetDirectories("..\\Debug", "test", SearchOption.AllDirectories);
        }
    }
}
