using CustomAspNetUser.Model;
using Microsoft.EntityFrameworkCore;

namespace CustomAspNetUser.Application;

public static class EntityRegisterer
{
    public static ModelBuilder AddUser(this ModelBuilder builder)
    {
        builder.Entity<User>();
        return builder;
    }
}