using System.Threading.Tasks;

namespace Orders.DataAccess.Repository
{
	public interface IRepository<K, T> where T : class
	{

		Task<T> Get(K key);

		void Create(T t);

		void Update(T t);

		void Remove(T t);

	}
}