using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Katalog.Models
{
    public partial class Opinion
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public double Classification { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Companies { get; set; }
    }
}