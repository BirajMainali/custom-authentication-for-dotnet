namespace CustomAspNetUser.Validator.Interfaces;

public interface IUserValidator
{
    Task EnsureUniqueUserEmail(string email, long? id = null);
}