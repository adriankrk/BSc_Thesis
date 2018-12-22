using Katalog.Models;
using Katalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Katalog.Repositories
{
    /// <summary>
    /// Opinions repository class
    /// </summary>
    public class OpinionRepository : IOpinionRepository
    {
        private KatalogContext _db;

        public OpinionRepository()
        {
            _db = new KatalogContext();
        }

        /// <summary>
        /// Add opinion
        /// </summary>
        /// <param name="opinion">Opinion to add</param>
        public void Add(Opinion opinion)
        {
            _db.Opinions.Add(opinion);
        }

        /// <summary>
        /// Remove opinion
        /// </summary>
        /// <param name="opinion">Opinion to remove</param>
        public void Delete(Opinion opinion)
        {
            _db.Opinions.Remove(opinion);
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Get all opinions of company
        /// </summary>
        /// <param name="companyId">Company ID</param>
        /// <returns>Opinions of company by given ID</returns>
        public IQueryable<Opinion> GetOpinionsByCompanyId(int companyId)
        {
            //var opinions = from p in _db.Opinions.Where(x => x.Companies.Id == companyId)
            //               select new Opinion
            //               {
            //                   Id = p.Id,
            //                   Content = p.Content,
            //                   Classification = p.Classification,
            //               };

            return _db.Opinions.Where(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// Remove all opinions by company ID
        /// </summary>
        /// <param name="companyId">Company ID</param>
        public void DeleteAllOpinions(IQueryable<Opinion> opinions)
        {
            foreach (var entity in opinions)
            {
                _db.Opinions.Remove(entity);
            }
        }
    }
}