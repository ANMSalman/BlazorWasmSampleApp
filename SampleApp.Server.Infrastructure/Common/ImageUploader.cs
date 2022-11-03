using SampleApp.Server.Application.Common;

namespace SampleApp.Server.Infrastructure.Common;
internal class ImageUploader : IImageUploader
{
    public async Task<string> UploadImageAsync(string base64ImageData, CancellationToken cancellationToken = default)
    {
        //Uploads an Image
        await Task.CompletedTask;

        //randomName
        return Guid.NewGuid().ToString() + ".jpg";
    }

    public async Task DeleteImageAsync(string imageUrl, CancellationToken cancellationToken = default)
    {
        //Deletes an Image
        await Task.CompletedTask;
    }
}
