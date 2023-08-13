using Catalog.Application.Models;
using Catalog.Domain;
using Common.Data;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Catalog.Application.Queries
{
    public record GetProductsQuery(int Page) : IRequest<PagedResult<ProductDto[]>>;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto[]>>
    {
        private readonly IMongoDatabase _db;
        private readonly IOptions<PictureFileSettings> _options;

        public GetProductsQueryHandler(IMongoDatabase db, IOptions<PictureFileSettings> options)
        {
            _db = db;
            _options = options;
        }

        public async Task<PagedResult<ProductDto[]>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var collection = _db.GetCollection<Product>(Product.CollectionName);
            var query = collection.AsQueryable().OrderBy(x => x.Id);
            var products = await query
                .Skip((request.Page - 1) * PagedResult.PageSize)
                .Take(PagedResult.PageSize)
                .ToListAsync();
            var dtos = products
                .Select(product => new ProductDto
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    ImageUrl = _options.Value.ImageBaseUrl.Replace("[0]", product.Id.ToString()),
                    ImageFileName = product.ImageFileName,
                    Price = product.Price
                })
                .ToArray();

            var count = await query.CountAsync();

            return new PagedResult<ProductDto[]>
            {
                Data = dtos,
                TotalCount = count,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling((double)count / PagedResult.PageSize)
            };
        }
    }
}
