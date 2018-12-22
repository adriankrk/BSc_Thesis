using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using Katalog.ViewModels;


namespace Katalog.Models
{
    [MetadataType(typeof(ServiceProviderMetaData))]
    public class ServiceProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime RegistrationDate { get; set; }
        
        public int UserId { get; set; }
    }

    public class ServiceProviderMetaData
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [Display(Name = "E-mail")]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", ErrorMessage = "Adres e-mail jest niepoprawny.")]
        public string Email { get; set; }

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

        [Display(Name = "Nr telefonu")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Wprowadzony numer telefonu jest niepoprawny.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Potwierdzony")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Data rejestracji")]
        public DateTime RegistrationDate { get; set; }

       

        [ScaffoldColumn(false)]
        public int UserId { get; set; }
    }

    public class ServiceProviderViewModel
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Nr telefonu")]
        public string PhoneNumber { get; set; }

        public bool IsConfirmed { get; set; }

        [Display(Name = "Aktywny")]
        public string IsActive { get; set; }

        [Display(Name = "Data rejestracji")]
        public DateTime RegistrationDate { get; set; }
    }

    public class ServiceProviderFilterViewModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }

    public class ServiceProviderListContainerViewModel
    {
        public IPagination<ServiceProviderViewModel> ServiceProviderPagedList { get; set; }
        public ServiceProviderFilterViewModel ServiceProviderFilterViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }

    public class ServiceProviderCompaniesListContainerViewModel
    {
        public CompanyListContainerViewModel Companies { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
    }
}