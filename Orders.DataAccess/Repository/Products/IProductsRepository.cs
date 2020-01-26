using Orders.Data.Models;

namespace Orders.DataAccess.Repository.Products
{
	public interface IProductsRepository : IRepository<int, Product>
	{
	}
}