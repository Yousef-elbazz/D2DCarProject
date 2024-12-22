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
    public class ShoppingCartRepo: GenericRopository<ShoppingCart>, IShoppingCart
    {
        public CarAppDbContext _context;
        public ShoppingCartRepo(CarAppDbContext  context) : base(context)
        {
            _context = context;
        }

        public int DecreaseCount(ShoppingCart cart, int count)
        {
            cart.count -= count;
            return cart.count;
        }

        public int IncreaseCount(ShoppingCart cart, int count)
        {
            cart.count += count;
            return cart.count;
        }
    }
}
