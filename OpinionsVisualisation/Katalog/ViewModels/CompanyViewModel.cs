using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Katalog.ViewModels
{
    public class CompanyViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Opis firmy")]
        public string Description { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Strona WWW")]
        public string WebPage { get; set; }


        [Display(Name = "Absolvent.pl")]
        public string AbsolventWebPage { get; set; }

        [Display(Name = "GoldenLine.pl")]
        public string GoldenLineWebPage { get; set; }

        [Display(Name = "Gowork.pl")]
        public string GoworkWebPage { get; set; }


        [ScaffoldColumn(false)]
        public int CategoryId { get; set; }

        [Display(Name = "Kategoria")]
        public string CategoryName { get; set; }


        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public string UserName { get; set; }

        [Display(Name = "Adres IP")]
        public string AddressIP { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data umieszczenia")]
        public DateTime PostedDate { get; set; }

        [ScaffoldColumn(false)]
        public string IsActive { get; set; }
    }
}