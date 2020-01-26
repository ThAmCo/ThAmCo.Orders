using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Data.Persistence;
using System;

namespace Orders.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var env = services.GetRequiredService<IWebHostEnvironment>();
				if (env.IsDevelopment())
				{
					var context = services.GetRequiredService<OrdersDbContext>();
					//context.Database.EnsureDeleted();
					context.Database.Migrate();
					try
					{
						new OrdersDbInitialiser().SeedTestData(context, services).Wait();
					}
					catch (Exception e)
					{
						var logger = services.GetRequiredService<ILogger<Program>>();
						logger.LogDebug("Seeding test data failed. " + e.ToString());
					}
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
	}
}
