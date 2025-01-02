using Ecommerce.Data;
using Ecommerce.Interfaces;

namespace Ecommerce.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly AppDbContext _context;
		public GenericRepository(AppDbContext dbContext)
		{
			_context = dbContext;
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public T GetById(int id)
		{
			throw new NotImplementedException();
		}
	}
}
