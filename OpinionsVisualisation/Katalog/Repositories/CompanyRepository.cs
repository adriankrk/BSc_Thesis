using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Katalog.Models;
using Katalog.ViewModels;

namespace Katalog.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {       
        private KatalogContext _db;
             
        public CompanyRepository()
        {
            _db = new KatalogContext();
        }

        public void Add(Company company)
        {
            _db.Companies.Add(company);
        }

        public void Delete(Company company)
        {
            _db.Companies.Remove(company);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public Company GetCompanyById(int id)
        {
            return _db.Companies.FirstOrDefault(company => company.Id == id);
        }

        public bool HasUserCompanies(int userId)
        {
            return _db.Companies.Where(u => u.UserId == userId).Count() > 0;
        }

        /// <summary>
        /// Pobranie wszystkich firm.
        /// </summary>
        /// <returns>Wszystkie firmy.</returns>
        public IQueryable<CompanyViewModel> GetAllCompanies()
        {
            var companies = from p in _db.Companies.OrderByDescending(o => o.PostedDate)
                           select new CompanyViewModel
                           {
                               AddressIP = p.IPAddress,
                               PostedDate = p.PostedDate,
                               Address = p.Address,
                               Phone = p.Phone,
                               WebPage = p.WebPage,
                               AbsolventWebPage = p.AbsolventWebPage,
                               GoldenLineWebPage = p.GoldenLineWebPage,
                               GoworkWebPage = p.GoworkWebPage,
                               Id = p.Id,
                               CategoryId = p.CategoryId,
                               UserId = p.UserId,
                               Description = p.Description,
                               Name = p.Name,
                               CategoryName = p.Categories.Name,
                               UserName = _db.ServiceProviders.FirstOrDefault(u => u.UserId == p.UserId).Name,
                           };

            return companies;
        }

        /// <summary>
        /// Pobranie firmy o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator firmy.</param>
        /// <returns>Firma o podanym identyfikatorze.</returns>
        public CompanyViewModel GetCompaniesViewModelById(int id)
        {
            var company = from p in _db.Companies
                          where p.Id == id
                          select new CompanyViewModel
                          {
                              Id = p.Id,
                              PostedDate = p.PostedDate,
                              Address = p.Address,
                              Phone = p.Phone,
                              WebPage = p.WebPage,
                              AbsolventWebPage = p.AbsolventWebPage,
                              GoldenLineWebPage = p.GoldenLineWebPage,
                              GoworkWebPage = p.GoworkWebPage,
                              CategoryId = p.CategoryId,
                              UserId = p.UserId,
                              Description = p.Description,
                              Name = p.Name,
                              CategoryName = p.Categories.Name,
                              UserName = _db.ServiceProviders.FirstOrDefault(u => u.UserId == p.UserId).Name,
                          };

            return company.FirstOrDefault();
        }

        /// <summary>
        /// Pobieranie firm o podanym identyfikatorze użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>Aktywne usługi użytkownika o podanym identyfikatorze użytkownika.</returns>
        public IQueryable<CompanyViewModel> GetCompaniesByUserId(int userId)
        {
            var companies = from p in _db.Companies
                            where p.UserId == userId
                            select new CompanyViewModel
                           {
                               AddressIP = p.IPAddress,
                               PostedDate = p.PostedDate,
                               Address = p.Address,
                               Phone = p.Phone,
                               WebPage = p.WebPage,
                               AbsolventWebPage = p.AbsolventWebPage,
                               GoldenLineWebPage = p.GoldenLineWebPage,
                               GoworkWebPage = p.GoworkWebPage,
                               Id = p.Id,
                               CategoryId = p.CategoryId,
                               UserId = p.UserId,
                               Description = p.Description,
                               Name = p.Name,
                               CategoryName = p.Categories.Name,                             
                               UserName = _db.ServiceProviders.FirstOrDefault(u => u.UserId == p.UserId).Name,
                           };
            return companies;
        }

        
       // Pobranie nowych wpisów.
   
        public IQueryable<CompanyViewModel> GetNewCompanies()
        {
            var companies = from p in _db.Companies
                           
                           orderby p.PostedDate descending
                           select new CompanyViewModel
                           {
                               PostedDate = p.PostedDate,
                               Id = p.Id,
                               UserId = p.UserId,
                               Name = p.Name,
                               Address = p.Address,
                               Phone = p.Phone,
                               WebPage = p.WebPage,
                               AbsolventWebPage = p.AbsolventWebPage,
                               GoldenLineWebPage = p.GoldenLineWebPage,
                               GoworkWebPage = p.GoworkWebPage,
                               UserName = _db.ServiceProviders.FirstOrDefault(u => u.UserId == p.UserId).Name
                           };

            return companies.Take(10); //  10 nowych wpisów
        }
    }
}