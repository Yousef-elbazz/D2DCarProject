using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class PartService : ModelClass
    {

        
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string Img { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int ProdServCategoryId { get; set; }
        [ValidateNever]
        public virtual ProdServCategory ProdServCategory { get; set; } = null!;
       

        
    }
}
