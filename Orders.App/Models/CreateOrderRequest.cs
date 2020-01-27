using Orders.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.App.Models
{
	public class CreateOrderRequest
	{

		[Required]
		public string UserId { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int? ProductId { get; set; }
		
		[Required]
		public double? Price { get; set; }

		[Required]
		public DateTime? PurchaseDateTime { get; set; }

		public Order ToOrder(Product product)
		{
			var userId = UserId;

			return new Order
			{
				ProductId = ProductId.Value,
				Product = product,
				UserId = userId,
				Name = Name,
				Address = Address,
				Price = Price.Value,
				PurchaseDateTime = PurchaseDateTime.Value
			};
		}

	}
}