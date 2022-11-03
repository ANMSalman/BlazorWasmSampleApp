using Microsoft.AspNetCore.Mvc;
using SampleApp.Server.Application.Services;
using SampleApp.Shared.RequestModels.Products;

namespace SampleApp.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProductsAsync([FromQuery] int page, [FromQuery] int pageSize,
        CancellationToken cancellationToken)
    {
        var result = await _productService.GetAllProductsAsync(page, pageSize, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductByIdAsync([FromRoute] int productId,
        CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductByIdAsync(productId, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequestModel model,
        CancellationToken cancellationToken)
    {
        var result = await _productService.CreateProductAsync(model, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{productId}/basic-details")]
    public async Task<IActionResult> UpdateProductBasicInfoAsync([FromRoute] int productId, [FromBody] UpdateProductBasicInfoRequestModel model,
        CancellationToken cancellationToken)
    {
        await _productService.UpdateProductBasicInfoAsync(productId, model, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{productId}/available-stock/add/{quantity}")]
    public async Task<IActionResult> AddNewStockAsync([FromRoute] int productId, [FromRoute] int quantity,
        CancellationToken cancellationToken)
    {
        await _productService.AddNewStockAsync(productId, quantity, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{productId}/available-stock/remove/{quantity}")]
    public async Task<IActionResult> RemoveFromStockAsync([FromRoute] int productId, [FromRoute] int quantity,
        CancellationToken cancellationToken)
    {
        await _productService.RemoveFromStockAsync(productId, quantity, cancellationToken);

        return NoContent();
    }
}
