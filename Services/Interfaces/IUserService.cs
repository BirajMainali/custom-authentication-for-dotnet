using CustomAspNetUser.Dto;
using CustomAspNetUser.Model;

namespace CustomAspNetUser.Services.Interfaces;

public interface IUserService
{
    Task<User> CreateUser(UserDto dto);
    Task Update(User user, UserDto dto);
    Task Remove(User user);
}