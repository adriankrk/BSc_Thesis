using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Katalog.Models;
using Katalog.ViewModels;

namespace Katalog.Repositories
{  
    public interface ICompanyRepository : IRepository<Company>
    {
        /// <summary>
        /// Get company by given ID
        /// </summary>
        /// <param name="id">Company ID</param>
        /// <returns>Company of given ID</returns>
        Company GetCompanyById(int id);

        /// <summary>
        /// Get company view model by given ID
        /// </summary>
        /// <param name="id">Company ID</param>
        /// <returns>Company of given ID</returns>
        CompanyViewModel GetCompaniesViewModelById(int id);

        /// <summary>
        /// Get all companies in database
        /// </summary>
        /// <returns>All companies</returns>
        IQueryable<CompanyViewModel> GetAllCompanies();

        /// <summary>
        /// Get new companies
        /// </summary>
        /// <returns>10 newest entries</returns>
        IQueryable<CompanyViewModel> GetNewCompanies();

        /// <summary>
        /// Get all companies added by given user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Companies added by given user</returns>
        IQueryable<CompanyViewModel> GetCompaniesByUserId(int userId);

        /// <summary>
        /// Checking if user of given ID added entry to database
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>True, if user added company to database</returns>
        bool HasUserCompanies(int userId);
    }
}
