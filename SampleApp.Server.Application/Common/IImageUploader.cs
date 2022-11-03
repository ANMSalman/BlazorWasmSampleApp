namespace SampleApp.Server.Application.Common;
public interface IImageUploader
{
    Task<string> UploadImageAsync(string base64ImageData, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(string imageUrl, CancellationToken cancellationToken = default);
}
