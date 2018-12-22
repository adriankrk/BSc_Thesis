using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Katalog.Models
{
    public class CommentMetaData
    {
        [Required(ErrorMessage = "Treść jest wymagana.")]
        [Display(Name = "Treść komentarza")]
        [StringLength(200, ErrorMessage = "Treść komentarza może mieć maksymalnie 200 znaków.")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data dodania")]
        public DateTime Date { get; set; }

        [Display(Name = "Kategoria")]
        public string CommentCategory { get; set; }
    }

    [MetadataType(typeof(CommentMetaData))]
    public partial class Comment
    {

    }
}