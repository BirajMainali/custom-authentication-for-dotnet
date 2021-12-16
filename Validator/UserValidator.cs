using User.Exception;
using User.Repository.Interfaces;
using User.Validator.Interfaces;

namespace User.Validator;

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