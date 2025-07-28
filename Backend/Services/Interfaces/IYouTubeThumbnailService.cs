namespace Backend.Services.Interfaces;

public interface IYouTubeThumbnailService
{
    /// <summary>
    /// Gets a YouTube video thumbnail URL from a full video URL.
    /// </summary>
    /// <param name="videoUrl">The full URL of the YouTube video (e.g., https://www.youtube.com/watch?v=dQw4w9WgXcQ).</param>
    /// <returns>The URL of the highest quality thumbnail.</returns>
    Task<string> GetThumbnailUrlAsync(string videoUrl);
}