using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Enities;
using Play.Common.Interfaces;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private static int _requestsCounter = 0;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        // GET /items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            _requestsCounter++;
            Console.WriteLine($"Request {_requestsCounter}: starting...");

            if (_requestsCounter <= 2)
            {
                Console.WriteLine($"Request {_requestsCounter}: Delaying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            if (_requestsCounter <= 4)
            {
                Console.WriteLine($"Request {_requestsCounter}: 500 (Internal Server Error).");
                return StatusCode(500);
            }

            var itemDtos = (await _itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            Console.WriteLine($"Request {_requestsCounter}: 200 (OK).");

            return Ok(itemDtos);
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            return item is null ? NotFound() : item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedOn = DateTimeOffset.UtcNow
            };

            await _itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item is null)
            {
                item = new()
                {
                    Id = Guid.NewGuid(),
                    Name = updateItemDto.Name,
                    Description = updateItemDto.Description,
                    Price = updateItemDto.Price,
                    CreatedOn = DateTimeOffset.UtcNow
                };

                return CreatedAtAction(nameof(GetByIdAsync), new { id = id }, item);
            }

            item.Name = updateItemDto.Name;
            item.Description = updateItemDto.Description;
            item.Price = updateItemDto.Price;

            await _itemsRepository.UpdateAsync(item);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            await _itemsRepository.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}