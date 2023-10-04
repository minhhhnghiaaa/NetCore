using AutoMapper;
using Microsoft.Extensions.Logging;
using NetCore.DataServices.Data;
using NetCore.DataServices.Repositories.Interfaces;

namespace NetCore.DataServices.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public IMemberRepository Members { get; }
    public IMerchantRepository Merchants { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(AppDbContext dbContext, ILoggerFactory loggerFactory,IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        var logger = loggerFactory.CreateLogger("Logs");
        
        Members = new MemberRepository(_dbContext, logger,_mapper);
        Merchants = new MerchantRepository(_dbContext, logger,_mapper);
        Users = new UserRepository(_dbContext, logger);
    }
    
    public async Task<bool> CompleteAsync()
    {
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}