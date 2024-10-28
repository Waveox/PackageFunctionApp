using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PackageFunctionApp.Configurations;
using PackageFunctionApp.Infrastructure.Database;
using PackageFunctionApp.Services;
using PackageFunctionApp.Services.Implementations;
using Serilog;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, builder) =>
    {
        string contentRootPath = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot") ?? context.HostingEnvironment.ContentRootPath;

        builder
            .SetBasePath(contentRootPath)
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.Configure<PackageFunctionOptions>(context.Configuration);
        services.AddFluentValidationAutoValidation();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(context.Configuration.GetConnectionString("DefaultSqliteConnection"));

            if (context.HostingEnvironment.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddTransient<IPackageParser, PackageParser>();
        services.AddTransient<IPackageService, PackageService>();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

    var databaseManager = new DatabaseManager(host.Services);
    databaseManager.Migrate();
    if (args.Contains("--seed"))
    {
        databaseManager.Seed();
        return;
    }

host.Run();