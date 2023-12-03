using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
    