namespace Ecommerce.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		T GetById(int id);

		void Delete(int id);
	}
}
