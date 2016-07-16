using System.Runtime.Serialization;

namespace LiquidPlannerPlasticExtension.LiquidPlanner
{
    [DataContract]
    internal class BaseObject
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
