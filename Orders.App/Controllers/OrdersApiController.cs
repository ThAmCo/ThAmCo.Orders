using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.App.Models;
using Orders.App.Services;
using Orders.Data.Models;
using Orders.DataAccess;

namespace Orders.App.Controllers
{
    [Route("orders/api")]
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

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("create")]
		public async Task<IActionResult> CreateOrder([FromBody, Bind("ProductId,UserId,Email,Address,Name,Price,PurchaseDateTime")] CreateOrderRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var product = await _unitOfWork.Products.Get(request.ProductId.Value);
			var order = request.ToOrder(product);

			if (await _unitOfWork.Orders.Contains(order))
			{
				return Conflict(order + " already exists.");
			}

			await _invoicesService.PostOrder(order, request.Email);

			_unitOfWork.Orders.Create(order);
			await _unitOfWork.Commit();

			return Ok();
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("get")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			return Ok(await _unitOfWork.Orders.GetAll());
		}

	}
}
