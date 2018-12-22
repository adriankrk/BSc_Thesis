using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Configuration;
using System.Web.Security;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using Katalog.Models;
using Katalog.Repositories;
using WebMatrix.WebData;
using Katalog.ViewModels;
using PagedList;
using static Katalog.WebScraping.OpinionsDownloader;
using System.IO;
using Katalog.WebScraping;

namespace Katalog.Controllers
{
    public class CompanyController : Controller
    {         
        private CompanyRepository _companyRepo;     // Repozytorium firm.
        private CategoryRepository _categoryRepo;   // Repozytorium kategorii.
        private ServiceProviderRepository _providerRepo;  // Repozytorium użytkowników dodających wpisy.
        private CustomerRepository _customerRepo;   // Repozytorium użytkownik.
        private CommentRepository _commentRepo;     // Repozytorium komentarzy.
        private OpinionRepository _opinionRepo;

        // Konstruktor kontrolera firm.
        public CompanyController()
        {
            _companyRepo = new CompanyRepository();
            _categoryRepo = new CategoryRepository();
            _providerRepo = new ServiceProviderRepository();
            _customerRepo = new CustomerRepository();
            _commentRepo = new CommentRepository();
            _opinionRepo = new OpinionRepository();
        }

        //
        // GET: /Company/

        public ActionResult Index(string name, string content, int? serviceProviderId, int? categoryId,  GridSortOptions gridSortOptions, [DefaultValue(1)]int page)
        {
            // Pobranie listy firm
            var companiesList = _companyRepo.GetAllCompanies();
           
            // Ustawienie domyślnej kolumny sortowania
            if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
            {
                gridSortOptions.Column = "Id";
            }

            // Filtrowanie po użytkowniku
            if (serviceProviderId.HasValue)
            {
                companiesList = companiesList.Where(a => a.UserId == serviceProviderId);
            }

            // Filtrowanie po kategorii
            if (categoryId.HasValue)
            {
                companiesList = companiesList.Where(a => a.CategoryId == categoryId);
            }

            // Filtrowanie po nazwie
            if (!string.IsNullOrWhiteSpace(name))
            {
                companiesList = companiesList.Where(a => a.Name.Contains(name));
            }

            // Filtrowanie po treści
            if (!string.IsNullOrWhiteSpace(content))
            {
                companiesList = companiesList.Where(a => a.Description.Contains(content));
            }

            // Przygotowanie modelu do filtrowania, wypełnienie modelu danymi
            var companyFilterViewModel = new CompanyFilteredViewModel();
            companyFilterViewModel.SelectedCategoryId = categoryId ?? -1;
            companyFilterViewModel.SelectedProviderId = serviceProviderId ?? -1;
            companyFilterViewModel.Fill();

            // Sortowanie listy firm oraz stronicowanie
            var companyPagedList =  companiesList.OrderBy(gridSortOptions.Column, gridSortOptions.Direction).AsPagination(page, 10);

            var companyListContainer = new CompanyListContainerViewModel
            {
                CompanyPagedList = companyPagedList,
                CompanyFilteredViewModel = companyFilterViewModel,
                GridSortOptions = gridSortOptions
            };

            return View(companyListContainer);
        }

        
        
        //
        // GET: /Company/Details/5

        public ActionResult Details(int id, [DefaultValue(1)]int page)
        {
            bool isConfirmed = false;

            // Pobranie firmy po identyfikatorze
            var company = _companyRepo.GetCompaniesViewModelById(id);

            // Pobranie listy komentarzy danej firmy
            var comments = _commentRepo.GetCommentByServiceId(id);


            var opinions = _opinionRepo.GetOpinionsByCompanyId(id); 

            
            // Jeżeli użytkownik jest zalogowany i ma potwierdzone konto, to będzie widzieć listę komentarzy.
            var userId = WebSecurity.CurrentUserId;

            // Sprawdzenie, czy użytkownik potwierdził konto
            if (_providerRepo.IsServiceProvider(userId))
            {
                isConfirmed = _providerRepo.IsConfirmed(userId);
            }
            else if (_customerRepo.IsCustomer(userId))
            {
                isConfirmed = _customerRepo.IsConfirmed(userId);
            }

            // Sortowanie listy komentarzy oraz stronicowanie
            var commentsList = comments.OrderBy("Date", SortDirection.Ascending).AsPagination(page, 10);

            var companyDetails = new CompanyDetailsViewModel
            {
                Company = company,
                CommentPagedList = commentsList,
                Opinions = opinions.AsPagination(page, 10),
                ConfirmedUser = isConfirmed,
            };

            return View(companyDetails);
        }

        [Authorize(Roles = "ServiceProvider, Administrator")]
        public ActionResult DownloadOpinions(int id = 0)
        {
            var company = _companyRepo.GetCompanyById(id);

            // Download Data From Internet
            var opinions = (company.AbsolventWebPage != null) ? DownloadOpinionsAbsolvent(company.AbsolventWebPage) : new List<string>();
            opinions = opinions.Concat((company.GoldenLineWebPage != null) ? DownloadOpinionsGoldenLine(company.GoldenLineWebPage) : null).ToList();
            opinions = opinions.Concat((company.GoworkWebPage != null) ? DownloadOpinionsGowork(company.GoworkWebPage) : null).ToList();

            for (int i = 0; i < opinions.Count; i++)
            {
                _opinionRepo.Add(new Opinion { Id = i, Content = opinions[i],
                    Classification = Convert.ToDouble(RepustateClient.Sentiment(opinions[0], "pl").Split(',')[0].Split(':')[1]),
                    CompanyId = company.Id });
            }
            _opinionRepo.SaveChanges();

            return RedirectToAction("Details", new { id = company.Id });
        }

        [Authorize(Roles = "ServiceProvider, Administrator")]
        public ActionResult DeleteOpinions(int id = 0)
        {
            var opinions = _opinionsRepo.GetOpinionsByCompanyId(id);
            _opinionRepo.DeleteAllOpinions();
            return RedirectToAction("Details", new { id = company.Id });
        }

        //
        // GET: /Company/Create

        [Authorize(Roles = "ServiceProvider, Administrator")]
        public ActionResult Create()
        {
            // Pobranie listy kategorii firm i umieszczenie jej w obiekcie ViewData           
            ViewData["Categories"] = GetCategoriesList();
            return View();
        }

        //
        // POST: /Company/Create

        [Authorize(Roles = "ServiceProvider, Administrator")]
        [HttpPost]
        public ActionResult Create(CompanyCreateEditViewModel company)
        {
            if (ModelState.IsValid)
            {
                // Dodanie nowej firmy i zapisanie w bazie danych
                if (TryUpdateModel(company))
                {
                    // Ustawienie daty umieszczenia firmy
                    company.Company.PostedDate = DateTime.Now;                    

                    // Ustawienie użytkownika dodającego firmę oraz adresu IP
                    company.Company.UserId = WebSecurity.CurrentUserId;
                    company.Company.IPAddress = Request.UserHostAddress;
                   
                    company.Company.WebPage = ChangeUrlAddress(company.Company.WebPage);
                    company.Company.AbsolventWebPage = ChangeUrlAddress(company.Company.AbsolventWebPage);
                    company.Company.GoldenLineWebPage = ChangeUrlAddress(company.Company.GoldenLineWebPage);
                    company.Company.GoworkWebPage = ChangeUrlAddress(company.Company.GoworkWebPage);

                    // Dodanie nowej firmy i zapisanie zmian
                    _companyRepo.Add(company.Company);
                    _companyRepo.SaveChanges();

                    return RedirectToAction("Details", new { id = company.Company.Id });
                }

                // Pobranie listy kategorii firm i umieszczenie jej w obiekcie ViewData  
                ViewData["Categories"] = GetCategoriesList();
                return View(company);
            }

            TempData["Error"] = "Wystąpił błąd podczas dodawania wpisu!";
            return RedirectToAction("Index");
        }

        [NonAction]
        private string ChangeUrlAddress(string webpage)
        {
            if (webpage[0] != 'h' && webpage[1] != 't' && webpage[2] != 't' && webpage[3] != 'p')
                return @"http://" + webpage;
            else
                return webpage;

        }

        [NonAction]
        private List<SelectListItem> GetCategoriesList()
        {
            // Pobranie listy kategorii firm
            var categoriesList = _categoryRepo.GetAllCategories();
            var categories = new List<SelectListItem>();

            // Wygenerowanie listy w postaci: nazwa – ID kategorii
            foreach (var category in categoriesList)
            {
                categories.Add(new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
            }

            return categories;
        }

        //
        // GET: /Company/Edit/5

        [Authorize(Roles = "ServiceProvider, Administrator")]
        public ActionResult Edit(int id = 0)
        {
            // Pobranie firmy po identyfikatorze
            var company = _companyRepo.GetCompanyById(id);

            if (company != null)
            {
                // Pobranie listy kategorii firm i umieszczenie jej w obiekcie ViewData              
                ViewData["Categories"] = GetEditCategories(company);

                // Utworzenie modelu widokowego w celu przekazania go do widoku
                CompanyCreateEditViewModel editCompany = new CompanyCreateEditViewModel();
                editCompany.Company = company;
                                
                // Sprawdzenie, czy zalogowany użytkownik może edytować usługę
                    if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("ServiceProvider") || Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
                {
                    if (company.UserId == WebSecurity.CurrentUserId)
                    {
                        return View(editCompany);
                    }
                    else if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
                    {
                        return View(editCompany);
                    }
                    else
                    {
                        TempData["Error"] = "Nie masz uprawnień do edytowania tego wpisu.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Nie masz uprawnień do edytowania tego wpisu.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "Brak wpisu o podanym ID!";
                return RedirectToAction("Index");
            }
        }

        //
         // POST: /Company/Edit/5

        [Authorize(Roles = "ServiceProvider, Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, CompanyCreateEditViewModel editedCompany)
        {
            // Pobranie firmy po identyfikatorze
            var company = _companyRepo.GetCompanyById(id);
            editedCompany.Company = company;
            
            // Uaktualnienie modelu i zapisanie zmian w bazie danych
            if (TryUpdateModel(editedCompany))
            {
                editedCompany.Company.WebPage = ChangeUrlAddress(editedCompany.Company.WebPage);
                editedCompany.Company.AbsolventWebPage = ChangeUrlAddress(editedCompany.Company.AbsolventWebPage);
                editedCompany.Company.GoldenLineWebPage = ChangeUrlAddress(editedCompany.Company.GoldenLineWebPage);
                editedCompany.Company.GoworkWebPage = ChangeUrlAddress(editedCompany.Company.GoworkWebPage);

                _companyRepo.SaveChanges();
                TempData["Message"] = "Pomyślnie zmodyfikowano wpis!";
                return RedirectToAction("Details", new { id = editedCompany.Company.Id });
            }
            else
            {
                // Pobranie listy kategorii firm i umieszczenie jej w obiekcie ViewData
                ViewData["Categories"] = GetEditCategories(editedCompany.Company);
                TempData["Error"] = "Wystąpił błąd podczas edytowania wpisu!";
                return View(editedCompany);
            }
        }

        [NonAction]
        private List<SelectListItem> GetEditCategories(Company company)
        {
            // Pobranie listy kategorii firm
            var categoriesList = _categoryRepo.GetAllCategories();
            var categories = new List<SelectListItem>();

            // Wygenerowanie listy w postaci: nazwa - ID kategorii oraz zaznaczenie wybranej w firmie kategorii
            foreach (var category in categoriesList)
            {
                categories.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                    Selected = category.Id == company.CategoryId

                });
            }
            return categories;
        }

        //
        // GET: /Company/Delete/5

        [Authorize(Roles = "ServiceProvider, Administrator")]
        public ActionResult Delete(int id = 0)
        {
            // Pobranie firmy po identyfikatorze
            var company = _companyRepo.GetCompanyById(id);

            if (company != null)
            {
                // Sprawdzenie, czy do firmy zostały dodane komentarze
                if (company.Comments.Count == 0)
                {
                    // firmę może usunąć jedynie administrator lub użytkownik, który ją dodał
                    if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("ServiceProvider") || Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
                    {
                        if (company.UserId == WebSecurity.CurrentUserId)
                        {
                            return View(company);
                        }
                        else if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
                        {
                            return View(company);
                        }
                        else
                        {
                            TempData["Error"] = "Nie masz uprawnień do usunięcia tego wpisu.";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Nie masz uprawnień do usunięcia tego wpisu.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Nie można usunąć wybranego wpisu.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "Brak wpisu o podanym ID!";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Company/Delete/5

        [Authorize(Roles = "ServiceProvider, Administrator")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Pobranie firmy po identyfikatorze
            var company = _companyRepo.GetCompanyById(id);

            try
            {
                // Usunięcie firmy i zapisanie zmian w bazie danych
                _companyRepo.Delete(company);
                _companyRepo.SaveChanges();
            }
            catch (Exception)
            {
                TempData["Error"] = "Nie można usunąć tego wpisu.";
                return View(company);
            }

            TempData["Message"] = "Wpis został usunięty.";
            return RedirectToAction("Index");
        }
    }
}
