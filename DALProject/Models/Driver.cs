using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DALProject.Models
{
    public class  Driver : Employee
    {
        
        public string License  { get; set; }

        
        public DateTime LicenseDate { get; set; }

        
        public DateTime LicenseExpDate { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public virtual ICollection<OrdeHeader> OrdeHeaders { get; set; } = new HashSet<OrdeHeader>();
    }
   

}
