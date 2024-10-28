using PackageFunctionApp.App.Models;
using PackageFunctionApp.Infrastructure.Database;

namespace PackageFunctionApp.Services.Implementations;
public class PackageService : IPackageService
{
    private readonly AppDbContext _context;

    public PackageService(AppDbContext context)
    {
        _context = context;
    }

    public async Task StorePackageAsync(Package package)
    {
        await _context.Packages.AddAsync(package);
        await _context.SaveChangesAsync();
    }
}