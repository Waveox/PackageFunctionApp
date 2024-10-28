using PackageFunctionApp.Infrastructure.Database.Interceptors;
using Microsoft.EntityFrameworkCore;
using PackageFunctionApp.App.Models;

namespace PackageFunctionApp.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public DbSet<Package> Packages { get; set; }
    public DbSet<Item> Items { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(new TimestampsInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // assuming that SupplierId and PackageId gives unique composite key and that every package is unique (i.e. no same package with additional Items to add to a package)
        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(p => new { p.SupplierId, p.PackageId });
            entity.HasIndex(p => new { p.SupplierId, p.PackageId });

            entity.HasMany(p => p.Items)
                  .WithOne()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(i => i.Id);
            
            entity.HasIndex(i => i.PoNumber);
            entity.HasIndex(i => i.Barcode);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}