using Mapster;
using Shop.Application.Category.Commands.CreateCategory;
using Shop.Application.Category.Commands.EditCategory;
using Shop.Application.Category.Models;
using Shop.Application.SpecificationTypes.Commands.CreateSpecificationType;
using Shop.Application.SpecificationTypes.Commands.EditSpecificationType;
using Shop.Application.SpecificationTypes.Models;
using Shop.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Common.Mappings;

[ExcludeFromCodeCoverage]
public static class MapsterConfig
{
    public static TypeAdapterConfig GetGlobalSettingsConfiguration()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config = GetCategoryConfiguration(config);
        config = GetSpecificationTypeConfiguration(config);

        return config;
    }

    public static TypeAdapterConfig GetCategoryConfiguration(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryCommand, ProductCategory>()
           .Map(dest => dest.Subcategories, src => src.Subcategories);

        config.NewConfig<ProductCategory, CategoryDto>()
            .Map(dest => dest.ParentCategoryId, src => src.ParentCategory != null ? src.ParentCategory.Id : Guid.Empty)
            .Map(dest => dest.ParentCategoryName, src => src.ParentCategory != null ? src.ParentCategory.Name : null)
            .Map(dest => dest.Subcategories, src => src.Subcategories);

        config.NewConfig<EditCategoryCommand, ProductCategory>()
            .Ignore(src => src.Subcategories);

        return config;
    }

    public static TypeAdapterConfig GetSpecificationTypeConfiguration(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSpecificationTypeCommand, SpecificationType>()
           .Map(dest => dest.Subtypes, src => src.Subtypes);

        config.NewConfig<SpecificationType, SpecificationTypeDto>()
            .Map(dest => dest.ParentId, src => src.Parent != null ? src.Parent.Id : Guid.Empty)
            .Map(dest => dest.ParentName, src => src.Parent != null ? src.Parent.Name : null)
            .Map(dest => dest.Subtypes, src => src.Subtypes);

        config.NewConfig<EditSpecificationTypeCommand, SpecificationType>()
            .Ignore(src => src.Subtypes);

        return config;
    }
}
