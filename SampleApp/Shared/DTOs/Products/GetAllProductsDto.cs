using SampleApp.Shared.ResponseModels.Products;

namespace SampleApp.Shared.DTOs.Products;
public class GetAllProductsDto
{
    public int TotalAvailableRecords { get; set; }
    public List<ProductInfoResponseModel> Products { get; set; }
}
