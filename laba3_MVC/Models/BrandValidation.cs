using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace laba2_MVC.Models
{
    [MetadataType(typeof(BrandMetaData))]
    public partial class Brand
    {
       // [Bind(Exclude = "IdBrand")]
        public class BrandMetaData
        {
            [DisplayName("IdBrand")]
            [Required(ErrorMessage = "IdBrand is required.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public int IdBrand { get; set; }

            [DisplayName("Name")]
            [Required(ErrorMessage = "Brand name is required.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            [StringLength(50)]
            public string Name { get; set; }

            [DisplayName("IdType")]
            [Required(ErrorMessage = "IdType is required.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string IdType { get; set; }

        }

    }
}