using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore.DataServices.Data;
using NetCore.DataServices.Repositories.Interfaces;
using NetCore.Domain.Entities;

namespace NetCore.DataServices.Repositories;

public class MerchantRepository : GenericRepository<Merchant>, IMerchantRepository
{
    protected readonly IMapper _mapper;

    public MerchantRepository(AppDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext, logger)
    {
        _mapper = mapper;
    }

    public override async Task<IEnumerable<Merchant>> All()
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} All func error", typeof(MerchantRepository));
            throw;
        }
    }

    public override async Task<bool> Delete(Guid id)
    {
        try
        {
            // get my entity
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return false;
            }
            // delete my entity
            result.Status = 0;
            _dbSet.Update(result);

            return true;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e, "{Repo} Delete func error", typeof(MerchantRepository));
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Delete func error", typeof(MerchantRepository));
            throw;
        }
    }

    public async Task<bool> GetByCode(string code)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.SwiftCode == code);
            return result != null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} GetByCode func error", typeof(MerchantRepository));
            throw;
        }
    }

    public override async Task<bool> Update(Merchant merchant)
    {
        if (merchant == null)
        {
            throw new ArgumentNullException(nameof(merchant));
        }

        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == merchant.Id);
            if (result == null)
            {
                return false;
            }
            
            result = _mapper.Map<Merchant,Merchant>(merchant, result);
            
            _dbSet.Update(result);

            return true;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e, "{Repo} Update func error", typeof(MerchantRepository));
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Update func error", typeof(MerchantRepository));
            throw;
        }
    }
}