using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Katalog.Models;

namespace Katalog.Repositories
{
    /// <summary>
    /// Interfejs repozytorium użytkownika.
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Pobranie użytkownika o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Użytkownik o podanym identyfikatorze.</returns>
        Customer GetCustomerById(int id);

        /// <summary>
        /// Pobranie użytkownika o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns> Użytkownik o podanym identyfikatorze.</returns>
        Customer GetCustomerByUserId(int id);


        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze użytkownika jest zwykłym użytkownikiem.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze jest zwykłym użytkownikiem.</returns>
        bool IsCustomer(int id);

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze użytkownika ma potwierdzone konto.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze ma potwierdzone konto.</returns>
        bool IsConfirmed(int id);

        /// <summary>
        /// Pobranie wszystkich użytkowników.
        /// </summary>
        /// <returns>Użytkownicy.</returns>
        IQueryable<CustomerViewModel> GetAllCustomers();
      
    }
}
