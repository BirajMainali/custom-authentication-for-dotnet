using Microsoft.Extensions.DependencyInjection;
using User.Repository;
using User.Repository.Interfaces;
using User.Services;
using User.Services.Interfaces;
using User.Validator;
using User.Validator.Interfaces;

namespace User.Application;

public static class DiConfiguration
{
    public static void UseUserConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>().
            AddScoped<IUserService, UserService>()
            .AddScoped<IUserValidator, UserValidator>();
    }
}