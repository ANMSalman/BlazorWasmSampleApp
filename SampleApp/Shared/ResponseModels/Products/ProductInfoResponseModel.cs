namespace SampleApp.Shared.ResponseModels.Products;
public class ProductInfoResponseModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string ProductType { get; set; }
    public int AvailableStock { get; set; }
    public int SellableStock { get; set; }
    public string ImageUrl { get; set; }
}
