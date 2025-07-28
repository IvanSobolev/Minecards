using System.Text.RegularExpressions;
using Backend.DAO.Abstractions.Exceptions;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementation;

public class YouTubeThumbnailService(IHttpClientFactory httpClientFactory, ILogger<YouTubeThumbnailService> logger)
    : IYouTubeThumbnailService
{
    private static readonly Regex YouTubeVideoRegex =
        new(@"(?:https?:\/\/)?(?:www\.)?(?:(?:youtube\.com\/(?:(?:watch\?v=)|(?:embed\/)|(?:v\/)))|(?:youtu\.be\/))([a-zA-Z0-9\-_]{11})", RegexOptions.Compiled);

    public async Task<string> GetThumbnailUrlAsync(string videoUrl)
    {
        logger.LogDebug("Trying to recover video ID from URL: {VideoUrl}", videoUrl);

        var match = YouTubeVideoRegex.Match(videoUrl);
        if (!match.Success)
        {
            throw new InvalidYouTubeUrlException(videoUrl);
        }

        var videoId = match.Groups[1].Value;
        logger.LogDebug("Extracted video ID: {VideoId}", videoId);

        var thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg";

        if (!await ThumbnailExistsAsync(thumbnailUrl, videoId))
        {
            throw new YouTubeVideoNotFoundException(videoId);
        }

        return thumbnailUrl;
    }

    private async Task<bool> ThumbnailExistsAsync(string thumbnailUrl, string videoId)
    {
        var client = httpClientFactory.CreateClient();
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Head, thumbnailUrl);
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("No preview found for video {VideoId} at {ThumbnailUrl}, status: {StatusCode}", 
                    videoId, thumbnailUrl, response.StatusCode);
                return false;
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Network error checking video preview {VideoId}", videoId);
            return false;
        }
    }
}