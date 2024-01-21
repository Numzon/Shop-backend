using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Persistance.Configurations;
public sealed class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();

        builder.HasOne(e => e.ParentCategory)
           .WithMany(x => x.Subcategories)
           .HasForeignKey(e => e.ParentCategoryId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.SpecificationPattern)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.SpecificationPatternId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
