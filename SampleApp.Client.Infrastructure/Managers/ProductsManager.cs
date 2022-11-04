using SampleApp.Client.Application.Exceptions;
using SampleApp.Client.Application.Managers;
using SampleApp.Client.Infrastructure.Constants;
using SampleApp.Shared.DTOs.Products;
using SampleApp.Shared.RequestModels.Products;
using SampleApp.Shared.ResponseModels.Products;
using System.Net.Http.Json;

namespace SampleApp.Client.Infrastructure.Managers;
internal class ProductsManager : ManagerBase<ProductInfoResponseModel>, IProductsManager
{
    private readonly HttpClient _httpClient;

    public ProductsManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task LoadProductsAsync(int page, int pageSize)
    {
        if (page <= 0)
            throw new ClientException("Invalid Page. Page must be greater than 0");

        if (pageSize <= 0)
            throw new ClientException("Invalid Page Size. Page Size must be greater than 0");

        var response =
            await _httpClient.GetAsync(
                RouteConstants.ProductsRoutes.GetAllProducts(page, pageSize));

        if (!response.IsSuccessStatusCode)
            await HandleError(response);

        var result = await response.Content.ReadFromJsonAsync<GetAllProductsDto>();

        Data = result.Products;
        CurrentPage = page;
        PageSize = pageSize;
        TotalAvailableRecords = result.TotalAvailableRecords;
        HasMoreRecords = (page * pageSize) < TotalAvailableRecords;
    }

    public async Task<ProductInfoResponseModel> GetProductByIdAsync(int productId)
    {
        if (productId <= 0)
            throw new ClientException("Invalid Product Id");

        var response =
            await _httpClient.GetAsync(
                RouteConstants.ProductsRoutes.GetProductById(productId));

        if (!response.IsSuccessStatusCode)
            await HandleError(response);

        var result = await response.Content.ReadFromJsonAsync<ProductInfoResponseModel>();

        return result;

    }

    public async Task CreateProductAsync(CreateProductRequestModel model)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                RouteConstants.ProductsRoutes.CreateProduct,
                model);

        if (!response.IsSuccessStatusCode)
            await HandleError(response);

        await LoadProductsAsync(CurrentPage, PageSize);
    }

    public async Task UpdateProductBasicInfoAsync(int productId, UpdateProductBasicInfoRequestModel model)
    {
        var response =
            await _httpClient.PutAsJsonAsync(
                RouteConstants.ProductsRoutes.UpdateProductBasicDetails(productId),
                model);

        if (!response.IsSuccessStatusCode)
            await HandleError(response);

        await LoadProductsAsync(CurrentPage, PageSize);
    }

    public async Task AddNewStockAsync(int productId, int quantity)
    {
        var response =
            await _httpClient.PatchAsync(
                RouteConstants.ProductsRoutes.AddNewStock(productId, quantity),
                default);

        if (!response.IsSuccessStatusCode)
            await HandleError(response);

        await LoadProductsAsync(CurrentPage, PageSize);
    }

    public async Task RemoveFromStockAsync(int productId, int quantity)
    {
        var response =
            await _httpClient.PatchAsync(
                RouteConstants.ProductsRoutes.RemoveFromStock(productId, quantity),
                default);

        if (!response.IsSuccessStatusCode)
            await HandleError(response);

        await LoadProductsAsync(CurrentPage, PageSize);
    }

    public async Task DeleteProductAsync(int productId)
    {
        var response =
            await _httpClient.DeleteAsync(
                RouteConstants.ProductsRoutes.DeleteProduct(productId));

        if (!response.IsSuccessStatusCode)
            await HandleError(response);

        await LoadProductsAsync(CurrentPage, PageSize);
    }
}
