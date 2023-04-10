using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Clients;

public class CatalogClient
{
    private readonly HttpClient _httpCLient;

    public CatalogClient(HttpClient httpCLient)
    {
        _httpCLient = httpCLient;
    }

    public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync()
    {
        var catalogItemDtos = await _httpCLient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");

        return catalogItemDtos;
    }
}
