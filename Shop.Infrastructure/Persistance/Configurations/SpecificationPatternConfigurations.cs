using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Persistance.Configurations;
public sealed class SpecificationPatternConfigurations : IEntityTypeConfiguration<SpecificationPattern>
{
    public void Configure(EntityTypeBuilder<SpecificationPattern> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
    }
}
