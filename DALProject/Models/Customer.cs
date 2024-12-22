using DALProject.Models.sss;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DALProject.Models
{
    public class Customer :  ModelClass
    {
        public virtual ICollection<Car> Cars { get; set; } = new HashSet<Car>();
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; } = new HashSet<CartItem>();
    }
}
