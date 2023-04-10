using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Interfaces;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("invItems")]
public class InventoryItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _invItemsRepository;
    private readonly CatalogClient _catalogClient;

    public InventoryItemsController(IRepository<InventoryItem> invItemsRepository, CatalogClient catalogClient)
    {
        _invItemsRepository = invItemsRepository;
        _catalogClient = catalogClient;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }

        var catalogItemDtos = await _catalogClient.GetCatalogItemsAsync();

        var invItems = (await _invItemsRepository.GetAllAsync(invItem => invItem.UserId == userId));

        var invItemDtos = invItems.Select(invItem =>
        {
            var catalogItem = catalogItemDtos.Single(catalogItemDto => catalogItemDto.Id == invItem.CatalogItemId);

            return invItem.AsDto(catalogItem.Name, catalogItem.Description);
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
                Qunatity = grantItemsDto.Qunatity,
                AquiredDate = DateTimeOffset.UtcNow,
                UserId = grantItemsDto.UserId
            };

            await _invItemsRepository.CreateAsync(invItem);

            return Ok(invItem);
        }

        existingInvItem.CatalogItemId = grantItemsDto.CatalogItemId;
        existingInvItem.Qunatity = grantItemsDto.Qunatity;

        await _invItemsRepository.UpdateAsync(existingInvItem);

        return NoContent();
    }
}

