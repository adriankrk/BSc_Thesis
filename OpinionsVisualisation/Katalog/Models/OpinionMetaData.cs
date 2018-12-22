using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Katalog.Models
{
    public class OpinionMetaData
    {
        [Display(Name = "Opinie")]
        public string Content { get; set; }

        [Display(Name = "Klasyfikacja")]
        public double Classification { get; set; }
    }

    [MetadataType(typeof(OpinionMetaData))]
    public partial class Opinion
    {

    }
}