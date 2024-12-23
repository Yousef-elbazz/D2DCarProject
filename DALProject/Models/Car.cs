using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class Car : ModelClass
    {

        public string PlateNumber { get; set; }
        
        public int ModelId { get; set; }

        public int CustomerId { get; set; }
        
        public int ColorId { get; set; }

        public int Year { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<OrdeHeader> OrdeHeaders { get; set; } = new HashSet<OrdeHeader>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<KiloMetres> KiloMetres { get; set; } = new HashSet<KiloMetres>();

        public virtual Customer Customer { get; set; } = null!;
        public virtual Color Color { get; set; } = null!;
        public virtual Model Model { get; set; } = null!;

    }
}
