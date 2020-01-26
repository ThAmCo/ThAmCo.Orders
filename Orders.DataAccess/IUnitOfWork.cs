using Orders.DataAccess.Repository.OrderDispatches;
using Orders.DataAccess.Repository.Orders;
using Orders.DataAccess.Repository.Products;
using Orders.DataAccess.Repository.Profiles;
using System.Threading.Tasks;

namespace Orders.DataAccess
{
	public interface IUnitOfWork
	{

		public IOrdersRepository Orders { get; }

		public IOrderDispatchesRepository OrderDispatches { get; }

		public IProfilesRepository Profiles { get; }

		public IProductsRepository Products { get; }

		public Task Commit();

	}
}