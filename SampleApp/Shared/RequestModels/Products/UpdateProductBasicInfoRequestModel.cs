﻿namespace SampleApp.Shared.RequestModels.Products;
public class UpdateProductBasicInfoRequestModel
{
    public string Name { get; set; }
    public string ProductType { get; set; }
    public int AvailableStock { get; set; }
    public string Base64ImageData { get; set; }
}
