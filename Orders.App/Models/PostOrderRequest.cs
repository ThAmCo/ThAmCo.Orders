using Orders.Data.Models;
using System;

namespace Orders.App.Models
{
	public class PostOrderRequest
	{

		public string UserId { get; set; }

		public string ProductName { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public double Price { get; set; }

		public string Email { get; set; }

		public DateTime PurchaseDateTime { get; set; }

		public static PostOrderRequest FromOrder(Order order, string email)
		{
			return new PostOrderRequest
			{
				UserId = order.UserId,
				Email = email,
				Address = order.Address,
				Name = order.Name,
				Price = order.Price,
				ProductName = order.Product.Name,
				PurchaseDateTime = order.PurchaseDateTime
			};
		}

	}
}