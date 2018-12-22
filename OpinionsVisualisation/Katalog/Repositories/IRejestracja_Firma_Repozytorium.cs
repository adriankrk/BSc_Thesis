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
    public interface IServiceProviderRepository : IRepository<ServiceProvider>
    {
        /// <summary>
        /// Pobranie usługodawcy o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Usługodawca o podanym identyfikatorze.</returns>
        ServiceProvider GetServiceProviderById(int id);

        /// <summary>
        /// Pobranie użytkownika o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Użytkownik o podanym identyfikatorze</returns>
        ServiceProvider GetServiceProviderByUserId(int id);

        /// <summary>
        /// Pobieranie użytkowników z potwierdzonym kontem.
        /// </summary>
        /// <returns>Użytkownicy z potwierdzonym kontem.</returns>
        IQueryable<ServiceProvider> GetServiceProviders();

       
        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze jest użytkownikiem dodającym wpisy.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze jest użytkownikiem dodającym wpisy.</returns>
        bool IsServiceProvider(int id);

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze ma potwierdzone konto.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze ma potwierdzone konto.</returns>
        bool IsConfirmed(int id);

        /// <summary>
        /// Pobranie wszystkich użytkowników.
        /// </summary>
        /// <returns>Użytkownikcy.</returns>
        IQueryable<ServiceProviderViewModel> GetAllServiceProviders();
        
    }
}
