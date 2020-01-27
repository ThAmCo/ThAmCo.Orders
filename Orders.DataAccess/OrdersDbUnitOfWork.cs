using System.Threading.Tasks;
using Orders.Data.Persistence;
using Orders.DataAccess.Repository.OrderDispatches;
using Orders.DataAccess.Repository.Orders;
using Orders.DataAccess.Repository.Products;

namespace Orders.DataAccess
{
	public class OrdersDbUnitOfWork : IUnitOfWork
	{

		private readonly OrdersDbContext _context;

		private DbOrdersRepository _ordersRepository;

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
