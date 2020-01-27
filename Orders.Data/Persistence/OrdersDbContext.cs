using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;

namespace Orders.Data.Persistence
{
	public class OrdersDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderDispatch> OrderDispatches { get; set; }

		public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>(e =>
			{
				e.Property(p => p.Name).IsRequired();
				e.Property(p => p.Description).IsRequired();
			});

			modelBuilder.Entity<Order>(e =>
			{
				e.HasOne(o => o.Product)
					.WithMany()
					.HasForeignKey(o => o.ProductId)
					.IsRequired();

				e.Property(p => p.UserId).IsRequired();
				e.Property(p => p.Name).IsRequired();
				e.Property(p => p.Address).IsRequired();
				e.Property(o => o.Price).IsRequired();
				e.Property(o => o.PurchaseDateTime).IsRequired();
			});

			modelBuilder.Entity<OrderDispatch>(e =>
			{
				e.HasOne(o => o.Order)
					.WithOne()
					.HasForeignKey<OrderDispatch>(o => o.OrderId)
					.IsRequired();

				e.Property(o => o.DispatchDateTime).IsRequired();
			});
		}
	}
}
