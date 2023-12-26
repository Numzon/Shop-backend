using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Persistance.Configurations;
public sealed class SpecificationTypeConfigurations : IEntityTypeConfiguration<SpecificationType>
{
    public void Configure(EntityTypeBuilder<SpecificationType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Subtypes)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
