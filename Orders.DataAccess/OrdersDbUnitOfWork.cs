using System.Threading.Tasks;
using Orders.Data.Persistence;
using Orders.DataAccess.Repository.OrderDispatches;
using Orders.DataAccess.Repository.Orders;
using Orders.DataAccess.Repository.Products;
using Orders.DataAccess.Repository.Profiles;

namespace Orders.DataAccess
{
	public class OrdersDbUnitOfWork : IUnitOfWork
	{

		private readonly OrdersDbContext _context;

		private DbOrdersRepository _ordersRepository;

		private DbProfilesRepository _profilesRepository;

		private DbOrderDispatchesRepository _orderDispatchesRepository;

		private DbProductsRepository _productsRepository;

		public OrdersDbUnitOfWork(OrdersDbContext context)
		{
			_context = context;
		}

		public async Task Commit()
		{
			await _context.SaveChangesAsync();
		}

		public IOrdersRepository Orders
		{
			get
			{
				if (_ordersRepository == null)
				{
					_ordersRepository = new DbOrdersRepository(_context.Orders);
				}
				return _ordersRepository;
			}
		}

		public IProfilesRepository Profiles
		{
			get
			{
				if (_profilesRepository == null)
				{
					_profilesRepository = new DbProfilesRepository(_context.Profiles);
				}
				return _profilesRepository;
			}
		}

		public IOrderDispatchesRepository OrderDispatches
		{
			get
			{
				if (_orderDispatchesRepository == null)
				{
					_orderDispatchesRepository = new DbOrderDispatchesRepository(_context.OrderDispatches);
				}
				return _orderDispatchesRepository;
			}
		}

		public IProductsRepository Products
		{
			get
			{
				if (_productsRepository == null)
				{
					_productsRepository = new DbProductsRepository(_context.Products);
				}
				return _productsRepository;
			}
		}

	}
}
