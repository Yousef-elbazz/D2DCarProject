using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject;
using DALProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLLProject.Repositories
{
    public class OrderDetailsRepo: GenericRopository<OrderDetial>, IOrderDetialsRepository
    {
        private readonly CarAppDbContext _context;
        public OrderDetailsRepo(CarAppDbContext  context) : base(context)
        {
            _context = context;
        }

    }
}
