﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class Appointment : ModelClass
    {
        
        public string? PartialReport { get; set; } = null!;
        
        public DateTime StartDateTime { get; set; }
        
        public DateTime EndtDateTime { get; set; }
        public int TechnicianId { get; set; }
        public int DriverId { get; set; }
        public int TicketId { get; set; }

        [InverseProperty(nameof(Technician.Appointments))]
        public virtual Technician Technicians { get; set; } = null!;

        [InverseProperty(nameof(Driver.Appointments))]
        public virtual Driver Drivers { get; set; } = null!;

        [InverseProperty(nameof(Ticket.Appointments))]
        public virtual Ticket Tickets { get; set; } = null!;


    }
}
