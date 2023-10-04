using NetCore.Domain.Entities;

namespace NetCore.DataServices.Repositories.Interfaces;

public interface IMerchantRepository : IGenericRepository<Merchant>
{
    Task<bool> GetByCode(string code);
}