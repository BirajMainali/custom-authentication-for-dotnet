using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using User.Base.GenericRepository.Interfaces;

namespace User.Base.GenericRepository;

public class GenericRepository<T>  : IGenericRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;


    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task CreateAsync(T t) => await _context.AddAsync(t);

    public void Update(T t) => _context.Set<T>().Update(t);

    public async Task<long> FlushAsync() => await _context.SaveChangesAsync();

    public void Remove(T t) => _context.Set<T>().Remove(t);

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
    {
        predicate ??= x => true;
        return _context.Set<T>().Where(predicate).ToListAsync();
    }

    public List<T> Get(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).ToList();

    public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
    {
        predicate ??= x => true;
        return await _context.Set<T>().CountAsync(predicate);
    }

    public async Task<T> FindAsync(long id) => await _dbSet.FindAsync(id);

    public async Task<bool> CheckIfExistAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

    public async Task<T> FindOrThrowAsync(long id) => await FindAsync(id) ?? throw new System.Exception("Not Found");

    public IQueryable<T> GetQueryable() => _dbSet.AsQueryable();

    public Task<T> GetItemAsync(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefaultAsync(predicate);

    public T Find(long id) => _dbSet.Find(id);
}