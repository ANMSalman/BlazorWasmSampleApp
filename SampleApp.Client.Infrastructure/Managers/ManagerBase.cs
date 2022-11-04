using SampleApp.Client.Application.Exceptions;
using SampleApp.Client.Application.Managers;
using SampleApp.Shared.ResponseModels.Error;
using System.Net.Http.Json;

namespace SampleApp.Client.Infrastructure.Managers;
internal abstract class ManagerBase<T> : IManagerBase<T>
{
    public List<T> Data { get; protected set; }
    public int CurrentPage { get; protected set; }
    public int PageSize { get; protected set; }
    public int TotalAvailableRecords { get; protected set; }
    public int AvailablePageCount { get; protected set; }

    protected async Task HandleError(HttpResponseMessage response)
    {
        var error = await response.Content.ReadFromJsonAsync<ErrorDetailsResponseModel>();

        throw new ClientException(error?.Message ?? "Something went wrong. Please check your internet connection or try again in a moment.");
    }
}
