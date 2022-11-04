using SampleApp.Shared.DTOs.Products;
using SampleApp.Shared.RequestModels.Products;
using SampleApp.Shared.ResponseModels.Products;

namespace SampleApp.Server.Application.Services;
public interface IProductService
{
    Task<int> CreateProductAsync(CreateProductRequestModel model, CancellationToken cancellationToken = default);
    Task UpdateProductBasicInfoAsync(int productId, UpdateProductBasicInfoRequestModel model, CancellationToken cancellationToken = default);
    Task AddNewStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);
    Task RemoveFromStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);
    Task<GetAllProductsDto> GetAllProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ProductInfoResponseModel> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(int productId, CancellationToken cancellationToken = default);

}
