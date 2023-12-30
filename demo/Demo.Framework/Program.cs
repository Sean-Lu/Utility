using System;
using System.Collections.Generic;
using System.Data;
using Demo.Framework.Impls.Test;
using Newtonsoft.Json;
using Sean.Utility.Contracts;

namespace Demo.Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            //JsonHelper.Serializer = NewJsonSerializer.Instance;

            ISimpleDo toDo = new RandomTest();
            Console.WriteLine(toDo.GetType());
            toDo.Execute();

            Console.WriteLine("--------------->Done.");
            Console.ReadLine();
        }
    }
}
