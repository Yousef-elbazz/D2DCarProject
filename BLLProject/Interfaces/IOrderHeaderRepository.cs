using DALProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLProject.Interfaces
{
    public interface IOrderHeaderRepository : IGenericRepository<OrdeHeader>
    {

        
        void UpdateStatus(int id, string OrderStatus, string PaymentStatus);
    }
}
