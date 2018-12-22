using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Katalog.Models;

namespace Katalog.Repositories
{
    /// <summary>
    /// Interfejs repozytorium kategorii.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Pobranie wszystkich kategorii firm.
        /// </summary>
        /// <returns>Kategorie firm.</returns>
        IQueryable<Category> GetAllCategories();

        /// <summary>
        /// Pobranie kategorii firmy po identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator kategorii firm.</param>
        /// <returns>Kategoria firm o podanym identyfikatorze.</returns>
        Category GetCategoryById(int id);

        /// <summary>
        /// Pobranie wszystkich kategorii komentarzy.
        /// </summary>
        /// <returns>Kategorie komentarzy.</returns>
        IQueryable<CommentCategory> GetAllCommentCategories();

        /// <summary>
        /// Pobranie kategorii komentarzy po identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator kategorii komentarzy.</param>
        /// <returns>Kategoria komentarzy o podanym identyfikatorze.</returns>
        CommentCategory GetCommentCategoryById(int id);
    }
}
