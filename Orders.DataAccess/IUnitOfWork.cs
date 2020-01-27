using Orders.DataAccess.Repository.OrderDispatches;
using Orders.DataAccess.Repository.Orders;
using Orders.DataAccess.Repository.Products;
using System.Threading.Tasks;

namespace Orders.DataAccess
{
	public interface IUnitOfWork
	{

		public IOrdersRepository Orders { get; }

		public IOrderDispatchesRepository OrderDispatches { get; }

		public IProductsRepository Products { get; }

		public Task Commit();

	}
}