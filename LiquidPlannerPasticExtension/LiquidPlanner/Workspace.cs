using System.Runtime.Serialization;

namespace LiquidPlannerPasticExtension.LiquidPlanner
{
    [DataContract]
    internal class Workspace: BaseObject
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return "Space: " + this.Id + "> " + this.Name;
        }
    }
}
