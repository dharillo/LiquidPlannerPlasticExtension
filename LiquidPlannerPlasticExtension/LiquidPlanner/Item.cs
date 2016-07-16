using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LiquidPlannerPlasticExtension.LiquidPlanner
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
        [DataMember(Name = "description")]
        public string Description { get;  set; }
        [DataMember(Name = "is_on_hold")]
        public bool IsOnHold { get; set; }
        [DataMember(Name = "is_done")]
        public bool IsDone { get; set; }
        [DataMember(Name = "created_by")]
        public int CreatorId { get; set; }

        public override string ToString()
        {
            return "Item: " + this.Id + "(" + this.Type + ")>" + this.Name;
        }

        internal string GetStatus()
        {
            if (IsOnHold)
            {
                return "On Hold";
            }
            else if (IsDone)
            {
                return "Closed";
            }
            else
            {
                return "Open";
            }
        }
    }
}
