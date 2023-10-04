using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore.DataServices.Data;
using NetCore.DataServices.Repositories.Interfaces;
using NetCore.Domain.Entities;

namespace NetCore.DataServices.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    protected readonly IMapper _mapper;

    public MemberRepository(AppDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext, logger)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<Member?>> GetMemberMerchantAsync(Guid merchantId)
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
                .Where(x => x.MerchantId == merchantId)
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} GetMemberMerchantAsync func error", typeof(MemberRepository));
            throw;
        }
    }

    public async Task<bool> GetByEmail(string email)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
            return result != null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} GetByEmail func error", typeof(MemberRepository));
            throw;
        }
    }

    public override async Task<IEnumerable<Member>> All()
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
            _logger.LogError(e, "{Repo} All func error", typeof(MemberRepository));
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
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Delete func error", typeof(MemberRepository));
            throw;
        }
    }

    public override async Task<bool> Update(Member member)
    {
        if (member == null)
        {
            throw new ArgumentNullException(nameof(member));
        }

        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == member.Id);
            if (result == null)
            {
                return false;
            }

            result = _mapper.Map<Member, Member>(member, result);

            _dbSet.Update(result);

            return true;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e, "{Repo} Update func error", typeof(MemberRepository));
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Update func error", typeof(MemberRepository));
            throw;
        }
    }
}