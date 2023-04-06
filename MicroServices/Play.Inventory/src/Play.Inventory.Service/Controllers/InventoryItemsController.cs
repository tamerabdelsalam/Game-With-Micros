using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Interfaces;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[Route("invItems")]
public class InventoryItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _invItemsRepository;

    public InventoryItemsController(IRepository<InventoryItem> invItemsRepository)
    {
        _invItemsRepository = invItemsRepository;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }

        var results = (await _invItemsRepository.GetAllAsync(invItem => invItem.UserId == userId))
                     .Select(invItem => invItem.AsDto());

        return Ok(results);
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

