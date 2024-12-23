using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Models
{
    public class OrdeHeader :ModelClass
    {
        public int CustomerId { get; set; }
        [ValidateNever]
        public  virtual Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string?  OrderStatus { get; set; }
        public string?  PaymentStatus { get; set; }
        public string TrackingNumber { get; set; }
        public string? FinalReport { get; set; } = null!;
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndtDateTime { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public int? TechnicianId { get; set; }
        public int? DriverId { get; set; }
        [ValidateNever]
        public virtual Technician Technician { get; set; } = null!;
        [ValidateNever]
        public virtual Driver Driver { get; set; } = null!;
        //Stripe
        public string? SessionName { get; set; }
        public string? SessionId { get; set; }


        public string? PaymentIntentId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public long ContactNumber { get; set; }
         


    }
}
