using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Interfaces;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;
    public CatalogItemDeletedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var catalogItemDeletedMessage = context.Message;

        var catalogItem = await _catalogItemRepository.GetAsync(catalogItemDeletedMessage.ItemId);

        if (catalogItem is null)
        {
            return;
        }

        await _catalogItemRepository.RemoveAsync(catalogItemDeletedMessage.ItemId);
    }
}
