using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katalog.Repositories
{
    /// <summary>
    /// Base interface of repository
    /// </summary>
    /// <typeparam name="T">Type of repository</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Add operation
        /// </summary>
        /// <param name="element">Object to add</param>
        void Add(T element);

        /// <summary>
        /// Removing operation
        /// </summary>
        /// <param name="element">Object to delete</param>
        void Delete(T element);

        /// <summary>
        /// Save changes to database
        /// </summary>
        void SaveChanges();
    }
}
