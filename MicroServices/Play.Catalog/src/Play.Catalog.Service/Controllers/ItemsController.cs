using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
           new ItemDto(Guid.NewGuid(),"Aly","Restores a small amount of HP",5,DateTimeOffset.UtcNow),
           new ItemDto(Guid.NewGuid(),"Samir","Restores a small amount of HP",7,DateTimeOffset.UtcNow),
           new ItemDto(Guid.NewGuid(),"Hatem","Restores a small amount of HP",20,DateTimeOffset.UtcNow),
        };

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public ItemDto GetById(Guid id)
        {
            return items.SingleOrDefault(i => i.Id == id);
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var item = GetById(id);

            var updatedItem = item with
            {
                Description = updateItemDto.Description,
                Name = updateItemDto.Name,
                Price = updateItemDto.Price
            };

            item = updatedItem;

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var item = GetById(id);

            items.Remove(item);

            return NoContent();
        }
    }
}