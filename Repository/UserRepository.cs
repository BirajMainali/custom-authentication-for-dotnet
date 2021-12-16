using User.Model;
using Microsoft.EntityFrameworkCore;
using User.Base.GenericRepository;
using User.Repository.Interfaces;

namespace User.Repository;

public class UserRepository : GenericRepository<Model.User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
        
    }
    public async Task<bool> IsEmailUsed(string email, long? excludedId = null) 
        => await CheckIfExistAsync(x => (excludedId == null || x.Id != excludedId) && x.Email.Trim().ToLower() == email.Trim().ToLower());
}