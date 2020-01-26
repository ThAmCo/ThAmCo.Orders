using Orders.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Data.Persistence
{
	public class OrdersDbInitialiser
	{
		public List<Order> Orders { get; } = new List<Order>();
		public List<OrderDispatch> OrderDispatches { get; } = new List<OrderDispatch>();

		public async Task SeedTestData(OrdersDbContext context, IServiceProvider services)
		{
			var products = new List<Product>
			{
				new Product { Name = "Wrap It and Hope Cover", Description = "Poor quality fake faux leather cover loose enough to fit any mobile device." },
				new Product { Name = "Chocolate Cover", Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device." },
				new Product { Name = "Cloth Cover", Description = "Lamely adapted used and dirty teatowel.  Guaranteed fewer than two holes." },
				new Product { Name = "Harden Sponge Case", Description = "Especially toughen and harden sponge entirely encases your device to prevent any interaction." },
				new Product { Name = "Water Bath Case", Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs." },
				new Product { Name = "Smartphone Car Holder", Description = "Keep you smartphone handsfree with this large assembly that attaches to your rear window wiper (Hatchbacks only)." },
				new Product { Name = "Sticky Tape Sport Armband", Description = "Keep your device on your arm with this general purpose sticky tape." },
				new Product { Name = "Real Pencil Stylus", Description = "Stengthen HB pencils guaranteed to leave a mark." },
				new Product { Name = "Spray Paint Screen Protector", Description = "Coat your mobile device screen in a scratch resistant, opaque film." },
				new Product { Name = "Rippled Screen Protector", Description = "For his or her sensory pleasure. Fits few known smartphones." },
				new Product { Name = "Fish Scented Screen Protector", Description = "For an odour than lingers on your device." },
				new Product { Name = "Non-conductive Screen Protector", Description = "Guaranteed not to conduct electical charge from your fingers." }
			};

			var profiles = new Profile[]
			{
				new Profile { Name = "Sam Hammersley - Gonsalves", Address = "1 Spaghetti Drive", Email = "sam.hammersley@outlook.com", PhoneNumber = "07941395171" },
				new Profile { Name = "Tom Gonsalves", Address = "18 Holey Road", Email = "t.gonsalves@ntlworld.com", PhoneNumber = "07722281941" },
				new Profile { Name = "Kim Hammersley - Gonsalves", Address = "5 Hello Close", Email = "k.gonsalves@ntlworld.com", PhoneNumber = "07594420143" },
				new Profile { Name = "Francesca Hammersley - Gonsalves", Address = "3 China Road", Email = "francescahammersley@gmail.com", PhoneNumber = "07941395171" }
			};

			for (int i = 0; i < profiles.Length; i++)
			{
				var prof = profiles[i];
				Orders.Add(new Order { ProfileId = i + 1, Address = prof.Address, Name = prof.Name, Price = 0.99, ProductId = i + 1, PurchaseDateTime = new DateTime(1995, 11, 26) });

				OrderDispatches.Add(new OrderDispatch { Order = Orders[i], DispatchDateTime = DateTime.Now });
			}

			if (!context.Products.Any()) 
			{
				products.ForEach(p => context.Products.Add(p));
				await context.SaveChangesAsync();
			}

			if (!context.Profiles.Any())
			{
				for (int i = 0; i < profiles.Length; i++)
				{
					context.Profiles.Add(profiles[i]);
				}
				await context.SaveChangesAsync();
			}

			if (!context.Orders.Any())
			{
				for (int i = 0; i < Orders.Count; i++)
				{
					context.Orders.Add(Orders[i]);
				}
				await context.SaveChangesAsync();
			}

			if (!context.OrderDispatches.Any())
			{
				OrderDispatches.ForEach(o => context.OrderDispatches.Add(o));
				await context.SaveChangesAsync();
			}
		}
	}
}