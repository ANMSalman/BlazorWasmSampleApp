using SampleApp.Server.Domain.Constants;
using SampleApp.Server.Domain.Enums;
using SampleApp.Server.Domain.Exceptions;

namespace SampleApp.Server.Domain.Entities;
public class Product
{
    public Product(string name, ProductTypes productType, int availableStock, string imageName)
    {
        Name = name;
        ProductType = productType;
        AvailableStock = availableStock;
        SellableStock = availableStock - ProductConstants.AvailableAndSellableQtyVariance;
        ImageName = imageName;

        ValidateSellableQtyAndThrowIfInvalid();
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public ProductTypes ProductType { get; private set; }
    public int AvailableStock { get; private set; }
    public string ImageName { get; private set; }
    public int SellableStock { get; private set; }

    private void ValidateSellableQtyAndThrowIfInvalid()
    {
        if (SellableStock <= 0)
            throw new DomainException(
                $"Available stock must be greater than {ProductConstants.AvailableAndSellableQtyVariance}");
    }

    public void UpdateBasicInfo(string name, ProductTypes productType, string imageName)
    {
        Name = name;
        ProductType = productType;
        ImageName = imageName;
    }

    public void AddNewStock(int count)
    {
        AvailableStock += count;
        SellableStock = AvailableStock - ProductConstants.AvailableAndSellableQtyVariance;

        ValidateSellableQtyAndThrowIfInvalid();
    }

    public void RemoveFromStock(int count)
    {
        AvailableStock -= count;
        SellableStock = AvailableStock - ProductConstants.AvailableAndSellableQtyVariance;

        ValidateSellableQtyAndThrowIfInvalid();
    }

}
