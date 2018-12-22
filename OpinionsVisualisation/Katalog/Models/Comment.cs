using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Katalog.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string IPAddress { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int CommentCategoryId { get; set; }

        public virtual Company Companies { get; set; }
        public virtual CommentCategory CommentCategory { get; set; }
    }
}