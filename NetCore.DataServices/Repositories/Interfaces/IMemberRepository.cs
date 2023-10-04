using NetCore.Domain.Entities;

namespace NetCore.DataServices.Repositories.Interfaces;

public interface IMemberRepository : IGenericRepository<Member>
{
    Task<IEnumerable<Member?>> GetMemberMerchantAsync(Guid merchantId);
    Task<bool> GetByEmail(string email);
    
}
