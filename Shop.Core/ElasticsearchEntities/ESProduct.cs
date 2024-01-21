namespace Shop.Domain.ElasticsearchEntities;
public class ESProduct
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public required ESCategory Category { get; set; }
}
