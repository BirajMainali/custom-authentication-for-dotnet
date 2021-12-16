using CustomAspNetUser.Base.GenericRepository;
using CustomAspNetUser.Model;
using CustomAspNetUser.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomAspNetUser.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
        
    }
    public async Task<bool> IsEmailUsed(string email, long? excludedId = null) 
        => await CheckIfExistAsync(x => (excludedId == null || x.Id != excludedId) && x.Email.Trim().ToLower() == email.Trim().ToLower());
}