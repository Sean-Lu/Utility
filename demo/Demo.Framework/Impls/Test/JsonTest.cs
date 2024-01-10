using System;
using System.Dynamic;
using Demo.Framework.Models;
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
            //dynamic obj = new ExpandoObject();
            //obj.Id = 1001;
            //obj.Name = "Sean";
            //var json = JsonHelper.Serialize(obj);
            //var dynamicObj = JsonHelper.Deserialize<dynamic>(json);
            //var id = dynamicObj.Id;
            //var name = dynamicObj.Name;
            //Console.WriteLine(json);

            var json = "{\"Age\":18,\"CreateTime\":\"2024-01-08T16:23:40+08:00\"}";
            var dynamicObj = JsonHelper.Deserialize<TestModel>(json);
            Console.WriteLine(dynamicObj.Age);
            Console.WriteLine(dynamicObj.CreateTime);
        }
    }
}
