using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Katalog.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj")]
        public bool RememberMe { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Aktualne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi posiadać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Wprowadzone hasła są różne.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Login użytkownika jest wymagany.")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [Display(Name = "E-mail")]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", ErrorMessage = "Adres e-mail jest niepoprawny.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi posiadać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła są różne.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Chcesz dodawać wpisy?")]
        public bool IsProvider { get; set; }

        [StringLength(50, ErrorMessage = "Nazwisko może mieć maksymalnie 50 znaków.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "Imię może mieć maksymalnie 50 znaków")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane.")]
        [StringLength(40, ErrorMessage = "Miasto może mieć maksymalnie 40 znaków.")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ulica jest wymagana.")]
        [StringLength(40, ErrorMessage = "Ulica może mieć maksymalnie 40 znaków.")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^[0-9]{2}\-[0-9]{3}$", ErrorMessage = "Wprowadzony kod jest niepoprawny.")]
        public string ZipCode { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Numer telefonu")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Wprowadzony numer telefonu jest niepoprawny.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Wykonaj poniższe działanie")]
        public string Captcha { get; set; }

    }
}
