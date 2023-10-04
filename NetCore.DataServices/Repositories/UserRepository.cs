using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore.DataServices.Data;
using NetCore.DataServices.Repositories.Interfaces;
using NetCore.Domain.Entities;

namespace NetCore.DataServices.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
    }

    public async Task<User?> GetByUsername(string username)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Username == username);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} GetByCode func error", typeof(MerchantRepository));
            throw;
        }
    }

    public async Task<bool?> CheckExitsUsername(string username)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Username == username);
            return result != null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} GetByCode func error", typeof(MerchantRepository));
            throw;
        }
    }
}