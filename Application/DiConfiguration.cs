using CustomAspNetUser.Repository;
using CustomAspNetUser.Repository.Interfaces;
using CustomAspNetUser.Services;
using CustomAspNetUser.Services.Interfaces;
using CustomAspNetUser.Validator;
using CustomAspNetUser.Validator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CustomAspNetUser.Application
{
    public static class DiConfiguration
    {
        public static void UseUserConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>().
                AddScoped<IUserService, UserService>()
                .AddScoped<IUserValidator, UserValidator>();
        }
    }
}