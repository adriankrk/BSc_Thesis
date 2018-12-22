using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Katalog.Models
{
    [MetadataType(typeof(CategoryMetaData))]
    public class Category
    {
        public Category()
        {
            Companies = new HashSet<Company>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
    }

    public class CategoryMetaData
    {
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
    }
}