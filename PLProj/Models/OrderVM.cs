using DALProject.Models;
using System.Collections;
using System.Collections.Generic;

namespace PLProj.Models
{
    public class OrderVM
    {
        public OrdeHeader OrdeHeader { get; set; }
        public IEnumerable<OrderDetial> OrderDetials { get; set; }
        public Customer Customer { get; set; }

    }
}
