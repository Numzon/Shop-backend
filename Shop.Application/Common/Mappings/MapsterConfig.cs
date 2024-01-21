using Mapster;
using Nest;
using Shop.Application.Category.Commands.CreateCategory;
using Shop.Application.Category.Commands.EditCategory;
using Shop.Application.Category.Models;
using Shop.Application.Products.Models;
using Shop.Application.SpecificationPatterns.Commands;
using Shop.Application.SpecificationPatterns.Models;
using Shop.Application.SpecificationTypes.Commands.CreateSpecificationType;
using Shop.Application.SpecificationTypes.Commands.EditSpecificationType;
using Shop.Application.SpecificationTypes.Models;
using Shop.Domain.ElasticsearchEntities;
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
        config = GetSpecificationPatternConfiguration(config);
        config = GetProductConfiguration(config);

        return config;
    }

    public static TypeAdapterConfig GetCategoryConfiguration(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryCommand, ProductCategory>()
           .Map(dest => dest.Subcategories, src => src.Subcategories);

        config.NewConfig<EditCategoryCommand, ProductCategory>()
            .Ignore(src => src.Subcategories);

        config.NewConfig<ProductCategory, CategoryDto>()
           .Map(dest => dest.Subcategories, src => src.Subcategories)
           .Map(dest => dest.HasSubcategories, src => src.Subcategories.Count > 0)
           .Map(dest => dest.ParentCategory, src => src.ParentCategory)
           .Map(dest => dest.SpecificationPattern, src => src.SpecificationPattern);

        return config;
    }

    public static TypeAdapterConfig GetSpecificationTypeConfiguration(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSpecificationTypeCommand, SpecificationType>()
           .Map(dest => dest.Subtypes, src => src.Subtypes);

        config.NewConfig<EditSpecificationTypeCommand, SpecificationType>()
            .Ignore(src => src.Subtypes);

        config.NewConfig<SpecificationType, SpecificationTypeDto>()
            .Map(dest => dest.Subtypes, src => src.Subtypes)
            .Map(dest => dest.Parent, src => src.Parent);

        return config;
    }

    public static TypeAdapterConfig GetSpecificationPatternConfiguration(TypeAdapterConfig config)
    {
        config.NewConfig<SpecificationPattern, SpecificationPatternDto>()
            .Map(dest => dest.Types, src => src.SpecificationPatternSpecificationTypes);

        return config;
    }

    public static TypeAdapterConfig GetProductConfiguration(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.Category, src => src.Category);

        return config;
    }
}
