using Orders.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.DataAccess.Repository
{
	public class NonPersistentRepository<T> : IRepository<int, T> where T : KeyEntity<int>
	{

		protected readonly List<T> _elements;

		public NonPersistentRepository(List<T> elements)
		{
			_elements = elements;
		}

		public Task<T> Get(int key)
		{
			T value = _elements.First(e => e.Id == key);

			return Task.FromResult(value);
		}

		public void Create(T t)
		{
			_elements.Add(t);
		}

		public void Remove(T t)
		{
			_elements.Remove(t);
		}

		public void Update(T t)
		{
			int index = _elements.IndexOf(t);

			_elements.RemoveAt(index);
			_elements.Insert(index, t);
		}
	}
}