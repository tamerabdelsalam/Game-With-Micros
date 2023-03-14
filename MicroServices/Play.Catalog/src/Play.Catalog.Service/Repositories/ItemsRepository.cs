using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Enities;

namespace Play.Catalog.Service.Repositories;

public class ItemsRepository : IItemsRepository
{
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> dbCollection;
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

    public ItemsRepository(IMongoDatabase database)
    {      
        dbCollection = database.GetCollection<Item>(collectionName);
    }

    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> GetAsync(Guid id)
    {
        var filter = filterBuilder.Where(m => m.Id == id);

        return await dbCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task CreateAsync(Item entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await dbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Item entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var filter = filterBuilder.Where(m => m.Id == entity.Id);
        await dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        var filter = filterBuilder.Where(m => m.Id == id);
        await dbCollection.DeleteOneAsync(filter);
    }

}