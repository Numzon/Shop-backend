using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ElasticsearchEntities;
public class ESCategory
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
