using System;
using Play.Common.Interfaces;

namespace Play.Inventory.Service.Entities;

public class InventoryItem : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CatalogItemId { get; set; }
    public int Qunatity { get; set; }
    public DateTimeOffset AquiredDate { get; set; }
}
