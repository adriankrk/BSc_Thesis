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

namespace Katalog.Controllers
{
    public class KomentarzController : Controller
    {
        
        // Repozytorium kategorii.
        private CategoryRepository _categoryRepo;

        // Repozytorium komentarzy.       
        private CommentRepository _commentRepo;

      
        // Konstruktor kontrolera komentarzy.      
        public KomentarzController()
        {
            // Inicjalizacja repozytoriów
            _categoryRepo = new CategoryRepository();
            _commentRepo = new CommentRepository();
        }

        //
        // GET: /Comment/Create

        [Authorize]
        public ActionResult Create(int id)
        {
            // Pobranie listy kategorii komentarzy
            var categoriesList = _categoryRepo.GetAllCommentCategories();
            var categories = new List<SelectListItem>();

            // Wygenerowanie listy w postaci: nazwa - ID kategorii
            foreach (var category in categoriesList)
            {
                categories.Add(new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
            }

            // Umieszczenie listy w obiekcie ViewData
            ViewData["CommentCategories"] = categories;

            // Utworzenie komentarza i ustawienie ID firmy
            var comment = new Comment
            {
                ServiceId = id,
            };

            return View(comment);
        }

        //
        // POST: /Comment/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(int id, FormCollection collection)
        {
            // Utworzenie komentarza
            Comment comment = new Comment();

            if (ModelState.IsValid)
            {
                // Uaktualnienie danych komentarza
                if (TryUpdateModel(comment))
                {
                    // Uzupełnienie danych komentarza o datę dodania, adres IP, ID użytkownika dodającego komentarz oraz ID firmy
                    comment.Date = DateTime.Now;
                    comment.IPAddress = Request.UserHostAddress;
                    comment.UserId = WebSecurity.CurrentUserId;
                    comment.ServiceId = id;

                    // Dodanie komentarza i zapisanie zmian w bazie danych
                    _commentRepo.Add(comment);
                    _commentRepo.SaveChanges();

                    return RedirectToAction("Details", "Company", new { id = comment.ServiceId });
                }
            }

            TempData["Error"] = "Wystąpił błąd podczas dodawania komentarza!";
            return View();
        }

        //
        // GET: /Comment/Edit/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            // Pobranie komentarza po identyfikatorze
            var comment = _commentRepo.GetCommentById(id);

            if (comment != null)
            {
                return View(comment);
            }
            else
            {
                TempData["Error"] = "Nie ma takiego komentarza.";
                return RedirectToAction("Index", "Company");
            }
        }

        //
        // POST: /Comment/Edit/5

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            // Pobranie komentarza po identyfikatorze
            var comment = _commentRepo.GetCommentById(id);

            // Uaktualnienie danych komentarza
            if (TryUpdateModel(comment))
            {
                // Zapisanie zmian w bazie danych
                _commentRepo.SaveChanges();
                TempData["Message"] = "Pomyślnie zmodyfikowano komentarz!";

                return RedirectToAction("Details", "Company", new { id = comment.ServiceId });
            }
            else
            {
                TempData["Error"] = "Wystąpił błąd podczas edycji komentarza!";
                return View(comment);
            }
        }

        //
        // GET: /Comment/Delete/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            // Pobranie komentarza po identyfikatorze
            var comment = _commentRepo.GetCommentById(id);

            if (comment != null)
            {
                try
                {
                    // Usunięcie komentarza i zapisanie zmian w bazie danych
                    _commentRepo.Delete(comment);
                    _commentRepo.SaveChanges();
                    TempData["Message"] = "Komentarz został usunięty.";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Nie można usunąć tego komentarza.";
                }

                return RedirectToAction("Details", "Company", new { id = comment.ServiceId });
            }
            else
            {
                TempData["Error"] = "Nie ma takiego komentarza.";
                return RedirectToAction("Index", "Company");
            }
        }
    }
}
