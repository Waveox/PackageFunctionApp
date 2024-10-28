using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PackageFunctionApp.Configurations;
using PackageFunctionApp.Services;

namespace PackageFunctionApp.Jobs;
public class ProcessPackage
{
    private readonly ILogger<ProcessPackage> _logger;
    private readonly IPackageParser _parser;
    private readonly IPackageService _packageService;
    private readonly PackageFunctionOptions _options;

    public ProcessPackage(IPackageParser parser, IPackageService packageService, IOptions<PackageFunctionOptions> options, ILogger<ProcessPackage> logger)
    {
        _logger = logger;
        _parser = parser;
        _options = options.Value;
        _packageService = packageService;
    }

    [Function("ProcessPackage")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timer)
    {
        // this should be actual shared ftp location and extensions should be improved to an allowed list
        var files = Directory.GetFiles(_options.FolderStructure.Shared, _options.Files.AllowedExtensions);

        foreach (var filePath in files)
        {
            var fileName = Path.GetFileName(filePath);
            var processingFilePath = Path.Combine(_options.FolderStructure.Processing, fileName);

            if (File.Exists(filePath + _options.Files.LockFileExtension))
            {
                _logger.LogInformation($"Skipping {fileName} - locked by another process.");
                continue;
            }

            try
            {
                using (File.Create(filePath + _options.Files.LockFileExtension)) { }
                File.Move(filePath, processingFilePath);
                _logger.LogInformation($"Processing {fileName}");

                await foreach (var package in _parser.ParseAsync(processingFilePath))
                {
                    await _packageService.StorePackageAsync(package);
                }

                File.Move(processingFilePath, Path.Combine(_options.FolderStructure.Processed, fileName));
                _logger.LogInformation($"Processed {fileName}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing {fileName}: {ex.Message}");
            }
            finally
            {
                File.Delete(filePath + _options.Files.LockFileExtension);
            }
        }
    }
}
