using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;

namespace Katalog.Models
{
    [MetadataType(typeof(CustomerMetaData))]
    public class Customer
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime RegistrationDate { get; set; }
        
        public int UserId { get; set; }
    }

    public class CustomerMetaData
    {
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane.")]
        [StringLength(40, ErrorMessage = "Miasto może mieć maksymalnie 40 znaków.")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^[0-9]{2}\-[0-9]{3}$", ErrorMessage = "Wprowadzony kod jest niepoprawny.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Ulica jest wymagana.")]
        [StringLength(40, ErrorMessage = "Ulica może mieć maksymalnie 40 znaków.")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Potwierdzony")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Data rejestracji")]
        public DateTime RegistrationDate { get; set; }
    }

    public class CustomerViewModel
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        public bool IsConfirmed { get; set; }

        [Display(Name = "Aktywny")]
        public string IsActive { get; set; }

        [Display(Name = "Data rejestracji")]
        public DateTime RegistrationDate { get; set; }

       
    }

    public class CustomerFilterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }

    public class CustomerListContainerViewModel
    {
        public IPagination<CustomerViewModel> CustomerPagedList { get; set; }
        public CustomerFilterViewModel CustomerFilterViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }
}