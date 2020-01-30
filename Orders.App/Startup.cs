using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.App.Services;
using Orders.Data.Persistence;
using Orders.DataAccess;
using Polly;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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

			if (_environment.IsDevelopment())
			{
				services.AddDataProtection()
					.PersistKeysToFileSystem(new DirectoryInfo(@"c:/shared-cookies"))
					.SetApplicationName("ThAmCo");
			}
			else
			{
				var storageKey = Configuration["BLOB_STORAGE_KEY"];

				var credentials = new StorageCredentials("thamcostorage", storageKey);
				var storageAccount = new CloudStorageAccount(credentials, true);
				var blobClient = storageAccount.CreateCloudBlobClient();

				CloudBlobContainer container = blobClient.GetContainerReference("keys");

				services.AddDataProtection()
					.PersistKeysToAzureBlobStorage(container, "cookies")
					.SetApplicationName("ThAmCo");
			}

			var homeBaseUrl = Configuration["HOME_URL"];
			var authAuthorityUrl = Configuration["AUTHENTICATION_AUTHORITY"];

			services.AddAuthentication("Cookies")
				.AddJwtBearer("Bearer", options =>
				{
					options.Authority = authAuthorityUrl;
					options.Audience = "thamco_orders_api";
				})
				.AddCookie("Cookies", options =>
				{
					options.Cookie.Name = ".ThAmCo.SharedCookie";
					options.Cookie.Path = "/";
					options.LoginPath = "/Account/Login";
					options.LogoutPath = "/Account/Logout";

					options.Events.OnRedirectToLogin = context =>
					{
						context.HttpContext.Response.Redirect(homeBaseUrl + options.LoginPath);
						return Task.CompletedTask;
					};

					options.Events.OnRedirectToLogout = context =>
					{
						context.HttpContext.Response.Redirect(homeBaseUrl + options.LogoutPath);
						return Task.CompletedTask;
					};

					options.Events.OnRedirectToAccessDenied = context =>
					{
						context.HttpContext.Response.Redirect(homeBaseUrl + "/accessdenied");
						return Task.CompletedTask;
					};
				});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("StaffOnly", builder =>
				{
					builder.RequireClaim("role", "Staff");
				});
			});

			using var response = new HttpResponseMessage()
			{
				Content = new StringContent("Fallback response!"),
				StatusCode = HttpStatusCode.OK
			};

			services.AddHttpClient("PollyClient")
					.AddTransientHttpErrorPolicy(p => p.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
					.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
					.AddTransientHttpErrorPolicy(prop => prop.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)))
					.AddTransientHttpErrorPolicy(b => b.FallbackAsync(response));
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
					pattern: "{controller=Orders}/{action=Index}");
			});
		}
	}
}