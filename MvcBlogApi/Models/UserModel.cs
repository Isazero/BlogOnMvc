using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvcBlogApi.Models
{
    [DataContract]
    public class UserModel
    {
        [IgnoreDataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [IgnoreDataMember]
        public bool IsDeleted { get; set; }
        [IgnoreDataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public virtual ICollection<CommentModel> Comments { get; set; }

        [DataMember]
        public virtual ICollection<PostModel> Posts { get; set; }

        [IgnoreDataMember]
        public virtual RoleModel Role { get; set; }
    }
}