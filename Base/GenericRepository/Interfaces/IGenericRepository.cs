using System.Linq.Expressions;

namespace User.Base.GenericRepository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task CreateAsync(T t);
    void Update(T t);
    Task<long> FlushAsync();
    void Remove(T t);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
    List<T> Get(Expression<Func<T, bool>> predicate);
    Task<T> GetItemAsync(Expression<Func<T, bool>> predicate);
    Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);
    Task<T> FindAsync(long id);
    Task<bool> CheckIfExistAsync(Expression<Func<T, bool>> predicate);
    Task<T> FindOrThrowAsync(long id);
    IQueryable<T> GetQueryable();
}