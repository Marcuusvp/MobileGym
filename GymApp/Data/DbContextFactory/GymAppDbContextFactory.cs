

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GymApp.Data.DbContextFactory;

public class GymAppDbContextFactory : IDesignTimeDbContextFactory<GymAppDbContext>
{
    public GymAppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GymAppDbContext>();

        var connectionString = "Host=localhost;Port=5432;Database=devdb;Username=devuser;Password=senhasegura";

        optionsBuilder.UseNpgsql(connectionString);

        return new GymAppDbContext(optionsBuilder.Options);
    }
}
