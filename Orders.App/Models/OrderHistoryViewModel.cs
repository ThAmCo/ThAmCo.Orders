using Orders.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orders.App.Models
{
	public class OrderHistoryViewModel
	{
		public string Name { get; set; }

		public string Address { get; set; }

		public string ProductName { get; set; }

		public double Price { get; set; }

		public string DispatchStatus { get; set; }

		public DateTime PurchaseDateTime { get; set; }

		public static OrderHistoryViewModel From(Order o, IEnumerable<OrderDispatch> od)
		{
			var dispatchStatus = (od == null || !od.Any()) ? "Not Dispatched" : od.First().DispatchDateTime.ToString();

			return new OrderHistoryViewModel
			{
				Name = o.Name,
				Address = o.Address,
				ProductName = o.Product.Name,
				Price = o.Price,
				DispatchStatus = dispatchStatus,
				PurchaseDateTime = o.PurchaseDateTime
			};
		}

	}
}