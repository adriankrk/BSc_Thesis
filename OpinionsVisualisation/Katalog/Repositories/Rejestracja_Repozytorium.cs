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
    /// Klasa repozytorium użytkownika.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private KatalogContext database;

        /// <summary>
        /// Konstruktor repozytorium użytkownika.
        /// </summary>
        public CustomerRepository()
        {
            database = new KatalogContext();
        }

        /// <summary>
        /// Pobranie użytkownika o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Użytkownik o podanym identyfikatorze.</returns>
        public Customer GetCustomerById(int id)
        {
            return database.Customers.Find(id);
        }

        /// <summary>
        /// Pobranie użytkownika o podanym identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Użytkownik o podanym identyfikatorze .</returns>
        public Customer GetCustomerByUserId(int id)
        {
            return database.Customers.FirstOrDefault(customer => customer.UserId == id);
        }

      

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze jest zwykłym użytkownikiem.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze jest zwykłym użytkownikiem.</returns>
        public bool IsCustomer(int id)
        {
            return database.Customers.FirstOrDefault(u => u.UserId == id) != null ? true : false;
        }

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze ma potwierdzone konto.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze ma potwierdzone konto.</returns>
        public bool IsConfirmed(int id)
        {
            return database.Customers.FirstOrDefault(u => u.UserId == id).IsConfirmed;
        }

        /// <summary>
        /// Dodanie użytkownika.
        /// </summary>
        /// <param name="customer">Dodawany użytkownik.</param>
        public void Add(Customer customer)
        {
            database.Customers.Add(customer);
        }

        /// <summary>
        /// Usunięcie użytkownika.
        /// </summary>
        /// <param name="customer">Usuwany użytkownik.</param>
        public void Delete(Customer customer)
        {
            string name = ((SimpleMembershipProvider)Membership.Provider).GetUserNameFromId(customer.UserId);
            database.Customers.Remove(customer);
            database.SaveChanges();

            Roles.RemoveUserFromRole(name, "Customer");
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(name);
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(name, true);
        }

        /// <summary>
        /// Zapisanie zmian.
        /// </summary>
        public void SaveChanges()
        {
            database.SaveChanges();
        }

        /// <summary>
        /// Pobranie wszystkich użytkowników.
        /// </summary>
        /// <returns>Użytkownicy.</returns>
        public IQueryable<CustomerViewModel> GetAllCustomers()
        {
            var customers = from p in database.Customers
                            select new CustomerViewModel
                            {
                                Id = p.Id,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                City = p.City,
                                Street = p.Street,
                                ZipCode = p.ZipCode,
                                UserId = p.UserId,
                                IsActive = p.IsConfirmed ? "Tak" : "Nie",
                                IsConfirmed = p.IsConfirmed,
                                RegistrationDate = p.RegistrationDate
                            };

            return customers;
        }
    }
}