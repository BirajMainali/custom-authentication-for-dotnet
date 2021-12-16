using CustomAspNetUser.Base.GenericRepository.Interfaces;
using CustomAspNetUser.Model;

namespace CustomAspNetUser.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsEmailUsed(string email, long? excludedId = null);
}