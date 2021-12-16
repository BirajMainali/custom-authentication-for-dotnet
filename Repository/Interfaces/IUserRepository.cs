using User.Model;
using User.Base.GenericRepository.Interfaces;

namespace User.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<Model.User>
{
    Task<bool> IsEmailUsed(string email, long? excludedId = null);
}