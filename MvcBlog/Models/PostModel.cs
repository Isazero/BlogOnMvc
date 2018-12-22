using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcBlog.Models
{
    public class PostModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        
    }
}