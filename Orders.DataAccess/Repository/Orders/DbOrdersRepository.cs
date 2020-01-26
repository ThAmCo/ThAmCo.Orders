using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;

namespace Orders.DataAccess.Repository.Orders
{
	public class DbOrdersRepository : DbSetRepository<int, Order>, IOrdersRepository
	{

		public DbOrdersRepository(DbSet<Order> dbSet) : base(dbSet)
		{
		}

		protected override IQueryable<Order> Including()
		{
			return _dbSet.Include(o => o.Product);
		}

		public async Task<IEnumerable<Order>> GetOrderHistory(int profileId)
		{
			return await Including().Where(o => o.ProfileId == profileId).ToListAsync();
		}

		public async Task<bool> Contains(Order order)
		{
			return await Including()
				.Where(o => o.Id == order.Id ||
				(o.PurchaseDateTime == order.PurchaseDateTime
				&& o.ProfileId == order.ProfileId 
				&& o.ProductId == order.ProductId))
				.AnyAsync();

			//return await Including().ContainsAsync(order);
		}

		public async Task<IEnumerable<Order>> GetAll()
		{
			return await Including().ToListAsync();
		}
	}
}
