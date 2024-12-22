using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class OrderDetial: ModelClass
    {

        public int OrdeHeaderId { get; set; }
        [ValidateNever]
        public virtual  OrdeHeader OrdeHeader { get; set; }
        public int PartServiceId { get; set; }
        [ValidateNever]
        public virtual PartService PartService { get; set; }
        public int count { get; set; }

        public decimal Price { get; set; }
    }
}
