using CustomAspNetUser.Exception;
using CustomAspNetUser.Repository.Interfaces;
using CustomAspNetUser.Validator.Interfaces;

namespace CustomAspNetUser.Validator;

public class UserValidator : IUserValidator
{
    private readonly IUserRepository _userRepository;

    public UserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task EnsureUniqueUserEmail(string email, long? id = null)
    {
        if (await _userRepository.IsEmailUsed(email, id))
            throw new DuplicateUserException();
    }
}