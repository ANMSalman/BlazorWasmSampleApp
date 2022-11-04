namespace SampleApp.Client.Infrastructure.Constants;
internal static class RouteConstants
{
    private const string BasePath = "api";
    internal static class ProductsRoutes
    {
        private const string BaseEndpoint = $"{BasePath}/Products";

        internal static string GetAllProducts(int page, int pageSize) =>
            $"{BaseEndpoint}?page={page}&pageSize={pageSize}";

        internal static string GetProductById(int productId) =>
            $"{BaseEndpoint}/{productId}";

        internal static string DeleteProduct(int productId) =>
            $"{BaseEndpoint}/{productId}";

        internal const string CreateProduct = $"{BaseEndpoint}";

        internal static string UpdateProductBasicDetails(int productId) =>
            $"{BaseEndpoint}/{productId}/basic-details";

        internal static string AddNewStock(int productId, int quantity) =>
            $"{BaseEndpoint}/{productId}/available-stock/add/{quantity}";

        internal static string RemoveFromStock(int productId, int quantity) =>
            $"{BaseEndpoint}/{productId}/available-stock/remove/{quantity}";
    }
}

