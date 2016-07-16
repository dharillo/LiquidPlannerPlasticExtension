using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LiquidPlannerPlasticExtension.LiquidPlanner
{
    [DataContract]
    internal class Group
    {
        [DataMember(Name = "group")]
        public string Name { get; set; }
        [DataMember(Name = "items")]
        public List<Item> Items { get; set; }
    }
}
