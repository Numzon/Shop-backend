using Mapster;
using MediatR;
using Nest;
using Shop.Application.Products.Models;
using Shop.Application.Products.Queries;
using Shop.Domain.ElasticsearchEntities;

namespace Shop.Application.Products.Handlers;
public sealed class GetSearchBarHandler : IRequestHandler<GetSearchBarQuery, GetSearchBarResponse>
{
    private readonly IElasticClient _client;

    public GetSearchBarHandler(IElasticClient client)
    {
        _client = client;
    }

    public async Task<GetSearchBarResponse> Handle(GetSearchBarQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.SearchString))
        {
            return new GetSearchBarResponse();
        }

        var searchResponse = await _client.SearchAsync<ESProduct>(x => x
            .Query(query => query
                .QueryString(d => d
                    .Query("*" + request.SearchString + "*")
                    .Fields(f =>
                        f.Fields(z =>
                            z.Name,
                            z => z.Description)
                        )
                )
            )
            .Size(request.PageSize)
        );

        var products = searchResponse.Documents.ToList();

        if (products == null)
        {
            return new GetSearchBarResponse();
        }

        if (products.Count >= request.PageSize)
        {
            var items = products.Adapt<List<SearchBarItemDto>>();
            return new GetSearchBarResponse
            {
                Data = items
            };
        }

        var categoriesSearchBarItems = products
            .Select(x => x.Category)
            .DistinctBy(x => x.Id)
            .Take(request.PageSize - products.Count)
            .Select(x => new SearchBarItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsCategory = true
            }).ToList();

        var productsSearchBarItem = products.Adapt<List<SearchBarItemDto>>();
        productsSearchBarItem.AddRange(categoriesSearchBarItems);

        return new GetSearchBarResponse
        {
            Data = productsSearchBarItem
        };
    }
}
