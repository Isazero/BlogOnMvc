using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvcBlogApi.Models
{
    [DataContract]
    public class RoleModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public virtual ICollection<UserModel> Users { get; set; }
    }
}