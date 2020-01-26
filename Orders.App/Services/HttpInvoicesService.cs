using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Orders.App.Models;
using Orders.Data.Models;

namespace Orders.App.Services
{
	public class HttpInvoicesService : IInvoicesService
	{

		private readonly IConfiguration _config;

		private readonly IHttpClientFactory _httpClientFactory;

		public HttpInvoicesService(IConfiguration config, IHttpClientFactory httpClientFactory)
		{
			_config = config;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<bool> PostOrder(Order order, string email)
		{
			var request = PostOrderRequest.FromOrder(order, email);
			var uri = _config.GetConnectionString("InvoicesConnectionUri") + "api/invoices/postorder";

			HttpClient client = _httpClientFactory.CreateClient();
			client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
			var response = await client.PostAsJsonAsync(uri, request);

			return response.IsSuccessStatusCode;
		}
	}
}
