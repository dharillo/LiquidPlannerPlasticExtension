using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LiquidPlannerPlasticExtension.LiquidPlanner
{
    [DataContract]
    internal class Comment
    {
        [DataMember(Name = "item_id")]
        public int CommentId { get; set; }

        [DataMember(Name = "comment")]
        public string Content { get; set; }
    }
}
