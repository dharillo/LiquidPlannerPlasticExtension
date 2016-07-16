using System.Runtime.Serialization;

namespace LiquidPlannerPlasticExtension.LiquidPlanner
{
    /// <summary>
    /// Class for respresenting the information of the account of a
    /// LiquidPlanner user
    /// </summary>
    [DataContract]
    internal class Member: BaseObject
    {
        [DataMember(Name = "user_name")]
        public string Name { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; internal set; }

        public override string ToString()
        {
            return "User: " + this.Id + "> " + this.Name + "> " + this.FirstName + " " + this.LastName;
        }
    }
}
