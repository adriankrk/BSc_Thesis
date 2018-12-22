using Katalog.Models;
using Katalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katalog.Repositories
{
    /// <summary>
    /// Opinions repository interface
    /// </summary>
    public interface IOpinionRepository : IRepository<Opinion>
    {
        /// <summary>
        /// Get all opinions of company
        /// </summary>
        /// <param name="companyId">Company ID</param>
        /// <returns>Opinions of company by given ID</returns>
        IQueryable<Opinion> GetOpinionsByCompanyId(int companyId);

        /// <summary>
        /// Remove all opinions by company ID
        /// </summary>
        /// <param name="companyId">Company ID</param>
        void DeleteAllOpinionsByCompanyId(IQueryable<Opinion> opinions);
    }
}
