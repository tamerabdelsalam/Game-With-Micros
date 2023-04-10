using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Dtos
{
    public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Qunatity);

    public record InventoryItemDto(Guid CatalogItemId,string Name, string Description, int Qunatity, DateTimeOffset AquiredDate);

    public record CatalogItemDto(Guid Id, string Name, string Description);
}