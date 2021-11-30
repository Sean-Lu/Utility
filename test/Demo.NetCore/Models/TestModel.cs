using System;

namespace Demo.NetCore.Models
{
    //[Serializable]
    public class TestModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
