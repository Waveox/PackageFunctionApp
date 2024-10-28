using PackageFunctionApp.App.Models;

namespace PackageFunctionApp.Services;
public interface IPackageParser
{
    IAsyncEnumerable<Package> ParseAsync(string filePath);
}
