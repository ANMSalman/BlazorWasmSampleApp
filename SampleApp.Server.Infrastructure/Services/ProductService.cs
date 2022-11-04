using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SampleApp.Server.Application.Common;
using SampleApp.Server.Application.Exceptions;
using SampleApp.Server.Application.Services;
using SampleApp.Server.Domain.Entities;
using SampleApp.Server.Domain.Enums;
using SampleApp.Server.Infrastructure.Constants;
using SampleApp.Server.Infrastructure.Persistence;
using SampleApp.Shared.DTOs.Products;
using SampleApp.Shared.RequestModels.Products;
using SampleApp.Shared.ResponseModels.Products;

namespace SampleApp.Server.Infrastructure.Services;
internal class ProductService : IProductService
{
    private readonly SampleAppDbContext _context;
    private readonly IImageUploader _imageUploader;
    private readonly IConfiguration _configuration;
    public ProductService(SampleAppDbContext context, IImageUploader imageUploader, IConfiguration configuration)
    {
        _context = context;
        _imageUploader = imageUploader;
        _configuration = configuration;
    }

    private ProductTypes GetProductTypeFromString(string value)
    {
        var productType = value switch
        {
            "Cloth" => ProductTypes.Cloth,
            "Food" => ProductTypes.Food,
            "Sport" => ProductTypes.Sport,
            _ => throw new AppException("Invalid Product Type")
        };

        return productType;
    }

    public async Task<int> CreateProductAsync(CreateProductRequestModel model, CancellationToken cancellationToken = default)
    {
        var imageName = await _imageUploader.UploadImageAsync(model.Base64ImageData, cancellationToken);

        var product = new Product(model.Name, GetProductTypeFromString(model.ProductType), model.AvailableStock,
            imageName);

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    public async Task UpdateProductBasicInfoAsync(int productId, UpdateProductBasicInfoRequestModel model, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId && !x.IsDeleted, cancellationToken: cancellationToken);

        if (product is null)
            throw new AppException("Invalid Product Id. Product not found");

        var productOldImage = product.ImageName;
        var productNewImageName = product.ImageName;

        if (!string.IsNullOrEmpty(model.Base64ImageData))
        {
            productNewImageName = await _imageUploader.UploadImageAsync(model.Base64ImageData, cancellationToken);
        }

        product.UpdateBasicInfo(model.Name, GetProductTypeFromString(model.ProductType), productNewImageName);

        await _context.SaveChangesAsync(cancellationToken);

        await _imageUploader.DeleteImageAsync(productOldImage, cancellationToken);
    }

    public async Task AddNewStockAsync(int productId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId && !x.IsDeleted, cancellationToken: cancellationToken);

        if (product is null)
            throw new AppException("Invalid Product Id. Product not found");

        product.AddNewStock(quantity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveFromStockAsync(int productId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId && !x.IsDeleted, cancellationToken: cancellationToken);

        if (product is null)
            throw new AppException("Invalid Product Id. Product not found");

        product.RemoveFromStock(quantity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetAllProductsDto> GetAllProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        page = (page <= 0) ? 1 : page;
        pageSize = (pageSize <= 0) ? CommonConstants.DefaultPageSize : pageSize;

        var query = _context.Products
            .Where(x => !x.IsDeleted)
            .Select(x => new ProductInfoResponseModel()
            {
                ProductId = x.Id,
                Name = x.Name,
                ProductType = x.ProductType.ToString(),
                AvailableStock = x.AvailableStock,
                SellableStock = x.SellableStock,
                ImageUrl = _configuration[CommonConstants.ImageStorageBasePathConfigName] + x.ImageName
            });

        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);
        var products = await query
            .OrderBy(x => x.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetAllProductsDto()
        {
            TotalAvailableRecords = totalCount,
            Products = products
        };
    }

    public async Task<ProductInfoResponseModel> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products
            .Where(x => x.Id == productId && !x.IsDeleted)
            .Select(x => new ProductInfoResponseModel()
            {
                ProductId = x.Id,
                Name = x.Name,
                ProductType = x.ProductType.ToString(),
                AvailableStock = x.AvailableStock,
                SellableStock = x.SellableStock,
                ImageUrl = _configuration[CommonConstants.ImageStorageBasePathConfigName] + x.ImageName
            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (product is null)
            throw new AppException("Invalid Product Id. Product not found");

        return product;
    }

    public async Task DeleteProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId && !x.IsDeleted, cancellationToken: cancellationToken);

        if (product is null)
            throw new AppException("Invalid Product Id. Product not found");

        product.MarkAsDeleted();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
