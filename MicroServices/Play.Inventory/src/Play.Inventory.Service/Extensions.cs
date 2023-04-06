using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service;

public static class Extensions
{
    public static InventoryItemDto AsDto(this InventoryItem invItem)
    {
        return new InventoryItemDto(invItem.CatalogItemId, invItem.Qunatity, invItem.AquiredDate);
    }
}
