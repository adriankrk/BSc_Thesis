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
    [Authorize(Roles = "Administrator")]
    public class KategoriaController : Controller
    {
      
        // Repozytorium kategorii.    
        private CategoryRepository _categoryRepo;

        
        // Konstruktor kontrolera kategorii firm.
        public KategoriaController()
        {
            // Inicjalizacja repozytorium
            _categoryRepo = new CategoryRepository();
        }

        //
        // GET: /Category/

        public ActionResult Index()
        {
            // Pobranie listy kategorii firm
            var categoriesList = _categoryRepo.GetAllCategories();
            return View(categoriesList);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // Dodanie nowej kategorii firm i zapisanie w bazie danych
                if (TryUpdateModel(category))
                {
                    _categoryRepo.Add(category);
                    _categoryRepo.SaveChanges();

                    TempData["Message"] = "Pomyślnie dodano kategorię!";
                    return RedirectToAction("Index");
                }
            }

            TempData["Error"] = "Wystąpił błąd podczas dodawania kategorii!";

            return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            // Pobranie kategorii firm po identyfikatorze
            var category = _categoryRepo.GetCategoryById(id);

            if (category != null)
            {
                return View(category);
            }
            else
            {
                TempData["Error"] = "Brak kategorii o podanym ID!";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            /// Pobranie kategorii firm z bazy danych
            var categoryFromDataBase = _categoryRepo.GetCategoryById(id);

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
        // GET: /Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            // Pobranie kategorii komentarzy po identyfikatorze
            var categoryFromDataBase = _categoryRepo.GetCategoryById(id);

            if (categoryFromDataBase != null)
            {
                return View(categoryFromDataBase);
            }
            else
            {
                TempData["Error"] = "Brak kategorii o podanym ID!";
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Category/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Pobranie kategorii po identyfikatorze
            var categoryFromDataBase = _categoryRepo.GetCategoryById(id);

            if (categoryFromDataBase != null)
            {
                try
                {
                    // Usunięcie kategorii i zapisanie zmian w bazie danych
                    _categoryRepo.Delete(categoryFromDataBase);
                    _categoryRepo.SaveChanges();
                    TempData["Message"] = "Kategoria został usunięty.";

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