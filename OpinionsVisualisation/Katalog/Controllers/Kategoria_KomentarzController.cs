using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Katalog.Models;
using Katalog.Repositories;

namespace Katalog.Controllers
{
    public class Kategoria_KomentarzController : Controller
    {
        
        // Repozytorium kategorii.      
        private CategoryRepository _categoryRepo;

       
        // Repozytorium komentarzy.       
        private CommentRepository _commentRepo;

      
        // Konstruktor kontrolera kategorii komentarzy.
        public Kategoria_KomentarzController()
        {
            _categoryRepo = new CategoryRepository();
            _commentRepo = new CommentRepository();
        }


        //
        // GET: /CommentCategory/

        public ActionResult Index()
        {
            // Pobranie listy kategorii komentarzy
            var categoriesList = _categoryRepo.GetAllCommentCategories();
            return View(categoriesList);
        }

        //
        // GET: /CommentCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CommentCategory/Create

        [HttpPost]
        public ActionResult Create(CommentCategory commentcategory)
        {
            if (ModelState.IsValid)
            {
                // Dodanie nowej kategorii komentarzy i zapisanie w bazie danych
                if (TryUpdateModel(commentcategory))
                {
                    _categoryRepo.Add(commentcategory);
                    _categoryRepo.SaveChanges();

                    TempData["Message"] = "Pomyślnie dodano kategorię!";
                    return RedirectToAction("Index");
                }
            }

            TempData["Error"] = "Wystąpił błąd podczas dodawania kategorii!";

            return View(commentcategory);
        }

        //
        // GET: /CommentCategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            // Pobranie kategorii komentarzy po identyfikatorze
            var commentcategory = _categoryRepo.GetCommentCategoryById(id);

            if (commentcategory != null)
            {
                return View(commentcategory);
            }
            else
            {
                TempData["Error"] = "Brak kategorii komentarza o podanym ID!";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /CommentCategory/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            // Pobranie kategorii komentarzy z bazy danych
            var categoryFromDataBase = _categoryRepo.GetCommentCategoryById(id);

            // Uaktualnienie modelu o nowe dane
            if (TryUpdateModel(categoryFromDataBase))
            {
                // Zapisanie zmian w bazie danych
                _categoryRepo.SaveChanges();
                TempData["Message"] = "Pomyślnie zmodyfikowano kategorię!";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Wystąpił błąd!";
                return View(categoryFromDataBase);
            }
        }

        //
        // GET: /CommentCategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            // Pobranie kategorii komentarzy po identyfikatorze
            var categoryFromDataBase = _categoryRepo.GetCommentCategoryById(id);

            if (categoryFromDataBase != null)
            {
                return View(categoryFromDataBase);
            }
            else
            {
                TempData["Error"] = "Brak kategorii komentarza o podanym ID!";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Comment/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Pobranie komentarza po identyfikatorze
            var categoryFromDataBase = _categoryRepo.GetCommentCategoryById(id);

            if (categoryFromDataBase != null)
            {
                try
                {
                    // Usunięcie komentarza i zapisanie zmian w bazie danych
                    _categoryRepo.Delete(categoryFromDataBase);            
                    _categoryRepo.SaveChanges();
                    TempData["Message"] = "Komentarz został usunięty.";
                    
                }
                catch (Exception)
                {
                    TempData["Error"] = "Nie można usunąć tego komentarza.";
                    
                }
                

                
            }
            return RedirectToAction("Index");
            
        }
    }
}