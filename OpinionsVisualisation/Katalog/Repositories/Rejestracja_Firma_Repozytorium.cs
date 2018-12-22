using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Katalog.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace Katalog.Repositories
{
    /// <summary>
    ///  Klasa repozytorium użytkownika.
    /// </summary>
    public class ServiceProviderRepository : IServiceProviderRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private KatalogContext _db;

        /// <summary>
        /// Konstruktor repozytorium użytkownika.
        /// </summary>
        public ServiceProviderRepository()
        {
            _db = new KatalogContext();
        }

        /// <summary>
        /// Pobranie użytkownika o podanym identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Użytkownik o podanym identyfikatorze</returns>
        public ServiceProvider GetServiceProviderById(int id)
        {
            return _db.ServiceProviders.FirstOrDefault(provider => provider.Id == id);
        }

        /// <summary>
        /// Pobranie użytkownika o podanym identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Użytkownik o podanym identyfikatorze.</returns>
        public ServiceProvider GetServiceProviderByUserId(int id)
        {
            return _db.ServiceProviders.FirstOrDefault(provider => provider.UserId == id);
        }

        /// <summary>
        /// Pobieranie użytkowników z potwierdzonym kontem.
        /// </summary>
        /// <returns>Użytkownicy z potwierdzonym kontem.</returns>
        public IQueryable<ServiceProvider> GetServiceProviders()
        {
            return _db.ServiceProviders.Where(u => u.IsConfirmed);
        }

        

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze jest użytkownikiem dodającym wpisy.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze jest użytkownikiem dodającym wpisy.</returns>
        public bool IsServiceProvider(int id)
        {
            return _db.ServiceProviders.FirstOrDefault(u => u.UserId == id) != null ? true : false;
        }

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze ma potwierdzone konto.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze ma potwierdzone konto.</returns>
        public bool IsConfirmed(int id)
        {
            return _db.ServiceProviders.FirstOrDefault(u => u.UserId == id).IsConfirmed;
        }

        /// <summary>
        /// Dodanie użytkownika.
        /// </summary>
        /// <param name="provider">Dodawany użytkownika.</param>
        public void Add(ServiceProvider provider)
        {
            _db.ServiceProviders.Add(provider);
        }

        /// <summary>
        /// Usunięcie użytkownika.
        /// </summary>
        /// <param name="provider">Usuwany użytkownik.</param>
        public void Delete(ServiceProvider provider)
        {
            string name = ((SimpleMembershipProvider)Membership.Provider).GetUserNameFromId(provider.UserId);
            _db.ServiceProviders.Remove(provider);
            _db.SaveChanges();

            Roles.RemoveUserFromRole(name, "ServiceProvider");
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(name);
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(name, true);
        }

        /// <summary>
        /// Zapisanie zmian.
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

         /// <summary>
        /// Pobranie wszystkich użytkowników.
        /// </summary>
        /// <returns>Użytkownicy.</returns>
        public IQueryable<ServiceProviderViewModel> GetAllServiceProviders()
        {
            var providers = from p in _db.ServiceProviders
                            select new ServiceProviderViewModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                City = p.City,
                                Street = p.Street,
                                ZipCode = p.ZipCode,
                                UserId = p.UserId,
                                PhoneNumber = p.PhoneNumber,
                                IsActive = p.IsConfirmed ? "Tak" : "Nie",
                                IsConfirmed = p.IsConfirmed,
                                RegistrationDate = p.RegistrationDate
                            };

            return providers;
        }
    
    }
}