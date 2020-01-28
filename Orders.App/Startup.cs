using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.App.Services;
using Orders.Data.Persistence;
using Orders.DataAccess;
using System;
using System.IO;

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

			string azureStorageUri = Environment.GetEnvironmentVariable("COOKIE_STORAGE_URI");

			// Create the new Storage URI
			Uri storageUri = new Uri(azureStorageUri);

			//Create the blob client object.
			CloudBlobClient blobClient = new CloudBlobClient(storageUri);

			//Get a reference to a container to use for the sample code, and create it if it does not exist.
			CloudBlobContainer container = blobClient.GetContainerReference("keys");

			services.AddDataProtection()
				.PersistKeysToAzureBlobStorage(container, "cookies")
				.SetApplicationName("ThAmCo");

			services.AddAuthentication("Cookies")
				.AddCookie("Cookies", options =>
				{
					options.Cookie.Name = ".ThAmCo.SharedCookie";
					options.Cookie.Path = "/";
					options.LoginPath = "/Home/Account/Login";
					options.LogoutPath = "/Home/Account/Logout";
					options.AccessDeniedPath = "/Home/Account/Login";
				});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("StaffOnly", builder =>
				{
					builder.RequireClaim("role", "Staff");
				});
			});

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

			app.UseAuthentication();
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