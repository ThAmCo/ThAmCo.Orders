using System.Linq;
using Orders.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Orders.DataAccess.Repository.Orders
{
	public class NonPersistentOrdersRepository : NonPersistentRepository<Order>, IOrdersRepository
	{
		public NonPersistentOrdersRepository(List<Order> elements) : base(elements)
		{
		}

		public Task<bool> Contains(Order order)
		{
			return Task.FromResult(_elements.Contains(order));
		}

		public Task<IEnumerable<Order>> GetAll()
		{
			return Task.FromResult(_elements.AsEnumerable());
		}

		public Task<IEnumerable<Order>> GetOrderHistory(string userId)
		{
			return Task.FromResult(_elements.Where(o => o.UserId.Equals(userId)).AsEnumerable());
		}
	}
}
