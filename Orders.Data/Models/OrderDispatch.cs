using System;

namespace Orders.Data.Models
{
	public class OrderDispatch : KeyEntity<int>
	{

		public int OrderId { get; set; }

		public Order Order { get; set; }

		public DateTime DispatchDateTime { get; set; }

	}
}