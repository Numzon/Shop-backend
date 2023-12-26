using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Persistance.Configurations;
public sealed class SpecificationPatternSpecificationTypeConfiguration : IEntityTypeConfiguration<SpecificationPatternSpecificationType>
{
    public void Configure(EntityTypeBuilder<SpecificationPatternSpecificationType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.SpecificationPattern)
            .WithMany(x => x.SpecificationPatternSpecificationTypes)
            .HasForeignKey(x => x.SpecificationPatternId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.SpecificationType)
            .WithMany(x => x.SpecificationPatternSpecificationTypes)
            .HasForeignKey(x => x.SpecificationTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
