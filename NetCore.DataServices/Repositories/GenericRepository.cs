using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore.DataServices.Data;
using NetCore.DataServices.Repositories.Interfaces;

namespace NetCore.DataServices.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ILogger _logger;
    protected AppDbContext _dbContext;
    internal DbSet<T> _dbSet;

    protected GenericRepository(AppDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return true;
    }

    public virtual async Task<bool> Update(T entity)
    {
        _dbSet.Update(entity);
        return true;
    }

    public virtual async Task<bool> Delete(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);
        return true;
    }
}