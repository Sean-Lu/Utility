using System;
using System.Runtime.Serialization;

namespace Demo.Framework.Models
{
    [Serializable]
    //[DataContract]
    public class TestModel
    {
        //[DataMember]
        public long Id { get; set; }
        //[DataMember]
        public string Name { get; set; }
        //[DataMember]
        public int Age { get; set; }
        //[DataMember]
        public string Address { get; set; }
        //[DataMember]
        public DateTime CreateTime { get; set; }
    }
}
