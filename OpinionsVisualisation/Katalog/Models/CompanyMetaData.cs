using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Katalog.Models
{
    public class CompanyMetaData
    {
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany.")]
        [Display(Name = "Opis firmy")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefon jest wymagany.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Strona www jest wymagana")]
        [Display(Name = "Strona WWW")]
        public string WebPage { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }


        [Display(Name = "Absolvent.pl")]
        public string AbsolventWebPage { get; set; }

        [Display(Name = "GoldenLine.pl")]
        public string GoldenLineWebPage { get; set; }

        [Display(Name = "Gowork.pl")]
        public string GoworkWebPage { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Data umieszczenia")]
        public DateTime PostedDate { get; set; }
    }

    [MetadataType(typeof(CompanyMetaData))]
    public partial class Company
    {

    }
}