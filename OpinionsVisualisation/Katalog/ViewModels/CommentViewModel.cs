using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Katalog.ViewModels
{
    public class CommentViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Treść jest wymagana.")]
        [Display(Name = "Treść komentarza")]
        [StringLength(200, ErrorMessage = "Treść komentarza może mieć maksymalnie 200 znaków.")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data dodania")]
        public DateTime Date { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Firma")]
        public int ServiceId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Firma")]
        public string Service { get; set; }

        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public string User { get; set; }

        [Display(Name = "Adres IP")]
        public string IPAddress { get; set; }

        [Display(Name = "Kategoria")]
        public string CommentCategory { get; set; }

        [Display(Name = "Kategoria")]
        public int CommentCategoryId { get; set; }
    }
}