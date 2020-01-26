using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;
using Orders.Data.Persistence;

namespace Orders.DataAccess.Repository.OrderDispatches
{
	public class DbOrderDispatchesRepository : DbSetRepository<int, OrderDispatch>, IOrderDispatchesRepository
	{

		public DbOrderDispatchesRepository(DbSet<OrderDispatch> dbSet) : base(dbSet)
		{
		}

		protected override IQueryable<OrderDispatch> Including()
		{
			return _dbSet.Include(o => o.Order);
		}

		public async Task<OrderDispatch> GetForOrder(int orderId)
		{
			return await Including().Where(o => o.OrderId == orderId).FirstAsync();
		}

		public async Task<IEnumerable<OrderDispatch>> GetAll()
		{
			return await Including().ToListAsync();
		}

	}
}
