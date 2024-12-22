using BLLProject.Interfaces;
using DALProject;
using DALProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLProject.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly CarAppDbContext dbContext;
        private Hashtable _repository;
        public IShoppingCart shoppingCart { get; private set; }
         public IOrderDetialsRepository orderDetialsRepository { get; private set; }
        public IOrderHeaderRepository orderHeaderRepository { get; private set; }
        // we need here saveChanges and inject dbcontext

        public UnitOfWork(CarAppDbContext dbContext)
        {

            this.dbContext = dbContext;

            _repository = new Hashtable();
            shoppingCart = new ShoppingCartRepo(dbContext);
            orderDetialsRepository = new OrderDetailsRepo(dbContext);
            orderHeaderRepository = new OrderHeaderRepo(dbContext);

        }

        public IGenericRepository<T> Repository<T>() where T : ModelClass
        {
            var key = typeof(T).Name; //Customer
            if (!_repository.ContainsKey(key))
            {
                _repository.Add(key, new GenericRopository<T>(dbContext));

            }
            return _repository[key] as IGenericRepository<T>;
        }

        

        

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public int Complete()
		{
			return dbContext.SaveChanges();
		}

		
	}
}
