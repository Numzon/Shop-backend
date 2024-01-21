using Shop.Application.SpecificationTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.SpecificationPatterns.Models;
public class SimpleSpecificationPatternDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
