using System.Text.Json;

namespace SampleApp.Shared.ResponseModels.Error;
public class ErrorDetailsResponseModel
{
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
