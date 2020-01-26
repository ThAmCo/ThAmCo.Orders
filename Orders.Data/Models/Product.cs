namespace Orders.Data.Models
{
	public class Product : KeyEntity<int>
	{

		public string Name { get; set; }

		public string Description { get; set; }

	}
}
