namespace StockSphere.Infrastructure.Services;

public interface IImageStorageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType);
}
