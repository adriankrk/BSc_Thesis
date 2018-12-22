using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Katalog.Rejestracja
{
    public class Konto
    {
        /// <summary>
        /// Wyliczenie kodu rejestracyjnego. 
        /// </summary>
        /// <param name="name">Nazwa użytkownika.</param>
        /// <param name="email">E-mail użytkownika.</param>
        /// <param name="registrationDate">Data rejestracji.</param>
        /// <returns>Wygenerowany kod rejestracyjny</returns>
        public static string CalculateConfirmationCode(string name, string email, DateTime registrationDate)
        {
            // Wykorzystanie funkcji skrótu SHA-1.
            var sha1 = SHA1.Create();

            // Wyliczenie kodu rejestracyjnego na podstawie adresu e-mail, nazwy użytkownika oraz daty utworzenia konta.
            var dateInDatebase = email.Substring(0, 3) + name + registrationDate.ToString();
            byte[] hashDateInDatebaseBytes = sha1.ComputeHash(Encoding.Default.GetBytes(dateInDatebase));
            return BitConverter.ToString(hashDateInDatebaseBytes).Replace("-", "");
        }
    }
}