namespace Orders.Data.Models
{
	public class Profile : KeyEntity<int>
	{
		public string Name { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }

		public string Address { get; set; }

	}
}