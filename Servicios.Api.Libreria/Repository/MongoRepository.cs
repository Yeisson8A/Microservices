using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Servicios.Api.Libreria.Core;
using Servicios.Api.Libreria.Core.Entities;
using System.Linq.Expressions;

namespace Servicios.Api.Libreria.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IOptions<MongoSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.Database);
            _collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }

        public async Task<IEnumerable<TDocument>> GetAll()
        {
            return await _collection.Find(p => true).ToListAsync();
        }

        public async Task<TDocument> GetById(string id)
        {
            return await _collection.Find(p => p.Id == id).SingleOrDefaultAsync();
        }

        public async Task InsertDocument(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task UpdateDocument(TDocument document)
        {
            await _collection.FindOneAndReplaceAsync(p => p.Id == document.Id, document);
        }

        public async Task DeleteById(string id)
        {
            await _collection.FindOneAndDeleteAsync(p => p.Id == id);
        }

        public async Task<PaginationEntity<TDocument>> GetPagination(PaginationEntity<TDocument> pagination)
        {
            var totalDocuments = 0;
            var nextPage = (pagination.Page - 1) * pagination.PageSize;
            var sort = pagination.SortDirection == "desc" ? Builders<TDocument>.Sort.Descending(pagination.Sort) : Builders<TDocument>.Sort.Ascending(pagination.Sort);

            if (pagination.FilterValue == null)
            {
                pagination.Data = await _collection.Find(p => true).Sort(sort).Skip(nextPage).Limit(pagination.PageSize).ToListAsync();
                totalDocuments = (await _collection.Find(p => true).ToListAsync()).Count;
            }
            else
            {
                var valueFilter = ".*" + pagination.FilterValue.Valor + ".*";
                var filter = Builders<TDocument>.Filter.Regex(pagination.FilterValue.Propiedad, new BsonRegularExpression(valueFilter, "i"));
                pagination.Data = await _collection.Find(filter).Sort(sort).Skip(nextPage).Limit(pagination.PageSize).ToListAsync();
                totalDocuments = (await _collection.Find(filter).ToListAsync()).Count;
            }

            var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocuments / pagination.PageSize)));
            pagination.PagesQuantity = totalPages;
            pagination.TotalRows = totalDocuments;
            return pagination;
        }
    }
}
