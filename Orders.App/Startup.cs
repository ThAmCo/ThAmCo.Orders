using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.App.Services;
using Orders.Data.Persistence;
using Orders.DataAccess;
using System;

namespace Orders.App
{
	public class Startup
	{

		private readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;

			_environment = environment;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration.GetConnectionString("DatabaseConnection");

			services.AddControllersWithViews();
			services.AddHttpClient();
			services.AddDbContext<OrdersDbContext>(options => options.UseSqlServer(connectionString));

			if (_environment.IsDevelopment())
			{
				services.AddScoped<IUnitOfWork, OrdersDbUnitOfWork>();
				services.AddScoped<IInvoicesService, HttpInvoicesService>();
			}
			else
			{
				services.AddScoped<IUnitOfWork, OrdersDbUnitOfWork>();
				services.AddScoped<IInvoicesService, HttpInvoicesService>();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Orders}/{action=Index}/{id?}");
			});
		}
	}
}