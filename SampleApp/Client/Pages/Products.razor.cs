using Microsoft.AspNetCore.Components;
using MudBlazor;
using SampleApp.Client.Application.Managers;
using SampleApp.Client.Components;
using SampleApp.Client.Components.Products;
using SampleApp.Shared.RequestModels.Products;

namespace SampleApp.Client.Pages;

public partial class Products
{
    [Inject] public IProductsManager ProductsManager { get; set; }

    private bool _isLoading = true;

    private const int _pageSize = 5;

    protected override async Task OnInitializedAsync()
    {
        await ProductsManager.LoadProductsAsync(1, _pageSize);
        _isLoading = false;
    }

    private async Task OnPageChange(int page)
    {
        _isLoading = true;

        await ProductsManager.LoadProductsAsync(page, _pageSize);

        _isLoading = false;
    }

    private async Task OnRefresh()
    {
        _isLoading = true;

        await ProductsManager.LoadProductsAsync(ProductsManager.CurrentPage, _pageSize);

        _isLoading = false;
    }

    private async Task OnCreateNew()
    {
        var dialog = DialogService.Show<ProductCreationDialog>("Create new Product");
        var result = await dialog.Result;

        if (result.Cancelled)
            return;

        _isLoading = true;

        var model = result.Data as CreateProductRequestModel;

        await ProductsManager.CreateProductAsync(model);

        _isLoading = false;

        SnackBar.Add("Product created successfully", Severity.Success);
    }

    private async Task OnEditBasicDetails(int productId)
    {
        var parameters = new DialogParameters { { "ProductId", productId } };

        var dialog = DialogService.Show<ProductUpdateDialog>("Update Product Info", parameters);
        var result = await dialog.Result;

        if (result.Cancelled)
            return;

        _isLoading = true;

        var model = result.Data as UpdateProductBasicInfoRequestModel;

        await ProductsManager.UpdateProductBasicInfoAsync(productId, model);

        _isLoading = false;

        SnackBar.Add("Product updated successfully", Severity.Success);
    }

    private async Task OnAddNewStock(int productId)
    {
        var dialog = DialogService.Show<ProductStockAddRemoveDialog>("Add New Stock");
        var result = await dialog.Result;

        if (result.Cancelled)
            return;

        var quantity = int.Parse(result.Data?.ToString() ?? string.Empty);

        _isLoading = true;

        await ProductsManager.AddNewStockAsync(productId, quantity);

        _isLoading = false;

        SnackBar.Add("Stock added successfully", Severity.Success);

    }

    private async Task OnRemoveFromStock(int productId)
    {
        var dialog = DialogService.Show<ProductStockAddRemoveDialog>("Remove From Stock");
        var result = await dialog.Result;

        if (result.Cancelled)
            return;

        var quantity = int.Parse(result.Data?.ToString() ?? string.Empty);

        _isLoading = true;

        await ProductsManager.RemoveFromStockAsync(productId, quantity);

        _isLoading = false;

        SnackBar.Add("Stock removed successfully", Severity.Success);

    }

    private async Task OnDelete(int productId, string name)
    {
        var parameters = new DialogParameters { { "ContentText", $"Are you sure you want to delete the following product, {productId} : {name}?" } };

        var dialog = DialogService.Show<DeleteConfirmationDialog>("Confirmation", parameters);
        var result = await dialog.Result;

        if (result.Cancelled)
            return;

        _isLoading = true;

        await ProductsManager.DeleteProductAsync(productId);

        _isLoading = false;

        SnackBar.Add("product deleted successfully", Severity.Success);

    }
}
