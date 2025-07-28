namespace Backend.DAO.Abstractions.Exceptions;

public class InvalidYouTubeUrlException(string url)
    : Exception($"The provided URL is not a valid YouTube video URL: '{url}'");