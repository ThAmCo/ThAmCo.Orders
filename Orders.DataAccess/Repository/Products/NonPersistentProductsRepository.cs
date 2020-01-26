using Orders.Data.Models;
using System.Collections.Generic;

namespace Orders.DataAccess.Repository.Products
{
	public class NonPersistentProductsRepository : NonPersistentRepository<Product>, IProductsRepository
	{
		public NonPersistentProductsRepository(List<Product> elements) : base(elements)
		{
		}

	}
}