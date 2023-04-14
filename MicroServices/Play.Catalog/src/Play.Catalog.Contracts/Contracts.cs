using System;

namespace Play.Catalog.Contracts;

public class Contracts
{
    public record CatalogItemCreated(Guid ItemId, string Name, string Description);
    public record CatalogItemCUpdated(Guid ItemId, string Name, string Description);
    public record CatalogItemCDeleted(Guid ItemId);
}
