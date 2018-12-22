using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;

namespace Katalog.ViewModels
{
    public class CompanyListContainerViewModel
    {
        public IPagination<CompanyViewModel> CompanyPagedList { get; set; }
        public CompanyFilteredViewModel CompanyFilteredViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }
}