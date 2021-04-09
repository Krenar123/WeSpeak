using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public int ArticleId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Article Article { get; set; }
    }
}
