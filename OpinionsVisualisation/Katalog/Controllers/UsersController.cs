using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Katalog.Repositories;
using Katalog.Models;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using WebMatrix.WebData;

namespace Katalog.Controllers
{
    public class UsersController : Controller
    {

        
        // Repozytorium użytkowników.
        private CustomerRepository _customerRepo;
        private CommentRepository _commentRepo;

       
        /// Konstruktor kontrolera użytkowników.
        public UsersController()
        {
            // Inicjalizacja repozytoriów
            _customerRepo = new CustomerRepository();
            _commentRepo = new CommentRepository();
        }

       

        [Authorize(Roles = "Administrator")]
        public ActionResult Index(string firstName, string lastName, string city, string street, GridSortOptions gridSortOptions, [DefaultValue(1)]int page)
        {
            // Pobranie listy użytkowników
            var customersList = _customerRepo.GetAllCustomers();

            // Ustawienie domyślnej kolumny sortowania
            if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
            {
                gridSortOptions.Column = "Id";
            }

            // Filtrowanie po imieniu
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                customersList = customersList.Where(a => a.FirstName.Contains(firstName));
            }

            // Filtrowanie po nazwisku
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                customersList = customersList.Where(a => a.LastName.Contains(lastName));
            }

            // Filtrowanie po mieście
            if (!string.IsNullOrWhiteSpace(city))
            {
                customersList = customersList.Where(a => a.City.Contains(city));
            }

            // Filtrowanie po ulicy
            if (!string.IsNullOrWhiteSpace(street))
            {
                customersList = customersList.Where(a => a.Street.Contains(street));
            }

            var customerFilterViewModel = new CustomerFilterViewModel();

            // Sortowanie listy użytkowników oraz stronicowanie
            var customerPagedList = customersList.OrderBy(gridSortOptions.Column, gridSortOptions.Direction)
                   .AsPagination(page, 5);

            CustomerListContainerViewModel customerListContainer = new CustomerListContainerViewModel
            {
                CustomerPagedList = customerPagedList,
                CustomerFilterViewModel = customerFilterViewModel,
                GridSortOptions = gridSortOptions
            };

            return View(customerListContainer);
        }

        //
        // GET: /Customer/Edit/5

        [Authorize(Roles = "Administrator, Customer")]
        public ActionResult Edit(int id)
        {
            // Pobranie użytkownika po identyfikatorze
            var customer = _customerRepo.GetCustomerById(id);

            // Sprawdzenie, czy użytkownik próbuje zmienić swoje własne dane
            if (customer.UserId == WebSecurity.CurrentUserId)
            {
                return View(customer);
            }

            // Jeżeli użytkownik próbuje modyfikować cudze dane, to może jest administratorem.
            else if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
            {
                return View(customer);
            }
            else
            {
                TempData["Error"] = "Nie masz uprawnień do edytowania tego użytkownika.";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator, Customer")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            // Pobranie użytkownika po identyfikatorze
            var customer = _customerRepo.GetCustomerById(id);

            // Uaktualnienie modelu o nowe dane
            if (TryUpdateModel(customer))
            {
                // Zapisanie zmian w bazie danych
                _customerRepo.SaveChanges();

                TempData["Message"] = "Pomyślnie zmodyfikowano konto!";

                if (Roles.GetRolesForUser(WebSecurity.CurrentUserName).Contains("Administrator"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(customer);
                }
            }
            else
            {
                TempData["Error"] = "Wystąpił błąd!";
                return View(customer);
            }
        }


        [Authorize(Roles = "Customer")]
        public ActionResult Change()
        {
            // Pobranie aktualnie zalogowanego użytkownika i przekierowanie do akcji Edit
            var customer = _customerRepo.GetCustomerByUserId(WebSecurity.CurrentUserId);
            return RedirectToAction("Edit", new { id = customer.Id });
        }

        //
        // GET: /Customer/Delete/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            // Pobranie użytkownika po identyfikatorze
            var customer = _customerRepo.GetCustomerById(id);

            if (customer != null)
            {
                // Sprawdzenie, czy użytkownik dodał komentarze
                var hasComment = _commentRepo.HasUserComment(customer.UserId);

                if (!hasComment)
                {
                    return View(customer);
                }
                else
                {
                    TempData["Error"] = "Nie można usunąć tego użytkownika.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "Brak użytkownika o podanym id!";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Pobranie użytkownika po identyfikatorze
            var customer = _customerRepo.GetCustomerById(id);

            try
            {
                // Usunięcie użytkownika i zapisanie zmian
                _customerRepo.Delete(customer);
                _customerRepo.SaveChanges();
            }
            catch (Exception)
            {
                TempData["Error"] = "Nie można usunąć tego użytkownika";
                return View(customer);
            }

            TempData["Message"] = "Użytkownik został usunięty.";
            return RedirectToAction("Index");
        }
    }
}
