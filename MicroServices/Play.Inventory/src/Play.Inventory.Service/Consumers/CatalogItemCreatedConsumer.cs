using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Interfaces;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var catalogItemCreatedMessage = context.Message;

        var catalogItem = await _catalogItemRepository.GetAsync(catalogItemCreatedMessage.ItemId);

        if (catalogItem is not null)
        {
            return;
        }

        catalogItem = new CatalogItem
        {
            Id = catalogItemCreatedMessage.ItemId,
            Name = catalogItemCreatedMessage.Name,
            Description = catalogItemCreatedMessage.Description
        };

        await _catalogItemRepository.CreateAsync(catalogItem);
    }
}
