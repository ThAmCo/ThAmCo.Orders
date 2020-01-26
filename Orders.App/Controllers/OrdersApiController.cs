using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orders.App.Models;
using Orders.App.Services;
using Orders.Data.Models;
using Orders.DataAccess;

namespace Orders.App.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : ControllerBase
	{

		private readonly IUnitOfWork _unitOfWork;

		private readonly IInvoicesService _invoicesService;

		public OrdersApiController(IUnitOfWork unitOfWork, IInvoicesService invoicesService)
        {
			_unitOfWork = unitOfWork;
			_invoicesService = invoicesService;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateOrder([FromBody, Bind("ProductId,ProfileId,Price,PurchasheDateTime")] CreateOrderRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var profile = await _unitOfWork.Profiles.Get(request.ProfileId.Value);
			var product = await _unitOfWork.Products.Get(request.ProductId.Value);

			var order = request.ToOrder(profile, product);
			if (await _unitOfWork.Orders.Contains(order))
			{
				return Conflict(order + " already exists.");
			}

			await _invoicesService.PostOrder(order, profile.Email);

			_unitOfWork.Orders.Create(order);
			await _unitOfWork.Commit();

			return Ok();
		}

		[HttpGet("getorders")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			return Ok(await _unitOfWork.Orders.GetAll());
		}

	}
}
