using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LiquidPlannerPasticExtension.LiquidPlanner
{
    [DataContract]
    internal class Item: BaseObject
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "parent_id")]
        public int ParentId { get; set; }
        [DataMember(Name = "assignments")]
        public List<Assignment> Assignments { get; set; }

        public override string ToString()
        {
            return "Item: " + this.Id + "(" + this.Type + ")>" + this.Name;
        }
    }
}
