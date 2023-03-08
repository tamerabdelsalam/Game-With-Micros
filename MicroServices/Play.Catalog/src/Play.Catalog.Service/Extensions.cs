using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Enities;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedOn);
        }
    }
}
