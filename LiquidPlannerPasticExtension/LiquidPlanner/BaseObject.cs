using System.Runtime.Serialization;

namespace LiquidPlannerPasticExtension.LiquidPlanner
{
    [DataContract]
    internal class BaseObject
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
