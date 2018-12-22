using System;
using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using WebMatrix.WebData;
using Katalog.Models;
using Katalog.Repositories;
using Katalog.Rejestracja;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace Katalog.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {


        
        // Repozytorium użytkowników       
        private CustomerRepository _customerRepo;
       
        // Repozytorium użytkowników dodajacych wpisy        
        private ServiceProviderRepository _serviceProviderRepo;

       
        // Konstruktor kontrolera Account       
        public AccountController()
        {
            // Inicjalizacja repozytoriów
            _customerRepo = new CustomerRepository();
            _serviceProviderRepo = new ServiceProviderRepository();
        }



        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
             bool isConfirmed = false;

            if (ModelState.IsValid)
            {
                int userId = WebSecurity.GetUserId(model.UserName);

                    // Sprawdzenie, czy użytkownik jest usługobiorcą
                    var isCustomer = _customerRepo.IsCustomer(userId);

                    if (isCustomer)
                    {
                        // Sprawdzenie, czy usługobiorca potwierdził swoje konto
                        isConfirmed = _customerRepo.IsConfirmed(userId);
                    }
                    else
                    {
                        // Sprawdzenie, czy użytkownik jest usługodawcą
                        var isProvider = _serviceProviderRepo.IsServiceProvider(userId);
                        
                        if (isProvider)
                        {
                            // Sprawdzenie, czy usługodawca potwierdził swoje konto
                            isConfirmed = _serviceProviderRepo.IsConfirmed(userId);
                        }
                    }

                 if (Roles.IsUserInRole(model.UserName, "Administrator"))
                    {
                        // Jeżeli użytkownik jest administratorem, to jego konto jest potwierdzone
                        isConfirmed = true;
                    }

                    if (isConfirmed)
                    {
                        // Jeżeli użytkownik ma potwierdzone konto, to następuje zalogowanie
                        if(WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                        {
                        
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Nazwa użytkownika lub hasło są niepoprawne.");
                            return View(model);
                        }
                    }
                    else
                    {
                        //  Jeżeli wyliczony kod zabezpieczający jest różny od kodu przesłanego, to zwracany jest błąd.
                        TempData["Error"] = "Nie potwierdziłeś jeszcze rejestracji. Twoje konto jest nieaktywne.";
                        return View(model);
                    }
            }

            // Wpisane do formularza dane są niepoprawne, więc zwracany jest ten sam widok.
            ModelState.AddModelError("", "Nazwa użytkownika lub hasło są niepoprawne.");

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Confirm(string name, string securityCode)
        {
            int id = WebSecurity.GetUserId(name);

            // Pobranie usługobiorcy po identyfikatorze użytkownika (nie wiadomo, czy potwierdzany użytkownik jest usługodawcą, czy usługobiorcą)
            var customer = _customerRepo.GetCustomerByUserId(id);

            // Pobranie usługodawcy po identyfikatorze użytkownika (nie wiadomo, czy potwierdzany użytkownik jest usługodawcą, czy usługobiorcą)
            var provider = _serviceProviderRepo.GetServiceProviderByUserId(id);

            // Jeśli użytkownik nie jest usługodawcą lub usługobiorcą, to jest to błąd.
            if (customer == null && provider == null)
            {
                TempData["Error"] = "Błąd weryfikacji konta!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string hashDateInDatebase = string.Empty;

                if (customer != null)
                {
                    hashDateInDatebase = Konto.CalculateConfirmationCode(name, customer.Email, customer.RegistrationDate);
                }
                else
                {
                    hashDateInDatebase = Konto.CalculateConfirmationCode(name, provider.Email, provider.RegistrationDate);
                }

                // Jeżeli wyliczony kod zabezpieczający jest taki sam jak kod przesłany, to konto jest potwierdzone.
                if (securityCode.Equals(hashDateInDatebase, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (provider != null)
                    {
                        // Potwierdzenie konta usługodawcy
                        provider.IsConfirmed = true;
                        _serviceProviderRepo.SaveChanges();
                    }
                    else
                    {
                        // Potwierdzenie konta usługobiorcy
                        customer.IsConfirmed = true;
                        _customerRepo.SaveChanges();
                    }

                    TempData["Message"] = "Pomyślnie potwierdzono adres e-mail!";
                }
                else
                {
                    //  Jeżeli wyliczony kod zabezpieczający jest różny od kodu przesłanego to zwracany jest błąd.
                    TempData["Error"] = "Błąd weryfikacji konta!";
                }
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [Captcha]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, bool captchaValid)
        {
            if (captchaValid)
            {
               if (ModelState.IsValid)
                {
                    try
                    {
                        // Utworzenie użytkownika przy pomocy mechanizmu SimpleMembership
                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                        int userId = WebSecurity.GetUserId(model.UserName);
                        DateTime registrationDate = DateTime.Now;

                        try
                        {
                            // Tworzenie usługodawcy.
                            if (model.IsProvider)
                            {
                                // Utworzenie roli Usługodawca, jeśli jeszcze nie istnieje 
                                string roleName = ConfigurationManager.AppSettings["ProviderRole"];

                                if (!Roles.RoleExists(roleName))
                                {
                                    Roles.CreateRole(roleName);
                                }

                                // Dodanie użytkownika do roli
                                Roles.AddUserToRole(model.UserName, roleName);

                                // Utworzenie usługodawcy
                                var provider = new ServiceProvider();

                                if (string.IsNullOrEmpty(model.Name))
                                {
                                    provider.Name = model.UserName;
                                }
                                else
                                {
                                    provider.Name = model.Name;
                                }

                                provider.Email = model.Email;
                                provider.UserId = userId;
                                provider.ZipCode = model.ZipCode;
                                provider.City = model.City;
                                provider.Street = model.Street;
                                provider.PhoneNumber = model.PhoneNumber;
                                provider.RegistrationDate = registrationDate;
                                

                                // Zapisanie usługodawcy w bazie danych
                                _serviceProviderRepo.Add(provider);
                                _serviceProviderRepo.SaveChanges();
                            }
                            else
                            {
                                // Tworzenie usługobiorcy

                                // Utworzenie roli Usługobiorca, jeśli jeszcze nie istnieje
                                string roleName = ConfigurationManager.AppSettings["CustomerRole"];

                                if (!Roles.RoleExists(roleName))
                                {
                                    Roles.CreateRole(roleName);
                                }

                                // Dodanie użytkownika do roli
                                Roles.AddUserToRole(model.UserName, roleName);

                                // Utworzenie usługobiorcy
                                var customer = new Customer();
                                customer.LastName = model.LastName;
                                customer.FirstName = model.FirstName;
                                customer.Email = model.Email;
                                customer.UserId = userId;
                                customer.ZipCode = model.ZipCode;
                                customer.City = model.City;
                                customer.Street = model.Street;
                                customer.RegistrationDate = registrationDate;
                               

                                // Zapisanie usługobiorcy w bazie danych
                                _customerRepo.Add(customer);
                                _customerRepo.SaveChanges();
                            }


                            // Wygenerowanie linku potwierdzającego rejestrację konta w portalu przy użyciu klasy AccountHelper
                            var confirmedUrl = string.Format("http://{0}{1}{2}",
                            Request.Url.Host, (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port), Url.RouteUrl(new { controller = "Account", action = "Confirm", name = model.UserName, securityCode = Konto.CalculateConfirmationCode(model.UserName, model.Email, registrationDate) }));

                            // Wygenerowanie treści e-maila, który zostanie wysłany do użytkownika z linkiem potwierdzającym rejestrację
                            var mailContent =
                                string.Format(
                                    "Witaj!\n\nUtworzyłeś nowe konto w serwisie Portal Usług.\nAdres e-mail: {0}\nData utworzenia: {1}\n\nLink potwierdzający założenie konta: {2}",
                                    model.Email, DateTime.Now, confirmedUrl);

                            // Wysłanie e-maila przy użyciu klasy MailHelper
                            Mail.SendEmail(model.Email, model.UserName, "Potwierdzenie konta", mailContent);

                            TempData["Message"] = "Użytkownik został zarejestrowany. Potwierdź swoje konto linkiem w e-mailu.";
                            return RedirectToAction("Index", "Home");

                        }
                        catch
                        {
                            // W razie niepowodzenia należy usunąć utworzone konto użytkownika
                            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(model.UserName);
                            TempData["Error"] = "Cos poszło nie tak!";
                            return View();
                        }
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }

                return View(model);
            }
            else
            {
                ModelState.AddModelError("Captcha", "Wpisany wynik jest niepoprawny.");
                return View(model);
            }
            

           
        }

        

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
             ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Twoje hasło zostało zmienione." : "";
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {

            ViewBag.ReturnUrl = Url.Action("Manage");
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "Stare lub nowe hasło jest niepoprawne.");
                }
            }

            return View(model);
        }

         [AllowAnonymous]
         public ActionResult CaptchaImage(string prefix, bool noisy = true)
         {
             var rand = new Random((int)DateTime.Now.Ticks);

             // Generowanie nowego działania:
             // Wylosowanie dwóch liczb
             int a = rand.Next(0, 9);
             int b = rand.Next(0, 9);
             string eq = "+-";

             // Wylosowanie operacji (dodawanie lub odejmowanie)
             int oper = rand.Next(0, 2);
             string op = eq[oper].ToString();

             // Utworzenie działania arytmetycznego
             var captcha = string.Format("{0} {2} {1} = ?", a, b, op);

             // Zapisanie w sesji wyniku działania arytmetycznego
             switch (oper)
             {
                 case 0:
                     Session["Captcha" + prefix] = a + b;
                     break;
                 case 1:
                     Session["Captcha" + prefix] = a - b;
                     break;
             

             }

             // Generowanie obrazka
             FileContentResult img = null;
             using (var mem = new MemoryStream())
             using (var bmp = new Bitmap(130, 30))
             using (var gfx = Graphics.FromImage((Image)bmp))
             {
                 gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                 gfx.SmoothingMode = SmoothingMode.AntiAlias;
                 gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                 // Generowanie szumów i nakładanie ich na obrazek
                 if (noisy)
                 {
                     int i, r, x, y;
                     var pen = new Pen(Color.Yellow);

                     for (i = 1; i < 10; i++)
                     {
                         pen.Color = Color.FromArgb(
                          (rand.Next(0, 255)),
                          (rand.Next(0, 255)),
                          (rand.Next(0, 255)));

                         r = rand.Next(0, (130 / 3));
                         x = rand.Next(0, 130);
                         y = rand.Next(0, 30);

                         gfx.DrawEllipse(pen, x - r, y - r, r, r);
                     }
                 }

                 // Rysowanie działania arytmetycznego
                 gfx.DrawString(captcha, new Font("Tahoma", 16), Brushes.Gray, 2, 3);

                 // Renderowanie obrazka do formatu JPG
                 bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                 img = this.File(mem.GetBuffer(), "image/Jpeg");
             }

             return img;
         }
       

        #region Pomocnicy
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
           
        }

       

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Na stronie http://go.microsoft.com/fwlink/?LinkID=177550 znajduje się
            // pełna lista kodów stanu.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Nazwa użytkownika już istnieje. Wprowadź inną nazwę użytkownika.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Nazwa użytkownika dla tego adresu e-mail już istnieje. Wprowadź inny adres e-mail.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Podane hasło jest nieprawidłowe. Wprowadź prawidłową wartość hasła.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Podany adres e-mail jest nieprawidłowy. Sprawdź wartość i spróbuj ponownie.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Podana odpowiedź dla funkcji odzyskiwania hasła jest nieprawidłowa. Sprawdź wartość i spróbuj ponownie.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Podane pytanie dla funkcji odzyskiwania hasła jest nieprawidłowe. Sprawdź wartość i spróbuj ponownie.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Podana nazwa użytkownika jest nieprawidłowa. Sprawdź wartość i spróbuj ponownie.";

                case MembershipCreateStatus.ProviderError:
                    return "Dostawca uwierzytelniania zwrócił błąd. Sprawdź wpis i spróbuj ponownie. Jeśli problem nie zniknie, skontaktuj się z administratorem systemu.";

                case MembershipCreateStatus.UserRejected:
                    return "Żądanie utworzenia użytkownika zostało anulowane. Sprawdź wpis i spróbuj ponownie. Jeśli problem nie zniknie, skontaktuj się z administratorem systemu.";

                default:
                    return "Wystąpił nieznany błąd. Sprawdź wpis i spróbuj ponownie. Jeśli problem nie zniknie, skontaktuj się z administratorem systemu.";
            }
        }
        #endregion
    }
}
