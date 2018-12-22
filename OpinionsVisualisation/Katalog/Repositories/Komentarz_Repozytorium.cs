using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Katalog.Models;
using Katalog.ViewModels;

namespace Katalog.Repositories
{
    /// <summary>
    /// Klasa repozytorium komentarza.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private KatalogContext _db;

        /// <summary>
        /// Konstruktor repozytorium komentarza.
        /// </summary>
        public CommentRepository()
        {
            _db = new KatalogContext();
        }

        /// <summary>
        /// Pobranie komentarza o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator komentarza.</param>
        /// <returns>Komentarz o podanym identyfikatorze.</returns>
        public Comment GetCommentById(int id)
        {
            return _db.Comments.FirstOrDefault(comment => comment.Id == id);
        }

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze dodał komentarz/-e.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik dodał komentarz/-e.</returns>
        public bool HasUserComment(int userId)
        {
            return _db.Comments.Where(comment => comment.UserId == userId).Count() > 0;
        }

        /// <summary>
        /// Dodanie komentarza.
        /// </summary>
        /// <param name="comment">Dodawany komentarz.</param>
        public void Add(Comment comment)
        {
            _db.Comments.Add(comment);
        }

        /// <summary>
        /// Usunięcie komentarza.
        /// </summary>
        /// <param name="comment">Usuwany komentarz.</param>
        public void Delete(Comment comment)
        {
            _db.Comments.Remove(comment);
        }

        /// <summary>
        /// Zapisanie zmian.
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Pobranie komentarzy do firmy o podanym identyfikatorze firmy.
        /// </summary>
        /// <param name="serviceId">Identyfikator firmy.</param>
        /// <returns>Komentarze usługi o podanym identyfikatorze firmy.</returns>
        public IQueryable<CommentViewModel> GetCommentByServiceId(int serviceId)
        {
            var comments = from p in _db.Comments.Where(k => k.ServiceId == serviceId)
                           select new CommentViewModel
                           {
                               IPAddress = p.IPAddress,
                               Date = p.Date,
                               ServiceId = p.ServiceId,
                               Id = p.Id,
                               UserId = p.UserId,
                               Content = p.Content,
                               CommentCategory = p.CommentCategory.Name,
                               CommentCategoryId = p.CommentCategoryId,
                               User = (_db.ServiceProviders.FirstOrDefault(u => u.UserId == p.UserId) != null) ?
                                 _db.ServiceProviders.FirstOrDefault(u => u.UserId == p.UserId).Name :
                                 ((_db.Customers.FirstOrDefault(u => u.UserId == p.UserId) != null) ?
                                     (_db.Customers.FirstOrDefault(u => u.UserId == p.UserId)).FirstName + " " + (_db.Customers.FirstOrDefault(u => u.UserId == p.UserId)).LastName :
                                     "Administrator")
                           };
            return comments;
        }
    }
}