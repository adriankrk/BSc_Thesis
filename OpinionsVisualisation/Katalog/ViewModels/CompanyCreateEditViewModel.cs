using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Katalog.Models;

namespace Katalog.ViewModels
{
    public class CompanyCreateEditViewModel
    {
        public Company Company { get; set; }
        public CompanyViewModel CompanyViewModel { get; set; }
    }
}