using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;
using System.Linq;

namespace Orders.DataAccess.Repository.Products
{
	class DbProductsRepository : DbSetRepository<int, Product>, IProductsRepository
	{
		public DbProductsRepository(DbSet<Product> dbSet) : base(dbSet)
		{
		}

		protected override IQueryable<Product> Including()
		{
			return _dbSet.AsQueryable();
		}

	}
}