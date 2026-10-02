using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace StockSphere.Infrastructure.Services;

public class SupabaseStorageService : IImageStorageService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SupabaseStorageService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
    {
        var supabaseUrl = _configuration["Supabase:Url"];
        var supabaseKey = _configuration["Supabase:Key"];
        var bucketName = _configuration["Supabase:BucketName"] ?? "products";

        if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            throw new Exception("Supabase credentials are not configured.");

        // Clean up URL
        supabaseUrl = supabaseUrl.TrimEnd('/');
        
        var uploadUrl = $"{supabaseUrl}/storage/v1/object/{bucketName}/{fileName}";

        using var content = new StreamContent(imageStream);
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", supabaseKey);
        _httpClient.DefaultRequestHeaders.Remove("apikey"); // Ensure no duplicate headers
        _httpClient.DefaultRequestHeaders.Add("apikey", supabaseKey);

        var response = await _httpClient.PostAsync(uploadUrl, content);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to upload image to Supabase: {error}");
        }

        // Return public URL
        return $"{supabaseUrl}/storage/v1/object/public/{bucketName}/{fileName}";
    }
}
