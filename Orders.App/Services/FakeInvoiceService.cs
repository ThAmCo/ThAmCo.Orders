using System.Threading.Tasks;
using Orders.Data.Models;

namespace Orders.App.Services
{
	public class FakeInvoiceService : IInvoicesService
	{
		public Task PostOrder(Order order, string email)
		{
			return Task.CompletedTask;
		}
	}
}
