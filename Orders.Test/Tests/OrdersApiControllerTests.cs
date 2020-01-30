using Microsoft.AspNetCore.Mvc;
using Moq;
using Orders.App.Controllers;
using Orders.App.Models;
using Orders.App.Services;
using Orders.Data.Models;
using Orders.DataAccess;
using Orders.DataAccess.Repository.Orders;
using Orders.DataAccess.Repository.Products;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Orders.Test.Tests
{
	public class OrdersApiControllerTests
	{

		[Fact]
		public async Task GetOrders()
		{
			var orders = new List<Order>()
			{
				new Order { UserId = "1", ProductId = 1, Address = "64 Zoo Lane", Name = "Spaghetti Gonsalves", Price = 0.99, PurchaseDateTime = new DateTime(2015, 11, 26) },
				new Order { UserId = "2", ProductId = 2, Address = "18 Holey Road", Name = "Tom Gonsalves", Price = 0.99, PurchaseDateTime = new DateTime(2018, 11, 26) },
			};

			var repository = new NonPersistentOrdersRepository(orders);

			var mockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
			mockUnitOfWork.SetupGet(x => x.Orders).Returns(repository);

			var mockInvoicesService = new Mock<IInvoicesService>(MockBehavior.Strict);
			mockInvoicesService.Setup(s => s.PostOrder(It.IsAny<Order>(), It.IsAny<string>())).Verifiable();

			var apiController = new OrdersApiController(mockUnitOfWork.Object, mockInvoicesService.Object);

			var response = await apiController.GetOrders();
			Assert.NotNull(response);

			var okResult = Assert.IsType<OkObjectResult>(response.Result);
			Assert.NotNull(okResult);

			var resultValues = Assert.IsAssignableFrom<List<Order>>(okResult.Value);
			Assert.NotNull(resultValues);

			Assert.Equal(orders, resultValues);
		}

		[Fact]
		public async Task CreateOrder()
		{
			var order = new Order { Id = 0, UserId = "1", ProductId = 2, Price = 0.99, PurchaseDateTime = new DateTime(2018, 11, 26) };
			var product = new Product { Name = "Razor", Description = "Sharp Blade", Id = 2 };

			var request = new CreateOrderRequest { Price = order.Price, ProductId = order.ProductId, UserId = order.UserId, PurchaseDateTime = order.PurchaseDateTime };

			var orders = new NonPersistentOrdersRepository(new List<Order>());
			var products = new NonPersistentProductsRepository(new List<Product>() { product });

			var mockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
			mockUnitOfWork.Setup(x => x.Commit()).Returns(Task.CompletedTask);
			mockUnitOfWork.SetupGet(x => x.Orders).Returns(orders);
			mockUnitOfWork.SetupGet(x => x.Products).Returns(products);

			var mockInvoicesService = new Mock<IInvoicesService>(MockBehavior.Strict);
			mockInvoicesService.Setup(s => s.PostOrder(It.IsAny<Order>(), It.IsAny<string>())).Verifiable();

			var apiController = new OrdersApiController(mockUnitOfWork.Object, mockInvoicesService.Object);

			var response = await apiController.CreateOrder(request);
			Assert.NotNull(response);

			var okResult = Assert.IsType<OkResult>(response);
			Assert.Equal(okResult.StatusCode, (int) HttpStatusCode.OK);

			Assert.Single(await orders.GetAll());

			var o = await orders.Get(0);
			Assert.Equal(o.Id, order.Id);
		}
	}
}