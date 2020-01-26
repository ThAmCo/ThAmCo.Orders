using System.Linq;
using Microsoft.EntityFrameworkCore;
using Orders.Data.Models;

namespace Orders.DataAccess.Repository.Profiles
{
	public class DbProfilesRepository : DbSetRepository<int, Profile>, IProfilesRepository
	{

		public DbProfilesRepository(DbSet<Profile> dbSet) : base(dbSet)
		{
		}

		protected override IQueryable<Profile> Including()
		{
			return _dbSet.AsQueryable();
		}

	}
}
