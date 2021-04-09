using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Head { get; set; }
        [StringLength(150, MinimumLength = 3)]
        public string Body { get; set; }
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
