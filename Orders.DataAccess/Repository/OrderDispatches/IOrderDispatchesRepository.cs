using Orders.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.DataAccess.Repository.OrderDispatches
{
	public interface IOrderDispatchesRepository : IRepository<int, OrderDispatch>
	{

		public Task<OrderDispatch> GetForOrder(int orderId);

		public Task<IEnumerable<OrderDispatch>> GetAll();

	}
}