using DALProject.Migrations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class ShoppingCart : ModelClass
    {
        public override int Id
        {
            get => ShoppingCartId; // إعادة التوجيه إلى ShoppingCartId
            set => ShoppingCartId = value;
        }
        public int ShoppingCartId { get; set; }
        public int PartServiceId { get; set; }
        [ValidateNever]
        public  virtual PartService PartService { get; set; }
        [Range(1, 100, ErrorMessage = "You must Enter value between 1 to 100")]
        public int count { get; set; }

        public int CustomerId { get; set; }
        [ValidateNever]
        public virtual  Customer Customer { get; set; }
    }
}
