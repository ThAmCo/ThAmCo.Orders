using System.Net.Http;
using System.Threading.Tasks;

namespace Orders.App.Services
{
	/**
	 * Class that contains useful HttpClient utility methods.
	 */
	public static class HttpClientExtensions
	{

		/**
		 * Constructs an asynchronous http get request and then sends the 
		 * request to the specified uri through a http client. Once response is received
		 * parse and create object of specified, generic, type T.
		 */
		public static async Task<T> HttpGetJsonAsync<T>(this HttpClient client, string uri)
		{
			client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

			HttpResponseMessage responseMessage = await client.GetAsync(uri);
			return await responseMessage.Content.ReadAsAsync<T>();
		}

		/**
		 * Constructs an asynchronous http post request with the specified content
		 * of type T and then sends the request to the specified uri through a http
		 * client. Once response is received parse and create object of specified,
		 * generic, type K.
		 */
		public static async Task<K> HttpPostParseAsync<T, K>(this HttpClient client, string uri, T content)
		{
			client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

			HttpResponseMessage responseMessage = await client.PostAsJsonAsync(uri, content);
			return await responseMessage.Content.ReadAsAsync<K>();
		}

	}
}