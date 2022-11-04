using SampleApp.Shared.RequestModels.Products;
using SampleApp.Shared.ResponseModels.Products;

namespace SampleApp.Client.Application.Managers;
public interface IProductsManager : IManagerBase<ProductInfoResponseModel>
{
    Task LoadProductsAsync(int page, int pageSize);
    Task<ProductInfoResponseModel> GetProductByIdAsync(int productId);
    Task CreateProductAsync(CreateProductRequestModel model);
    Task UpdateProductBasicInfoAsync(int productId, UpdateProductBasicInfoRequestModel model);
    Task AddNewStockAsync(int productId, int quantity);
    Task RemoveFromStockAsync(int productId, int quantity);
}
