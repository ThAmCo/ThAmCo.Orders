using Orders.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.App.Models
{
	public class CreateOrderRequest
	{

		[Required]
		public int? ProfileId { get; set; }

		[Required]
		public int? ProductId { get; set; }
		
		[Required]
		public double? Price { get; set; }

		[Required]
		public DateTime? PurchaseDateTime { get; set; }

		public Order ToOrder(Profile profile, Product product)
		{
			return new Order
			{
				ProductId = ProductId.Value,
				Product = product,
				ProfileId = ProfileId.Value,
				Profile = profile,
				Name = profile.Name,
				Address = profile.Address,
				Price = Price.Value,
				PurchaseDateTime = PurchaseDateTime.Value
			};
		}

	}
}