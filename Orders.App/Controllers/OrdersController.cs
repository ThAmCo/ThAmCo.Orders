using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.App.Models;
using Orders.DataAccess;

namespace Orders.App.Controllers
{
    public class OrdersController : Controller
    {

		private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

		[Authorize]
		public async Task<IActionResult> History()
        {
			var userId = User.FindFirst("sub").Value;
			var history = await _unitOfWork.Orders.GetOrderHistory(userId);
			var dispatches = await _unitOfWork.OrderDispatches.GetAll();

			return View(history.GroupJoin(dispatches, order => order, dispatch => dispatch.Order, OrderHistoryViewModel.From));
        }

		[Authorize(Policy = "StaffOnly")]
		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.Orders.GetAll());
		}


		public IActionResult Odnex()
		{
			return View();
		}

    }
}
