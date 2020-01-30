using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
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

		public async Task PostOrder(Order order, string email)
		{
			var request = PostOrderRequest.FromOrder(order, email);
			var uri = _config.GetConnectionString("InvoicesConnectionUri") + "invoices/api/postorder";

			var client = _httpClientFactory.CreateClient("PollyClient");
			client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

			var tokenResponse = await client.GetTokenAsync(_config["AUTHENTICATION_AUTHORITY"]);
			if (tokenResponse.IsError)
			{
				throw new HttpRequestException("Error requesting token from authority");
			}

			client.SetBearerToken(tokenResponse.AccessToken);

			var response = await client.PostAsJsonAsync(uri, request);

			response.EnsureSuccessStatusCode();
		}
	}
}
