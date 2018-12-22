using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Katalog.Models
{
    public class KatalogContext : DbContext
    {
        public KatalogContext()
            : base("Katalog_Baza_danych")
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentCategory> CommentCategories { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
    }
}