namespace CustomAspNetUser.Exception;

public class DuplicateUserException : System.Exception
{
    public DuplicateUserException(string message = "email must be unique") : base(message)
    {
    }
}