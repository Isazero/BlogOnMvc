using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvcBlogApi.Models
{
    [DataContract]
    public class CommentModel
    {
        [IgnoreDataMember]
        public int CommentId { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int PostId { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }


        [DataMember]
        public virtual PostModel Post { get; set; }

        [DataMember]
        public virtual UserModel User { get; set; }
    }
}