using System;
using System.Dynamic;
using Sean.Utility.Contracts;
using Sean.Utility.Format;

namespace Demo.Framework.Impls.Test
{
    /// <summary>
    /// json序列化测试
    /// </summary>
    public class JsonTest : ISimpleDo
    {
        public void Execute()
        {
            dynamic obj = new ExpandoObject();
            obj.Id = 1001;
            obj.Name = "Sean";
            var json = JsonHelper.Serialize(obj);
            var dynamicObj = JsonHelper.Deserialize<dynamic>(json);
            var id = dynamicObj.Id;
            var name = dynamicObj.Name;
            Console.WriteLine(json);
        }
    }
}
