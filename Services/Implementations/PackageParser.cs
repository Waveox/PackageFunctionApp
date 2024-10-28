using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PackageFunctionApp.App.Models;
using PackageFunctionApp.Configurations;

namespace PackageFunctionApp.Services.Implementations;

public class PackageParser : IPackageParser
{
    private readonly PackageFunctionOptions _options;
    private readonly ILogger<PackageParser> _logger;

    public PackageParser(IOptions<PackageFunctionOptions> options, ILogger<PackageParser> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async IAsyncEnumerable<Package> ParseAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        string line;
        Package currentPackage = null;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            if (IsPackage(parts))
            {
                if (currentPackage != null)
                {
                    yield return currentPackage;
                }

                currentPackage = ParsePackage(parts);
            }
            else if (IsItem(parts) && currentPackage != null)
            {
                try
                {
                    var item = ParseItem(parts);
                    currentPackage.Items.Add(item);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error parsing item: {line}");
                    throw;
                }
            }
            else
            {
                _logger.LogError($"Invalid line: {line} in package: {currentPackage?.PackageId}");
                throw new FormatException($"Invalid line structure: {line} in package: {currentPackage?.PackageId}");
            }
        }

        if (currentPackage != null) yield return currentPackage;
    }


    private bool IsPackage(string[] parts) => parts.Length == 3 && parts[0] == _options.Delimeters.Package;

    private bool IsItem(string[] parts) => parts.Length == 4 && parts[0] == _options.Delimeters.Item;

    private Package ParsePackage(string[] parts)
    {
        if (!IsPackage(parts))
        {
            throw new FormatException($"Invalid package header format: {string.Join(' ', parts)}");
        }

        return new Package
        {
            SupplierId = parts[1],
            PackageId = parts[2],
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };
    }

    private Item ParseItem(string[] parts)
    {
        if (!IsItem(parts))
        {
            throw new FormatException($"Invalid item line format: {string.Join(' ', parts)}");
        }

        if (!int.TryParse(parts[3], out var quantity))
        {
            throw new FormatException($"Invalid quantity value: {parts[3]}");
        }

        return new Item
        {
            PoNumber = parts[1],
            Barcode = parts[2],
            Quantity = quantity,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };
    }
}
