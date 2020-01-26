using Orders.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.DataAccess.Repository.Orders
{
	public interface IOrdersRepository : IRepository<int, Order>
	{

		Task<IEnumerable<Order>> GetAll();

		Task<IEnumerable<Order>> GetOrderHistory(int profileId);

		Task<bool> Contains(Order order);

	}
}