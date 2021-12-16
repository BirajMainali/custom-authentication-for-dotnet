using User.Model;
using Microsoft.EntityFrameworkCore;

namespace User.Application;

public static class EntityRegisterer
{
    public static ModelBuilder AddUser(this ModelBuilder builder)
    {
        builder.Entity<Model.User>();
        return builder;
    }
}