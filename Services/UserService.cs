using System.Transactions;
using User.Model;
using User.Dto;
using User.Repository.Interfaces;
using User.Services.Interfaces;
using User.Validator.Interfaces;

namespace User.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _userValidator;

    public UserService(IUserRepository userRepository, IUserValidator userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }
    public async Task<Model.User> CreateUser(UserDto dto)
    {
        using var tsc = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _userValidator.EnsureUniqueUserEmail(dto.Email);
        var user = new Model.User(dto.Name, dto.Gender, dto.Email, Crypter.Crypter.Crypt(dto.Password), dto.Address,dto.Phone);
        await _userRepository.CreateAsync(user);
        await _userRepository.FlushAsync();
        tsc.Complete();
        return user;
    }

    public async Task Update(Model.User user, UserDto dto)
    {
        using var tsc = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _userValidator.EnsureUniqueUserEmail(dto.Email);
        user.Update(dto.Name, dto.Gender, dto.Email, Crypter.Crypter.Crypt(dto.Password), dto.Address, dto.Address);
        _userRepository.Update(user);
        await _userRepository.FlushAsync();
        tsc.Complete();
    }

    public async Task Remove(Model.User user)
    {
        using var tsc = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        _userRepository.Remove(user);
        await _userRepository.FlushAsync();
        tsc.Complete();
    }
}