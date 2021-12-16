using System.Transactions;
using CustomAspNetUser.Dto;
using CustomAspNetUser.Model;
using CustomAspNetUser.Repository.Interfaces;
using CustomAspNetUser.Services.Interfaces;
using CustomAspNetUser.Validator.Interfaces;

namespace CustomAspNetUser.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _userValidator;

    public UserService(IUserRepository userRepository, IUserValidator userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }
    public async Task<User> CreateUser(UserDto dto)
    {
        using var tsc = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _userValidator.EnsureUniqueUserEmail(dto.Email);
        var user = new User(dto.Name, dto.Gender, dto.Email, Crypter.Crypter.Crypt(dto.Password), dto.Address,dto.Phone);
        await _userRepository.CreateAsync(user);
        await _userRepository.FlushAsync();
        tsc.Complete();
        return user;
    }

    public async Task Update(User user, UserDto dto)
    {
        using var tsc = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _userValidator.EnsureUniqueUserEmail(dto.Email);
        user.Update(dto.Name, dto.Gender, dto.Email, Crypter.Crypter.Crypt(dto.Password), dto.Address, dto.Address);
        _userRepository.Update(user);
        await _userRepository.FlushAsync();
        tsc.Complete();
    }

    public async Task Remove(User user)
    {
        using var tsc = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        _userRepository.Remove(user);
        await _userRepository.FlushAsync();
        tsc.Complete();
    }
}