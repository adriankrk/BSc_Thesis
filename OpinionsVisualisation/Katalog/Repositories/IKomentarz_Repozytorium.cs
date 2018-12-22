using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Katalog.Models;
using Katalog.ViewModels;

namespace Katalog.Repositories
{
    /// <summary>
    /// Interfejs repozytorium komentarza.
    /// </summary>
    public interface ICommentRepository : IRepository<Comment>
    {
        /// <summary>
        /// Pobranie komentarza o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator komentarza.</param>
        /// <returns>Komentarz o podanym identyfikatorze.</returns>
        Comment GetCommentById(int id);

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze dodał komentarz/-e.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik dodał komentarz/-e.</returns>
        bool HasUserComment(int userId);

        /// <summary>
        /// Pobranie komentarzy do firmy o podanym identyfikatorze firmy.
        /// </summary>
        /// <param name="serviceId">Identyfikator firmy.</param>
        /// <returns>Komentarze usługi o podanym identyfikatorze firmy.</returns>
        IQueryable<CommentViewModel> GetCommentByServiceId(int serviceId);
        
    }
}
