using MediatR;
using Shop.Application.Category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Category.Queries;
public sealed class GetMainCategoriesQuery : IRequest<List<CategoryListItemDto>>
{
    public string? SearchString { get; set; }
}
