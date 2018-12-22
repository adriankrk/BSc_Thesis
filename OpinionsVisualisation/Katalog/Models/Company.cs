using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Katalog.Models
{  
    public partial class Company
    {
        public Company()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WebPage { get; set; }
        public int CategoryId { get; set; }

        public string AbsolventWebPage { get; set; }
        public string GoldenLineWebPage { get; set; }
        public string GoworkWebPage { get; set; }

        public int UserId { get; set; }
        public string IPAddress { get; set; }
        public DateTime PostedDate { get; set; }          
        
        public virtual Category Categories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }   
        public virtual ICollection<Opinion> Opinions { get; set; }
    }

}