using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Interfaces;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var catalogItemUpdatedMessage = context.Message;

        var catalogItem = await _catalogItemRepository.GetAsync(catalogItemUpdatedMessage.ItemId);

        if (catalogItem is null)
        {
            // Create new Item
            catalogItem = new CatalogItem
            {
                Id = catalogItemUpdatedMessage.ItemId,
                Name = catalogItemUpdatedMessage.Name,
                Description = catalogItemUpdatedMessage.Description
            };

            await _catalogItemRepository.CreateAsync(catalogItem);

        }
        else // Update Item
        {
            catalogItem.Name = catalogItemUpdatedMessage.Name;
            catalogItem.Description = catalogItemUpdatedMessage.Description;

            await _catalogItemRepository.UpdateAsync(catalogItem);
        }
    }
}
