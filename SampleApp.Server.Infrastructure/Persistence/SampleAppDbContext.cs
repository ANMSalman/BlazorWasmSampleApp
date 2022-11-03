using Microsoft.EntityFrameworkCore;
using SampleApp.Server.Domain.Entities;
using SampleApp.Server.Domain.Enums;

namespace SampleApp.Server.Infrastructure.Persistence;
internal class SampleAppDbContext : DbContext
{
    public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(e => e.ProductType)
            .HasConversion(
                value => value.ToString(),
                value => (ProductTypes)Enum.Parse(typeof(ProductTypes), value));
    }
}
