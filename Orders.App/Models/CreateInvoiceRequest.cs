using System.Collections.Generic;

namespace Orders.App.Models
{
	public class CreateInvoiceRequest
	{
		public int ProfileId { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string Address { get; set; }

		public List<int> OrderIds { get; set; }

	}
}