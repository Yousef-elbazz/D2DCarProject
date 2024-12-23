using System.Collections.Generic;

namespace DALProject.Models
{
    public class Technician : Employee
    {
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<OrdeHeader> OrdeHeaders { get; set; } = new HashSet<OrdeHeader>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
