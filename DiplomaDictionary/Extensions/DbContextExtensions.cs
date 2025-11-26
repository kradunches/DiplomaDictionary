using DiplomaDictionary.Data;
using Microsoft.EntityFrameworkCore;

namespace DiplomaDictionary.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var dataFolderPath =
            new DirectoryInfo(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "data"));
        if (!dataFolderPath.Exists)
            Directory.CreateDirectory(dataFolderPath.FullName);

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

        return services;
    }
}