using Orders.Data.Models;
using System.Collections.Generic;

namespace Orders.DataAccess.Repository.Profiles
{
	public class NonPersistentProfilesRepository : NonPersistentRepository<Profile>, IProfilesRepository
	{
		public NonPersistentProfilesRepository(List<Profile> elements) : base(elements)
		{
		}

	}
}