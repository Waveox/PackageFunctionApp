using PackageFunctionApp.App.Models;

namespace PackageFunctionApp.Services;
public interface IPackageService
{
    Task StorePackageAsync(Package package);
}