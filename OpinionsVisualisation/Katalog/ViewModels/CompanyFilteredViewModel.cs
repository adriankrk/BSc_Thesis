using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Katalog.Repositories;

namespace Katalog.ViewModels
{
    public class CompanyFilteredViewModel
    {
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }

        public int SelectedCategoryId { get; set; }
        public int SelectedProviderId { get; set; }

        public void Fill()
        {
            CategoryRepository _categoryRepo = new CategoryRepository();                // Repozytorium kategorii firm.
            ServiceProviderRepository _providerRepo = new ServiceProviderRepository();  // Repozytorium uzytkowników.

            // Wypełnienie listy kategorii firm danymi w postaci: nazwa kategorii - ID kategorii
            Categories = _categoryRepo.GetAllCategories()
                                      .Select(a => new { a.Name, a.Id })
                                      .ToList()
                                      .Select(a => new SelectListItem
                                      {
                                          Text = a.Name,
                                          Value = a.Id.ToString(),
                                          Selected = a.Id == SelectedCategoryId
                                      })
                                      .ToList();

            // Wypełnienie listy użytkowników danymi w postaci: nazwa użytkownika - ID użytkownika
            Users = _providerRepo.GetServiceProviders()
                                 .Select(a => new { a.Name, a.UserId })
                                 .ToList()
                                 .Select(a => new SelectListItem
                                 {
                                     Text = a.Name,
                                     Value = Convert.ToString(a.UserId),
                                     Selected = a.UserId == SelectedProviderId
                                 })
                                 .ToList();
        }
    }
}