using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using BlogModel;

namespace MvcBlogApi.Models
{
    [DataContract]
    public class PostModel
    {
        [IgnoreDataMember]
        public int PostId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Slug { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime PublishDate { get; set; }
        [IgnoreDataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public ICollection<CommentModel> Comments { get; set; }

    }
}