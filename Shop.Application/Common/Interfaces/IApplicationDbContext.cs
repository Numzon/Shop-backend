using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ProductCategory> Categories { get; set; }
    public DbSet<SpecificationPattern> SpecificationPatterns { get; set; }
    public DbSet<SpecificationType> SpecificationTypes { get; set; }
    public DbSet<SpecificationPatternSpecificationType> SpecificationPatternSpecificationTypes { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
