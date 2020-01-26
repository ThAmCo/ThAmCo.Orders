using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;
using Orders.Data.Persistence;
using Orders.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Orders.Test.Tests
{

	public class OrdersDbUnitOfWorkTests
	{

		[Fact]
		public async Task GetAllTest()
		{
			var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(databaseName: "Orders").Options;

			using var context = new OrdersDbContext(options);
			var init = new OrdersDbInitialiser();
			await init.SeedTestData(context, null);

			var unitOfWork = new OrdersDbUnitOfWork(context);
			var all = await unitOfWork.Orders.GetAll();

			Assert.Equal(init.Orders.Count(), all.Count());
			foreach (var order in all)
			{
				Assert.Contains(order, context.Orders);
			}
		}

		[Fact]
		public async Task GetOrderHistoryTest()
		{
			var expectedOrder = new Order { ProfileId = 2, ProductId = 2, Address = "18 Holey Road", Name = "Tom Gonsalves", Price = 0.99, PurchaseDateTime = new DateTime(1995, 11, 26) };

			var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(databaseName: "Orders").Options;

			using var context = new OrdersDbContext(options);
			var init = new OrdersDbInitialiser();
			await init.SeedTestData(context, null);

			var unitOfWork = new OrdersDbUnitOfWork(context);

			var history = await unitOfWork.Orders.GetOrderHistory(2);
			Assert.Single(history);

			var actualOrder = history.First();

			Assert.Equal(expectedOrder.Name, actualOrder.Name);
			Assert.Equal(expectedOrder.Price, actualOrder.Price);
			Assert.Equal(expectedOrder.Address, actualOrder.Address);
			Assert.Equal(expectedOrder.ProductId, actualOrder.ProductId);
			Assert.Equal(expectedOrder.ProfileId, actualOrder.ProfileId);
			Assert.Equal(expectedOrder.PurchaseDateTime, actualOrder.PurchaseDateTime);
		}

		[Fact]
		public async Task ContainsTest()
		{
			var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(databaseName: "Orders").Options;

			using var context = new OrdersDbContext(options);
			var init = new OrdersDbInitialiser();
			await init.SeedTestData(context, null);

			var unitOfWork = new OrdersDbUnitOfWork(context);

			var fakeOrder = new Order { ProfileId = 253, ProductId = 242, Address = "18 Smelly Road", Name = "Jon Jones", Price = 0.99, PurchaseDateTime = new DateTime(2050, 11, 26) };
			Assert.False(await unitOfWork.Orders.Contains(fakeOrder));

			var realOrder = new Order { ProfileId = 2, ProductId = 2, Address = "18 Holey Road", Name = "Tom Gonsalves", Price = 0.99, PurchaseDateTime = new DateTime(1995, 11, 26) };
			Assert.True(await unitOfWork.Orders.Contains(realOrder));
		}

	}
}