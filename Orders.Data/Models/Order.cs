using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.Data.Models
{
	public class Order : KeyEntity<int>
	{

		[Required]
		public int ProductId { get; set; }

		public Product Product { get; set; }

		[Required]
		public int ProfileId { get; set; }

		public Profile Profile { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public double Price { get; set; }

		[Required]
		public DateTime PurchaseDateTime { get; set; }

	}
}