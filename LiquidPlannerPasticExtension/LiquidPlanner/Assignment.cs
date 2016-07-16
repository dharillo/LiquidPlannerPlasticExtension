using System.Runtime.Serialization;

namespace LiquidPlannerPlasticExtension.LiquidPlanner
{
    [DataContract]
    internal class Assignment: BaseObject
    {
        [DataMember(Name = "person_id")]
        public int? PersonId { get; set; }
        [DataMember(Name = "team_id")]
        public int? team_id { get; set; }
    }
}
