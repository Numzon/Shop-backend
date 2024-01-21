using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Extensions;
using Shop.Infrastructure.Persistance.Interceptors;
using System.Reflection;

namespace Shop.Infrastructure.Persistance;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly BaseAuditableEntitySaveChangesInterceptor _baseAuditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        BaseAuditableEntitySaveChangesInterceptor baseAuditableEntitySaveChangesInterceptor) : base(options)
	{
        _mediator = mediator;
        _baseAuditableEntitySaveChangesInterceptor = baseAuditableEntitySaveChangesInterceptor;
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ProductCategory> Categories { get; set; }
    public DbSet<SpecificationPattern> SpecificationPatterns { get; set; }
    public DbSet<SpecificationType> SpecificationTypes { get; set; }
    public DbSet<SpecificationPatternSpecificationType> SpecificationPatternSpecificationTypes { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_baseAuditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
