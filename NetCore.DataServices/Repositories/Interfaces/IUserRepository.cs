using NetCore.Domain.Entities;

namespace NetCore.DataServices.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsername(string username);
    Task<bool?> CheckExitsUsername(string username);
}