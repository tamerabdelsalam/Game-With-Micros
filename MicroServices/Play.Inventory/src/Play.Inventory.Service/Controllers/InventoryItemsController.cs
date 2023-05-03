using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Interfaces;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("invItems")]
public class InventoryItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _invItemsRepository;
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public InventoryItemsController(IRepository<InventoryItem> invItemsRepository, IRepository<CatalogItem> catalogItemRepository)
    {
        _invItemsRepository = invItemsRepository;
        _catalogItemRepository = catalogItemRepository;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }

        var userInvItems = await _invItemsRepository.GetAllAsync(invItem => invItem.UserId == userId);
        var catalogItemsIds = userInvItems.Select(userInvItem => userInvItem.CatalogItemId).ToList();

        var userCatalogItems = await _catalogItemRepository.GetAllAsync(catalogItem => catalogItemsIds.Contains(catalogItem.Id));

        var invItemDtos = userInvItems.Select(userInvItem =>
        {
            var catalogItem = userCatalogItems.Single(userCatalogItem => userCatalogItem.Id == userInvItem.CatalogItemId);

            return userInvItem.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(invItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> PostAsync(GrantItemsDto grantItemsDto)
    {
        if (grantItemsDto is null)
        {
            return BadRequest();
        }

        var existingInvItem = (await _invItemsRepository.GetAsync(invItem => invItem.UserId == grantItemsDto.UserId
                                                                           && invItem.CatalogItemId == grantItemsDto.CatalogItemId));

        if (existingInvItem is null)
        {
            var invItem = new InventoryItem
            {
                Id = Guid.NewGuid(),
                CatalogItemId = grantItemsDto.CatalogItemId,
                Quantity = grantItemsDto.Quantity,
                AquiredDate = DateTimeOffset.UtcNow,
                UserId = grantItemsDto.UserId
            };

            await _invItemsRepository.CreateAsync(invItem);

            return Ok(invItem);
        }

        existingInvItem.CatalogItemId = grantItemsDto.CatalogItemId;
        existingInvItem.Quantity = grantItemsDto.Quantity;

        await _invItemsRepository.UpdateAsync(existingInvItem);

        return NoContent();
    }
}

