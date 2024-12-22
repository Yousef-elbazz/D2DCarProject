using DALProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLLProject.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : ModelClass;
        IShoppingCart shoppingCart { get; }
        IOrderDetialsRepository orderDetialsRepository { get; }
        IOrderHeaderRepository orderHeaderRepository { get; }

        abstract int Complete();
		
	}
}
