using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Katalog.Models;
using MvcContrib.Pagination;
using PagedList;

namespace Katalog.ViewModels
{
    public class CompanyDetailsViewModel
    {
        public CompanyViewModel Company { get; set; }
        public IPagination<CommentViewModel> CommentPagedList { get; set; }
        //public PagedList<Opinion> Opinions { get; set; }
        public IPagination<Opinion> Opinions { get; set; }
        public bool ConfirmedUser { get; set; }
    }
}