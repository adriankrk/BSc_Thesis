using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Katalog.Models
{
    [MetadataType(typeof(CommentCategoryMetaData))]
    public class CommentCategory
    {
        public CommentCategory()
        {
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class CommentCategoryMetaData
    {
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
    }
}