using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using Katalog.Models;
using Katalog.Repositories;
using WebMatrix.WebData;
using Katalog.ViewModels;

namespace Katalog.Controllers
{
    public class CompanyUserController : Controller
    {
         
        // Repozytorium użytkownika dodającego wpisy.
        private ServiceProviderRepository _providerRepo;

        
        // Repozytorium firm.       
        private CompanyRepository _companyRepo;

        
        // Repozytorium komentarzy.       
        private CommentRepository _commentRepo;

        
        // Konstruktor kontrolera użytkownika.
        public CompanyUserController()
        {
            // Inicjalizacja repozytoriów 
            _providerRepo = new ServiceProviderRepository();
            _companyRepo = new CompanyRepository();
            _commentRepo = new CommentRepository();
        }

        //
        // GET: /ServiceProvider/

        [Authorize]
        public ActionResult Index(string name, string city, string street, GridSortOptions gridSortOptions, [DefaultValue(1)]int page)
        {
            // Pobranie listy użtkowników
            var providersList = _providerRepo.GetAllServiceProviders();

            // Ograniczenie listy użtkowników do potwierdzonych dla użytkowników zalogowanych innych niż administrator
            if (!Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
            {
                providersList = providersList.Where(a => a.IsConfirmed);
            }

            // Ustawienie domyślnej kolumny sortowania
            if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
            {
                gridSortOptions.Column = "Id";
            }

            // Filtrowanie po nazwie
            if (!string.IsNullOrWhiteSpace(name))
            {
                providersList = providersList.Where(a => a.Name.Contains(name));
            }

            // Filtrowanie po mieście
            if (!string.IsNullOrWhiteSpace(city))
            {
                providersList = providersList.Where(a => a.City.Contains(city));
            }

            // Filtrowanie po ulicy
            if (!string.IsNullOrWhiteSpace(street))
            {
                providersList = providersList.Where(a => a.Street.Contains(street));
            }

            var providerFilterViewModel = new ServiceProviderFilterViewModel();

            // Sortowanie listy użtkowników oraz stronicowanie
            var providerPagedList = providersList.OrderBy(gridSortOptions.Column, gridSortOptions.Direction)
                   .AsPagination(page, 5);

            var providerListContainer = new ServiceProviderListContainerViewModel
            {
                ServiceProviderPagedList = providerPagedList,
                ServiceProviderFilterViewModel = providerFilterViewModel,
                GridSortOptions = gridSortOptions
            };

            return View(providerListContainer);
        }

        //
        // GET: /ServiceProvider/Details/5

        [Authorize]
        public ActionResult Details(int id, string name, string content, int? categoryId, GridSortOptions gridSortOptions,
           [DefaultValue(1)] int page)
        {
            // Pobranie użytkownika po identyfikatorze
            var provider = _providerRepo.GetServiceProviderById(id);

            // Pobranie listy firm danego  użytkownika
            var companiesList = _companyRepo.GetCompaniesByUserId(provider.UserId);

            // Ustawienie domyślnej kolumny sortowania
            if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
            {
                gridSortOptions.Column = "Id";
            }

            // Filtrowanie po kategorii firmy
            if (categoryId.HasValue)
            {
                companiesList = companiesList.Where(a => a.CategoryId == categoryId);
            }

            // Filtrowanie po nazwie firmy
            if (!string.IsNullOrWhiteSpace(name))
            {
                companiesList = companiesList.Where(a => a.Name.Contains(name));
            }

            // Filtrowanie po opisie firmy
            if (!string.IsNullOrWhiteSpace(content))
            {
                companiesList = companiesList.Where(a => a.Description.Contains(content));
            }

            // Utworzenie modelu do filtrowania firm
            var companiesFilterViewModel = new CompanyFilteredViewModel();
            companiesFilterViewModel.SelectedCategoryId = categoryId ?? -1;
            companiesFilterViewModel.Fill();

            // Stronicowanie i sortowanie listy firm
            var companiesPagedList = companiesList.OrderBy(gridSortOptions.Column, gridSortOptions.Direction)
                   .AsPagination(page, 10);

            var companiesListContainer = new CompanyListContainerViewModel
            {
                CompanyPagedList = companiesPagedList,
                CompanyFilteredViewModel = companiesFilterViewModel,
                GridSortOptions = gridSortOptions
            };

            var serviceProviderCompaniesListContainer = new ServiceProviderCompaniesListContainerViewModel
            {
                Companies = companiesListContainer,
                ServiceProvider = provider,
            };

            return View(serviceProviderCompaniesListContainer);
        }
        
        [Authorize(Roles = "ServiceProvider")]
        public ActionResult Change()
        {
            // Pobranie aktualnie zalogowanego użytkownika i przekierowanie do akcji Edit
            var provider = _providerRepo.GetServiceProviderByUserId(WebSecurity.CurrentUserId);
            return RedirectToAction("Edit", new { id = provider.Id });
        }

        [Authorize(Roles = "Administrator, ServiceProvider")]
        public ActionResult Edit(int id)
        {
            // Pobranie użytkownika po identyfikatorze
            var provider = _providerRepo.GetServiceProviderById(id);

            // Sprawdzenie, czy użytkownik próbuje zmienić swoje własne dane
            if (provider.UserId == WebSecurity.CurrentUserId)
            {
                return View(provider);
            }
            // Jeżeli użytkownik próbuje modyfikować cudze dane, to może jest administratorem.
            else if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
            {
                return View(provider);
            }
            else
            {
                TempData["Error"] = "Nie masz uprawnień do edytowania tego użytkownika.";
                return View("Index");
            }
        }

        //
        // POST: /ServiceProvider/Edit/5
        [Authorize(Roles = "Administrator, ServiceProvider")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection formValues)
        {
            // Pobranie użytkownika po identyfikatorze
            var provider = _providerRepo.GetServiceProviderById(id);

            // Uaktualnienie modelu o nowe dane
            if (TryUpdateModel(provider))
            {
                // Zapisanie zmian w bazie danych
                _providerRepo.SaveChanges();
                TempData["Message"] = "Pomyślnie zmodyfikowano usługodawcę!";
                return RedirectToAction("Details", new { id = provider.Id });
            }
            else
            {
                TempData["Error"] = "Wystąpił błąd!";
                return View(provider);
            }
        }

        //
        // GET: /ServiceProvider/Delete/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            // Pobranie użytkownika po identyfikatorze
            var provider = _providerRepo.GetServiceProviderById(id);

            if (provider != null)
            {
                // Sprawdzenie, czy użytkownik dodał firmę
                bool hasCompanies = _companyRepo.HasUserCompanies(provider.UserId);

                // Sprawdzenie, czy użytkownik dodał komentarze
                bool hasComments = _commentRepo.HasUserComment(provider.UserId);

                if (hasCompanies || hasComments)
                {
                    // Jeżeli użytkownik dodał firmę lub komentarz nie może zostać usunięty
                    TempData["Error"] = "Nie można usunąć tego użytkownika.";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(provider);
                }
            }
            else
            {
                TempData["Error"] = "Brak usługodawcy o podanym id!";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /ServiceProvider/Delete/5

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Pobranie użytkownika po identyfikatorze
            var provider = _providerRepo.GetServiceProviderById(id);

            try
            {
                // Usunięcie użytkownika i zapisanie zmian
                _providerRepo.Delete(provider);
                _providerRepo.SaveChanges();
            }
            catch (Exception)
            {
                TempData["Error"] = "Nie można usunąć tego użytkownika";
                return View(provider);
            }

            TempData["Message"] = "użytkownik został usunięty.";
            return RedirectToAction("Index");
        }
    
    }
}
