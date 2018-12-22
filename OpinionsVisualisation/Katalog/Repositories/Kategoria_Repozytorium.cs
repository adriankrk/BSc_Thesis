using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Katalog.Models;

namespace Katalog.Repositories
{
    /// <summary>
    /// Klasa repozytorium kategorii.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private KatalogContext _db;

        /// <summary>
        /// Konstruktor repozytorium kategorii.
        /// </summary>
        public CategoryRepository()
        {
            _db = new KatalogContext();
        }

        #region Kategorie firm

        /// <summary>
        /// Pobranie wszystkich kategorii firm.
        /// </summary>
        /// <returns>Kategorie firm.</returns>
        public IQueryable<Category> GetAllCategories()
        {
            return _db.Categories;
        }

        /// <summary>
        /// Pobranie kategorii firmy po identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator kategorii firm.</param>
        /// <returns>Kategoria firm o podanym identyfikatorze.</returns>
        public Category GetCategoryById(int id)
        {
            return _db.Categories.Find(id);
        }

        /// <summary>
        /// Dodanie kategorii.
        /// </summary>
        /// <param name="category">Dodawana kategoria.</param>
        public void Add(Category category)
        {
            _db.Categories.Add(category);
        }

        #endregion

        #region Kategorie komentarzy

        /// <summary>
        /// Pobranie wszystkich kategorii komentarzy.
        /// </summary>
        /// <returns>Kategorie komentarzy.</returns>
        public IQueryable<CommentCategory> GetAllCommentCategories()
        {
            return _db.CommentCategories;
        }

        /// <summary>
        /// Pobranie kategorii komentarzy po identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator kategorii komentarzy.</param>
        /// <returns>Kategoria komentarzy o podanym identyfikatorze.</returns>
        public CommentCategory GetCommentCategoryById(int id)
        {
            return _db.CommentCategories.Find(id);
        }

        /// <summary>
        /// Dodanie kategorii.
        /// </summary>
        /// <param name="category">Dodawana kategoria.</param>
        public void Add(CommentCategory category)
        {
            _db.CommentCategories.Add(category);
        }

        #endregion

        /// <summary>
        /// Zapisanie zmian.
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Usunięcie kategorii.
        /// </summary>
        /// <param name="category">Usuwana kategoria.</param>
        public void Delete(Category category)
        {
            _db.Categories.Remove(category);
        }

        /// <summary>
        /// Usunięcie kategorii komentarza
        /// </summary>
        /// <param name="category">Usuwana kategoria komentarza.</param>
        public void Delete(CommentCategory category)
        {
            _db.CommentCategories.Remove(category);
        }
    }
}