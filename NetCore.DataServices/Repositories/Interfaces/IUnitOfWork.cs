namespace NetCore.DataServices.Repositories.Interfaces;

public interface IUnitOfWork
{
    IMemberRepository Members { get; }
    IMerchantRepository Merchants { get; }
    IUserRepository Users { get; }
    
    Task<bool> CompleteAsync();
}