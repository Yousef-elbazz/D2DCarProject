using BLLProject.Interfaces;
using DALProject;
using DALProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLProject.Repositories
{
    public class OrderHeaderRepo: GenericRopository<OrdeHeader>, IOrderHeaderRepository
    {
        private readonly CarAppDbContext _context;
        public OrderHeaderRepo(CarAppDbContext  context) : base(context)
        {
            _context = context;
        }

        

        public void UpdateStatus(int id, string OrderStatus, string PaymentStatus)
        {
           var orderfromdb = _context.OrdeHeaders.FirstOrDefault(x => x.Id == id);
            orderfromdb.OrderStatus = OrderStatus;  
            orderfromdb.PaymentDate = DateTime.Now;  
            if(PaymentStatus != null)
            {
                orderfromdb.PaymentStatus = PaymentStatus;
            }
        }
    }
}
