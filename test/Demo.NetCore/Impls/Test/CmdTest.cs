using Sean.Utility.Common;
using Sean.Utility.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.NetCore.Impls.Test
{
    public class CmdTest : ISimpleDo
    {
        public void Execute()
        {
            ProcessHelper.RunProcessByCmd(@"D:\Sean\Code\Personal\Tools\CsvSplitter\CsvSplitter\bin\Debug\CsvSplitter.exe", null, null, null);
        }
    }
}
